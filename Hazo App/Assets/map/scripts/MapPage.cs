using Microsoft.Geospatial;
using Microsoft.Maps.Unity;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Networking;
using System.Collections;
using System.Text.RegularExpressions;
/*
[CustomEditor(typeof(MapPage))]
public class MapPageEditor : Editor
{
public override void OnInspectorGUI()
{
MapPage mapPage = (MapPage)target;

if(GUILayout.Button("Toggle Full screen"))
{
mapPage.toggleSubPage();
}
}
}*/

public class MapPage : MonoBehaviour
{
    public GameObject map;
    public GameObject subPage;
    public GameObject pinPrefab;
    public PageManager pageManager;
    public Page page;
    public GameObject[] mapMenu = new GameObject[2];

    private MapRenderer render;
    private Material m;

    public bool fullScreenMode = false;


    private DateTime start;
    private TimeSpan minTapTime = TimeSpan.FromMilliseconds(100);

    private List<Pin> pins = new List<Pin>();
    private const int pinLayer = 2;

    public Sprite[] icons = new Sprite[5];

    private bool locationsUpdated = false;

    public bool subPageStateChange = false;

    private Pin tmpPin;

    private class Pin
    {
        public GameObject gameObject;

        public int id;
        public LatLon coord;
        // {down vote, up vote}
        public int[] vote = new int[2];
        public int type = 0;
        public int visitors;

        public static AnimationCurve miniAnimationCurve, FullScreenAnimationCurve;

        public Pin(string rawPin, GameObject prefab, Transform parent, Sprite[] icons)
        {
            if(rawPin.Length == 0 || rawPin == null)
            {
                return;
            }
            string[] tmp = rawPin.Split(',');

            id = int.Parse(tmp[0].Split(':')[1]);

            coord = new LatLon(float.Parse(tmp[1].Split(':')[1]), float.Parse(tmp[2].Split(':')[1]));
            type = int.Parse(tmp[3].Split(':')[1]);

            vote[0] = int.Parse(tmp[4].Split(':')[1]);
            vote[1] = int.Parse(tmp[5].Split(':')[1]);
            
            visitors = int.Parse(tmp[6].Split(':')[1]);

            //s = new spriteIndexer();

            //setSymbol(type);

            gameObject = Instantiate(prefab, parent);
            gameObject.GetComponent<MapPin>().ScaleCurve = Pin.miniAnimationCurve;
            gameObject.GetComponent<MapPin>().Altitude = 1;
            gameObject.GetComponent<MapPin>().Location = coord;

            gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = icons[this.type];
        }

        public Pin(Transform parent, GameObject prefab, Sprite icon, int id, LatLon coord, int type, int[] vote, int visitors)
        {
            this.id = id;

            this.coord = coord;
            this.type = type;

            this.vote = vote;
            this.visitors = visitors;
            
            gameObject = Instantiate(prefab, parent);
            gameObject.GetComponent<MapPin>().ScaleCurve = Pin.miniAnimationCurve;
            gameObject.GetComponent<MapPin>().Altitude = 1;
            gameObject.GetComponent<MapPin>().Location = coord;

            gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = icon;
        }
        public void setPinIcon(Sprite icon, int type)
        {
            gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = icon;
            this.type = type;
        }
        public string toString()
        {
            return $"id:{id}\tcoord:{coord}\tvoteUp:{vote[0]}\tvoteDown:{vote[1]}\tvisitors:{visitors}";
        }
    }

    public void setPage(Vector2 screenSize)
    {
        Vector3 pos;
        render = map.GetComponent<MapRenderer>();

        m = render.TerrainMaterial;
        //sets current postion of the map
        LatLon center = new LatLon(Input.location.lastData.latitude, Input.location.lastData.longitude);
        //Input.location.Stop();
        map.GetComponent<MapRendererBase>().Center = center;

        //sets up map
        setMapSize(Mathf.Min(screenSize.x, screenSize.y));
        setMapPosition(screenSize);

        //sets up subpage
        float scaleFactor = 30f / 517.5f * screenSize.y / 20;
        subPage.GetComponent<RectTransform>().sizeDelta = screenSize;
        subPage.transform.GetChild(0).transform.localScale = new Vector3(scaleFactor * 1.5f, scaleFactor * 1.5f, scaleFactor * 1.5f);
        subPage.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(scaleFactor * 1.5f * 20 * 0.8f, -1 * (scaleFactor * 1.5f * 20 * 0.8f));

        Keyframe frame0 = new Keyframe(0, 20f / 475.5f * screenSize.y / map.transform.localScale.x * 20 * 40f);
        Keyframe frame1 = new Keyframe(18, 20f / 475.5f * screenSize.y / map.transform.localScale.x * 20 * 0.5f);
        Keyframe frame2 = new Keyframe(20, 20f / 475.5f * screenSize.y / map.transform.localScale.x * 20 * 0.25f);
        Pin.miniAnimationCurve = new AnimationCurve(frame0, frame1, frame2);

        frame0 = new Keyframe(0, 20f / 475.5f * screenSize.y / Mathf.Max(screenSize.x, screenSize.y) * 20 * 40);
        frame2 = new Keyframe(20, 20f / 475.5f * screenSize.y / Mathf.Max(screenSize.x, screenSize.y) * 20 * 0.5f);

        for(int i1 = 0; i1 < mapMenu.Length; i1++)
        {
            mapMenu[i1].GetComponent<MapMenu>().setUpPage(screenSize);
            pos = mapMenu[i1].GetComponent<RectTransform>().anchoredPosition;


            //mapMenu[i1].GetComponent<RectTransform>().anchoredPosition = pos;
        }

        Pin.FullScreenAnimationCurve = new AnimationCurve(frame0, frame2);

        pins.Add(new Pin(map.transform, pinPrefab, icons[0], -1, center, 0, null, 1));
    }

    private void setMapSize(float screenSize)
    {
        float scaleFactor = screenSize * 0.7f;
        map.transform.localScale = new Vector3(scaleFactor, 1, scaleFactor);
    }

    private void setMapPosition(Vector2 screenSize)
    {
        Vector2 pos = map.transform.GetComponent<RectTransform>().anchoredPosition;
        pos.y = screenSize.y/2 - 40f / 517.5f * screenSize.y - Mathf.Min(screenSize.x, screenSize.y) * 0.7f / 2 - screenSize.y/20;
        map.transform.GetComponent<RectTransform>().anchoredPosition = pos;
    }

    IEnumerator getPins(float radius, Action<UnityWebRequest> callback)
    {
        LatLon coord = map.GetComponent<MapRenderer>().Center;
        using (UnityWebRequest request = UnityWebRequest.Get($"http://3.97.134.252//index.php?lat={coord.LatitudeInDegrees}&lon={coord.LongitudeInDegrees}&rad={radius}"))
        {
            // Send the request and wait for a response
            yield return request.SendWebRequest();
            callback(request);
        }
    }
    public void getPinsResult(float radius)
    {
        StartCoroutine(getPins(radius, (UnityWebRequest req) =>
                {
                    if (req.isNetworkError || req.isHttpError)
                    {
                        Debug.Log($"{req.error}: {req.downloadHandler.text}");
                    }
                    else
                    {
                        string[] tmp = Regex.Split(req.downloadHandler.text,"<br>");
                        for(int i1 =0; i1 < tmp.Length; i1++)
                        {
                            if(tmp[i1].Length != 0)
                            {
                                pins.Add(new Pin(Regex.Replace(tmp[i1], "\n", ""), pinPrefab, map.transform, icons));
                            }

                        }
                        map.GetComponent<MapRenderer>().ZoomLevel = 18;
                    }
                }
            )
        );
    }
    void Update()
    {
        if(map.GetComponent<MapRenderer>().IsLoaded && !locationsUpdated)
        {
            float radius;
            LatLon center = new LatLon(Input.location.lastData.latitude, Input.location.lastData.longitude);

            radius = Mathf.Abs(Convert.ToSingle(map.GetComponent<MapRenderer>().Bounds.BottomLeft.LatitudeInDegrees - map.GetComponent<MapRenderer>().Bounds.TopRight.LatitudeInDegrees));
            radius /= 2;


            getPinsResult(radius);
            locationsUpdated = true;
        }
        Vector4 mapBorder = Vector4.zero;
        if(fullScreenMode)
        {
            mapBorder.x = pageManager.getScreenSize().x * pageManager.GetComponent<RectTransform>().localScale.x / -2;
            mapBorder.y = pageManager.getScreenSize().y * pageManager.GetComponent<RectTransform>().localScale.z / -2;

            mapBorder.w = pageManager.getScreenSize().x * pageManager.GetComponent<RectTransform>().localScale.x / 2;
            mapBorder.z = pageManager.getScreenSize().y * pageManager.GetComponent<RectTransform>().localScale.z / 2;
        }
        else
        {
            mapBorder.x = map.transform.position.x - 0.5f * render.MapDimension.x;
            mapBorder.y = map.transform.position.y - 0.5f * render.MapDimension.y;
            mapBorder.w = map.transform.position.x + 0.5f * render.MapDimension.x;
            mapBorder.z = map.transform.position.y + 0.5f * render.MapDimension.y;
        }

        m.SetVector("border", mapBorder);

        render.TerrainMaterial = m;

        //set current location on map
        
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 pos1 = touch.position;
            Vector2 pos2 = touch.rawPosition;

            pos1.x -= pageManager.getScreenSize().x / 2;
            pos1.y -= pageManager.getScreenSize().y / 2;

            pos2.x -= pageManager.getScreenSize().x / 2;
            pos2.y -= pageManager.getScreenSize().y / 2;

            pos1 *= pageManager.GetComponent<RectTransform>().localScale.x;
            pos2 *= pageManager.GetComponent<RectTransform>().localScale.x;

            if (touch.phase == TouchPhase.Began)
            {
                start = DateTime.UtcNow;
            }
            if (mapBorder.x <= pos1.x && pos1.x <= mapBorder.w)
            {
                if (mapBorder.y <= pos1.y && pos1.y <= mapBorder.z)
                {
                    if (!fullScreenMode && touch.phase == TouchPhase.Ended && DateTime.UtcNow - start < minTapTime)
                    {
                        toggleSubPage();
                    }
                }
            }
            else
            {
                page.active = false;
            }

            if (mapBorder.x <= pos2.x && pos2.x <= mapBorder.w)
            {
                if (mapBorder.y <= pos2.y && pos2.y <= mapBorder.z)
                {
                    page.active = true;
                }
            }
            else
            {
                page.active = false;
            }
        }
        //updatePins();
        
        if(tmpPin != null && mapMenu[0].GetComponent<MapMenu>().active)
        {
            switch(mapMenu[0].GetComponent<MapMenu>().elements[1].GetComponent<Dropdown>().value)
            {
                case 0:
                    tmpPin.setPinIcon(icons[1], 1);
                    break;
                case 1:
                    tmpPin.setPinIcon(icons[2], 2);
                    break;
            }
        }

        if(subPageStateChange)
        {
            toggleSubPage();
            subPageStateChange = false;
        }
    }

    public void toggleSubPage()
    {
        float scaleFactor;
        Vector2 screenSize = pageManager.getScreenSize();
        if (subPage.activeSelf)
        {
            scaleFactor = Mathf.Min(screenSize.x , screenSize.y) * 0.7f;
            map.transform.localScale = new Vector3(scaleFactor, 1, scaleFactor);
            Vector2 pos = map.transform.GetComponent<RectTransform>().anchoredPosition;
            pos.y = screenSize.y / 2 - 40f / 517.5f * screenSize.y - Mathf.Min(screenSize.x, screenSize.y) * 0.7f / 2 - screenSize.y / 20;
            map.transform.GetComponent<RectTransform>().anchoredPosition = pos;
            page.active = false;

            this.GetComponentInChildren<MapPin>().ScaleCurve = Pin.miniAnimationCurve;
            clearTmpPin();
        }
        else
        {
            scaleFactor = Mathf.Max(screenSize.x, screenSize.y);
            map.transform.localScale = new Vector3(scaleFactor, 1, scaleFactor);
            map.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            page.active = true;
            this.GetComponentInChildren<MapPin>().ScaleCurve = Pin.FullScreenAnimationCurve;
        }

        fullScreenMode = !fullScreenMode;
        subPage.SetActive(!subPage.activeSelf);
        pageManager.subPageActive = !pageManager.subPageActive;
    }

    public void addPin(LatLonAlt coord)
    {
        if (tmpPin == null)
        {
            tmpPin = new Pin(map.transform, pinPrefab, icons[1], -1, coord.LatLon, 0, null, 1);
            mapMenu[0].GetComponent<MapMenu>().active = true;
            map.GetComponent<MapRenderer>().Center = coord.LatLon;
            Handheld.Vibrate();
        }
    }

    private IEnumerator addPin(Action<UnityWebRequest> callback)
    {
        Debug.Log("Sending result");
        using (UnityWebRequest request = UnityWebRequest.Get($"http://3.97.134.252//addLocation.php?lat={tmpPin.coord.LatitudeInDegrees}&lon={tmpPin.coord.LongitudeInDegrees}&type={tmpPin.type}"))
        {
            // Send the request and wait for a response
            yield return request.SendWebRequest();
            callback(request);
        }
    }
    public void addPin()
    {
        StartCoroutine(addPin((UnityWebRequest req) =>
            {
                Debug.Log("Got result");
                if (req.isNetworkError || req.isHttpError)
                {
                    Debug.Log($"{req.error}: {req.downloadHandler.text}");
                }
                else
                {
                    string[] tmp = Regex.Split(req.downloadHandler.text, "<br>");
                    for (int i1 = 0; i1 < tmp.Length; i1++)
                    {
                        if(tmp[i1].Split(':')[0] == "error")
                        {
                            Debug.Log(tmp[i1]);
                        }
                        else if(tmpPin != null)
                        {
                            pins.Add(tmpPin);
                            tmpPin = null;
                            mapMenu[0].GetComponent<MapMenu>().active = false;
                        }
                    }
                }
            }
            )
        );
    }
    public void selectPin(LatLonAlt pos)
    {

    }

    public void clearTmpPin()
    {
        if(tmpPin != null)
        {
            Destroy(tmpPin.gameObject);
            tmpPin = null;
            mapMenu[0].GetComponent<MapMenu>().active = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 0.1f));
    }
}

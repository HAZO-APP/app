using Microsoft.Geospatial;
using Microsoft.Maps.Unity;
using System;
using UnityEngine;

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

    private MapRenderer render;
    private Material m;

    public bool fullScreenMode = false;


    private DateTime start;
    private TimeSpan minTapTime = TimeSpan.FromMilliseconds(100);

    private List<Pin> pins = new List<Pin>();
    private const int pinLayer = 2;

    public Sprite[] icons = new Sprite[5];
    public bool locationsUpdated = false;
    private class Pin
    {
        public GameObject gameObject;

        int id;
        LatLon coord;
        // {down vote, up vote}
        int[] vote = new int[2];
        int type = 0;
        int visitors;

        public static AnimationCurve miniAnimationCurve, FullScreenAnimationCurve;

        public Pin(string rawPin, GameObject prefab, Transform parent, Sprite icon)
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

            gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = icon;
        }
        public string toString()
        {
            return $"id:{id}\tcoord:{coord}\tvoteUp:{vote[0]}\tvoteDown:{vote[1]}\tvisitors:{visitors}";
        }
    }

    public void setPage(Vector2 screenSize)
    {
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

        Pin.FullScreenAnimationCurve = new AnimationCurve(frame0, frame2);

        
        pins.Add(new Pin($"id:-1,lat:{center.LatitudeInDegrees},lon:{center.LongitudeInDegrees},type:{0},upVote:{-1},downVote:{-1},visitors:{-1}", pinPrefab, map.transform, icons[0]));

        //Debug.Log(pins[pins.Count - 1].toString());

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

    IEnumerator GetRequest(LatLon coord, float radius, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get($"http://3.97.134.252//index.php?lat={coord.LatitudeInDegrees}&lon={coord.LongitudeInDegrees}&rad={radius}"))
        {
            // Send the request and wait for a response
            yield return request.SendWebRequest();
            callback(request);
        }
    }
    public void GetPosts(LatLon coord, float radius)
    {
        StartCoroutine(GetRequest(coord, radius, (UnityWebRequest req) =>
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
                                
                                pins.Add(new Pin(Regex.Replace(tmp[i1], "\n", ""), pinPrefab, map.transform, icons[1]));
                                map.GetComponent<MapPinLayer>().MapPins.Add(pins[pins.Count - 1].gameObject.GetComponent<MapPin>());
                                Debug.Log(pins[pins.Count - 1].toString()); ;
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


            GetPosts(center, radius);
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

        subPage.SetActive(fullScreenMode);
        //updatePins();
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
        }
        else
        {
            scaleFactor = Mathf.Max(screenSize.x, screenSize.y);
            map.transform.localScale = new Vector3(scaleFactor, 1, scaleFactor);
            map.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            page.active = true;
            this.GetComponentInChildren<MapPin>().ScaleCurve = Pin.FullScreenAnimationCurve;
        }

        subPage.SetActive(!subPage.activeSelf);
        fullScreenMode = !fullScreenMode;
        pageManager.subPageActive = !pageManager.subPageActive;

    }


    /*void removePin(string id)
    {
        pins.RemoveAt(pins.FindIndex(x => x.GetComponent<MapPin>().id == id));
    }*/

    void removePin(int index)
    {
        pins.RemoveAt(index);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 0.1f));
    }
}

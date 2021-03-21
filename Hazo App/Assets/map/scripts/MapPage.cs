using Microsoft.Geospatial;
using Microsoft.Maps.Unity;
using System;
using UnityEngine;

using UnityEditor;
using System.Collections.Generic;
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

    private List<GameObject> pins = new List<GameObject>();
    private const int pinLayer = 2;
    
    public void setPage(Vector2 screenSize)
    {
        render = map.GetComponent<MapRenderer>();

        m = render.TerrainMaterial;
        //sets current postion of the map
        LatLonAlt center = new LatLonAlt(Input.location.lastData.latitude, Input.location.lastData.longitude, 0);
        //Input.location.Stop();
        map.GetComponent<MapRendererBase>().Center = center.LatLon;

        //sets up map
        setMapSize(Mathf.Min(screenSize.x, screenSize.y));
        setMapPosition(screenSize);

        //sets up subpage
        float scaleFactor = 30f / 517.5f * screenSize.y / 20;
        subPage.GetComponent<RectTransform>().sizeDelta = screenSize;
        subPage.transform.GetChild(0).transform.localScale = new Vector3(scaleFactor * 1.5f, scaleFactor * 1.5f, scaleFactor * 1.5f);
        subPage.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(scaleFactor * 1.5f * 20 * 0.8f, -1 * (scaleFactor * 1.5f * 20 * 0.8f));

        //addPin(center, "CurrentPos", 0);
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

    void Update()
    {
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
        }
        else
        {
            scaleFactor = Mathf.Max(screenSize.x, screenSize.y);
            map.transform.localScale = new Vector3(scaleFactor, 1, scaleFactor);
            map.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            page.active = true;
        }

        subPage.SetActive(!subPage.activeSelf);
        fullScreenMode = !fullScreenMode;
        pageManager.subPageActive = !pageManager.subPageActive;

    }


    void addPin(LatLonAlt coord, string id, int symbol)
    {
        GameObject tmp = Instantiate(pinPrefab, map.transform);

        MapPin mapPin = tmp.GetComponent<MapPin>();
        Vector3 pos = render.TransformLatLonAltToLocalPoint(coord);// + map.transform.position;

        pos.z = pos.y;
        pos.y = pinLayer;

        tmp.transform.localPosition = pos;
        tmp.transform.eulerAngles = new Vector3(Mathf.PI / 2, 0, 0);
        tmp.transform.localScale = new Vector3(Vector3.one.x / map.transform.localScale.x, Vector3.one.x / map.transform.localScale.x, Vector3.one.x / map.transform.localScale.x) * Mathf.Clamp(map.GetComponent<MapRendererBase>().ZoomLevel / 5, 0.5f, 4);

        mapPin.id = id;
        mapPin.coord = coord;
        mapPin.setSymbol(symbol);
        pins.Add(tmp);
    }

    void removePin(string id)
    {
        pins.RemoveAt(pins.FindIndex(x => x.GetComponent<MapPin>().id == id));
    }

    void removePin(int index)
    {
        pins.RemoveAt(index);
    }

    void updatePin()
    {
        GameObject tmp;
        Vector3 pos;
        for(int i1 = 0; i1 < pins.Count; i1++)
        {
            tmp = pins[i1];
            pos = render.TransformLatLonAltToLocalPoint(tmp.GetComponent<MapPin>().coord);
            pos.z = pinLayer;
            tmp.transform.position = pos;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 0.1f));
    }
}

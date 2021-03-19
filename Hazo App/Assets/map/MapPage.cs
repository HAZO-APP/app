using Microsoft.Maps.Unity;
using UnityEngine;
/*
using UnityEditor;

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
    public PageManager pageManager;

    private MapRenderer render;
    private Material m;

    public bool fullScreenMode = false;
    public bool test = false;
    
    // Start is called before the first frame update
    void Start()
    {
        render = map.GetComponent<MapRenderer>();
        m = render.TerrainMaterial;
    }

    public void setPage(Vector2 screenSize)
    {
        //sets up map
        setMapSize(Mathf.Min(screenSize.x, screenSize.y));
        setMapPosition(screenSize);

        //sets up subpage
        float scaleFactor = 30f / 517.5f * screenSize.y / 20;
        subPage.GetComponent<RectTransform>().sizeDelta = screenSize;
        subPage.transform.GetChild(0).transform.localScale = new Vector3(scaleFactor * 1.5f, scaleFactor * 1.5f, scaleFactor * 1.5f);
        subPage.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(scaleFactor * 1.5f * 20 * 0.8f, -1 * (scaleFactor * 1.5f * 20 * 0.8f));
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

        if (Input.touchCount == 1)
        {
            if (!fullScreenMode)
            {
                Touch touch = Input.GetTouch(0);
                Vector2 pos = touch.position;
                pos.x -= pageManager.getScreenSize().x / 2;
                pos.y -= pageManager.getScreenSize().y / 2;

                pos *= pageManager.GetComponent<RectTransform>().localScale.x;

                if (mapBorder.x <= pos.x && pos.x <= mapBorder.w)
                {
                    if (mapBorder.y <= pos.y && pos.y <= mapBorder.z)
                    {
                        toggleSubPage();
                    }
                }
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
        }
        else
        {
            scaleFactor = Mathf.Max(screenSize.x, screenSize.y);
            map.transform.localScale = new Vector3(scaleFactor, 1, scaleFactor);
            map.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        }

        subPage.SetActive(!subPage.activeSelf);
        fullScreenMode = !fullScreenMode;
        pageManager.subPageActive = !pageManager.subPageActive;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 0.1f));
    }
}

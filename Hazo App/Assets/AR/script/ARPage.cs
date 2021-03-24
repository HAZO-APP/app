using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARPage : MonoBehaviour
{
    public GameObject ARCamera;
    public GameObject projector;
    public PageManager pageManager;
    
    private bool ARCameraSet = true;
    private int frames = 0;
    private Vector4 cameraBorder;

    private Material m;

    public void Start()
    {
        m = projector.GetComponent<MeshRenderer>().material;
    }

    public void setPage(Vector2 screenSize)
    {
        Vector3 scale = Vector3.one * Mathf.Max(screenSize.x, screenSize.y) / 10;
        Vector3 pos = new Vector3(screenSize.x / 2, 0, screenSize.y / 2);

        scale.z *= -1;

        projector.transform.localScale = scale;
        projector.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    public void Update()
    {
        if(ARCamera.transform.childCount > 0 && ARCameraSet && frames >= 10)
        {
            Transform tmpGameObject = ARCamera.transform.GetChild(0);

            Vector3 pos;
            Vector3 scale;

            pos = Vector3.zero;
            pos.z = 2;

            scale = Vector3.one;

            tmpGameObject.localPosition = pos;
            tmpGameObject.localScale = scale;

            ARCameraSet = false;
        }
        else if(frames < 10)
        {
            frames++;
        }

        cameraBorder.x = pageManager.getScreenSize().x * pageManager.GetComponent<RectTransform>().localScale.x / -2 + this.GetComponent<RectTransform>().anchoredPosition.x * pageManager.GetComponent<RectTransform>().localScale.x;
        cameraBorder.y = pageManager.getScreenSize().y * pageManager.GetComponent<RectTransform>().localScale.z / -2;

        cameraBorder.w = pageManager.getScreenSize().x * pageManager.GetComponent<RectTransform>().localScale.x / 2 + this.GetComponent<RectTransform>().anchoredPosition.x * pageManager.GetComponent<RectTransform>().localScale.x;
        cameraBorder.z = pageManager.getScreenSize().y * pageManager.GetComponent<RectTransform>().localScale.z / 2;

        m.SetVector("border", cameraBorder);

        projector.GetComponent<MeshRenderer>().material = m;

    }
}

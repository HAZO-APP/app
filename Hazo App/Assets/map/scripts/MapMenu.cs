using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMenu : MonoBehaviour
{
    public GameObject[] elements = new GameObject[0] { };

    public GameObject backdrop;

    public bool active = false;

    private AnimationCurve easeIntOut = AnimationCurve.EaseInOut(0,0,1,-1);
    private float state = 1;

    public void setUpPage(Vector2 screenSize)
    {
        //set background
        Vector3 pos;
        Vector3 scale;
        Vector3 size;

        //backdrop size
        size = backdrop.transform.GetComponent<RectTransform>().sizeDelta;

        size.x = screenSize.x;
        size.y = screenSize.y / 10;

        backdrop.transform.GetComponent<RectTransform>().sizeDelta = size;

        backdrop.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(screenSize.x, 0, 0);

        //buttons
        for (int i1 = 0; i1 < elements.Length; i1++)
        {

            pos = elements[i1].GetComponent<RectTransform>().anchoredPosition;
            size = elements[i1].GetComponent<RectTransform>().sizeDelta;
            scale = elements[i1].GetComponent<RectTransform>().localScale;

            switch(elements[i1].tag)
            {
                case "button":
                    scale.x = backdrop.transform.GetComponent<RectTransform>().sizeDelta.y * 0.8f / 25;
                    scale.y = backdrop.transform.GetComponent<RectTransform>().sizeDelta.y * 0.8f / 25;
                    break;
                case "dropDown":
                    
                    size.x = backdrop.transform.GetComponent<RectTransform>().sizeDelta.y / 2 * 4.5f;
                    size.y = backdrop.transform.GetComponent<RectTransform>().sizeDelta.y * 0.75f;
                    break;
            }
            
            switch(i1)
            {
                case 0:
                    pos.x = screenSize.x / 12 + backdrop.transform.GetComponent<RectTransform>().sizeDelta.y / 2;
                    break;
                case 1:
                    pos.x = screenSize.x / 2;
                    break;
                case 2:
                    pos.x = screenSize.x - (screenSize.x / 12 + backdrop.transform.GetComponent<RectTransform>().sizeDelta.y / 2);
                    break;
            }

            pos.y = backdrop.transform.GetComponent<RectTransform>().sizeDelta.y / 2;
            pos.z = -1;

            elements[i1].GetComponent<RectTransform>().anchoredPosition = pos;
            elements[i1].GetComponent<RectTransform>().sizeDelta = size;
            elements[i1].GetComponent<RectTransform>().localScale = scale;
        }

        pos = this.GetComponent<RectTransform>().anchoredPosition;
        pos.y = -1 * backdrop.transform.GetComponent<RectTransform>().sizeDelta.y;
        this.GetComponent<RectTransform>().anchoredPosition = pos;
    }

    public void Update()
    {
        Vector3 pos = this.GetComponent<RectTransform>().anchoredPosition;
        if(active)
        {
            pos.z = -3;
        }
        else
        {
            pos.z = -2;
        }

        if(active && state > 0.01)
        {
            state -= Time.deltaTime;
        }
        else if(!active && state < 1 - 0.01)
        {
            state += Time.deltaTime;
        }
        else
        {
            if(state > 0.5)
            {
                state = 1;
            }
            else
            {
                state = 0;
            }
        }

        pos.y = backdrop.transform.GetComponent<RectTransform>().sizeDelta.y * easeIntOut.Evaluate(state);
        this.GetComponent<RectTransform>().anchoredPosition = pos;
    }
}

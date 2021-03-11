using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

public class menuScript : MonoBehaviour
{
    // Start is called before the first frame update
    private PageManager pageManager;
    public GameObject pageManagerGameObject;
    public GameObject[] buttons = new GameObject[3];

    private static string STATE="Vector1_D5066601";
    private Material m;

    private Vector3 startScale;
    private Vector3 SelectedScale;
    
    void Start()
    {
        float scaleFactor = 30f / 517.5f;
        Vector3 pos = this.GetComponent<RectTransform>().anchoredPosition;

        pageManager = pageManagerGameObject.GetComponent<PageManager>();
        setButtonPos(pageManager.GetComponent<RectTransform>().sizeDelta.x) ;

        scaleFactor *= (float) pageManager.GetComponent<RectTransform>().sizeDelta.y;

        pos.y = scaleFactor + 10;
        this.GetComponent<RectTransform>().anchoredPosition = pos;

        scaleFactor /= 20f;

        Debug.Log(scaleFactor);
        
        this.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }


    // Update is called once per frame
    void Update()
    {
        updateButton();
    }

    void updateButton()
    {
        GameObject button;
        float buttonState;
        Vector3 pos;
        for (int i1 = 0; i1 < buttons.Length; i1 += 1)
        {
            button = buttons[i1];

            m = button.GetComponent<SpriteRenderer>().material;
            pos = button.GetComponent<RectTransform>().anchoredPosition;
            buttonState = m.GetFloat(STATE);

            if (i1 == pageManager.getClosestPage())
            {
                if(buttonState > 0)
                {
                    buttonState = stateJump(-1, buttonState);
                    if(buttonState < 0)
                    {
                        buttonState = 0;
                    }
                    m.SetFloat(STATE, buttonState);
                }
            }
            else
            {
                if (buttonState < 1)
                {
                    buttonState = stateJump(1, buttonState);
                    if (buttonState > 1)
                    {
                        buttonState = 1;
                    }
                    m.SetFloat(STATE, buttonState);
                }
            }

            pos.y = -10f * buttonState;
            button.GetComponent<RectTransform>().anchoredPosition = pos;
            button.transform.localScale = new Vector3(1 + (1-buttonState), 1 + (1 - buttonState), 1);
            button.GetComponent<SpriteRenderer>().material = m;
        }
    }

    public void changeToMap()
    {
        
        if (pageManager.getClosestPage() != 2)
        {
            pageManager.pageGoal = 2;
        }
    }
    public void changeToAR()
    {
        if (pageManager.getClosestPage() != 1)
        {
            pageManager.pageGoal = 1;
        }
    }
    public void changeToProfile()
    {
        
        if (pageManager.getClosestPage() != 0)
        {
            pageManager.pageGoal = 0;
        }
    }

    private float stateJump(int direction, float start)
    {
        float tmp = direction * 1 * Time.deltaTime + start;

        return tmp;
    }

    private void setButtonPos(float size)
    {
        Vector3 temp;
        int start = -1;

        for (int i1 = 0; i1 < buttons.Length; i1++)
        {
            temp = buttons[i1].GetComponent<RectTransform>().anchoredPosition;
            temp.x = (start + i1) * size/4;
            buttons[i1].GetComponent<RectTransform>().anchoredPosition = temp;
        }

    }
}

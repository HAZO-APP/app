using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageManager : MonoBehaviour
{
    public int pageGoal = 1;
    public GameObject[] pages = new GameObject[3];
    public float pagePosition = 1;
    private Vector2 screenSize;


    public GameObject header;
    // Start is called before the first frame update
    void Start()
    {
        Transform headerBox, headerLogo;
        Vector3 scale;

        screenSize = new Vector2();
        screenSize.x = this.GetComponent<RectTransform>().sizeDelta.x;
        screenSize.y = this.GetComponent<RectTransform>().sizeDelta.y;


        for (int i1 = 0; i1 < pages.Length; i1++)
        {
            pages[i1].GetComponent<RectTransform>().sizeDelta = screenSize;
        }

        setPagesPos(1);
        
        headerLogo = header.transform.GetChild(0);
        headerLogo.localScale = new Vector3(40f / 517.5f * screenSize.y / 10, 40f / 517.5f * screenSize.y / 10, 1);
        
        headerBox = header.transform.GetChild(1);
        headerBox.GetComponent<RectTransform>().localScale = new Vector3(screenSize.x, 40f / 517.5f * screenSize.y, 1);

        header.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -1 * 40f / 517.5f * screenSize.y / 2, 0);
    }

    public Vector2 getScreenSize()
    {
        return new Vector2(screenSize.x,screenSize.x);
    }

    public float getClosestPage()
    {
        return Mathf.Round(pagePosition);
    }

    private void updatePagesPos(float change)
    {
        Vector3 temp;

        for (int i1 = 0; i1 < pages.Length; i1++)
        {
            temp = pages[i1].GetComponent<RectTransform>().anchoredPosition;
            temp.x += change;
            pages[i1].GetComponent<RectTransform>().anchoredPosition = temp;
        }
    }
    private void setPagesPos(int frontPageIndex)
    {
        Vector3 temp;
        int start = 0 - frontPageIndex;

        for(int i1 = 0; i1 < pages.Length; i1++)
        {
            temp = pages[i1].GetComponent<RectTransform>().anchoredPosition;
            temp.x = (start + i1) * screenSize.x;
            pages[i1].GetComponent<RectTransform>().anchoredPosition = temp;
        }

    }

    // Update is called once per frame
    void Update()
    {
        float direction = 0f;
        float jump = 0.5f * Time.deltaTime;
        float tempGoal = (float)pageGoal;
        Vector3 pos;

        //right of goal
        if (pagePosition > tempGoal + 0.05)
        {
            direction = -1f;
        }
        //left of goal
        else if(pagePosition < tempGoal - 0.05)
        {
            direction = 1f;
        }
        
        if(pagePosition != tempGoal && direction == 0)
        {
            setPagesPos(pageGoal);
            pagePosition = pageGoal;
        }
        else
        {
            for (int i1 = 0; i1 < pages.Length; i1++)
            {
                pos = pages[i1].transform.position;

                pos.x += direction * jump;

                pagePosition += direction * jump;

                updatePagesPos(-1*direction * jump * screenSize.x);
            }
        }
        
        
    }
}

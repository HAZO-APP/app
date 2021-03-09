using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;

public class menuScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int mainPage = 0;
    public GameObject[] buttons = new GameObject[3];
    void Start()
    {
        GameObject temp;
        Material m;

        for(int i1 = 0; i1 < buttons.Length; i1+=1 )
        {
            temp = buttons[i1];

            m = temp.GetComponent<SVGImage>().material;
            if(i1 == mainPage - 1)
            {
                m.SetFloat("state", 0);
            }
            else
            {
                m.SetFloat("state", 1);
            }

            temp.GetComponent<SVGImage>().material = m;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using Microsoft.Geospatial;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPinData : MonoBehaviour
{
    public GameObject symbol;

    public Sprite[] sprites;

    public GameObject Map;

    public string id;
    public void setSymbol(int index)
    {
        /*if(index < 0 || sprites.Length <= index)
        {
            throw new System.ArgumentException("Invalid argument value");
        }*/

        //symbol.GetComponent<SpriteRenderer>().sprite = sprites[index];
    }

    public void Update()
    {
        //Debug.Log(this.transform.localScale.x);
    }
}

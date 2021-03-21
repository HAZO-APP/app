using Microsoft.Geospatial;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPin : MonoBehaviour
{
    public GameObject symbol;

    public Sprite[] sprites;

    public string id;
    
    public LatLonAlt coord;
    public void setSymbol(int index)
    {
        if(index < 0 || sprites.Length - 1 < index)
        {
            throw new System.ArgumentException("Invalid argument value");
        }

        symbol.GetComponent<SpriteRenderer>().sprite = sprites[index];
    }
}

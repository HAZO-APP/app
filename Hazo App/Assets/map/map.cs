using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Maps.Unity;

public class map : MonoBehaviour
{
    public MapRenderer render;
    private Material m;

    // Start is called before the first frame update
    void Start()
    {
        m = render.TerrainMaterial;
    }

    public void setScale(float screenSize)
    {
        float scaleFactor = screenSize * 0.7f;
        this.transform.localScale = new Vector3(scaleFactor, 1, scaleFactor);
        
    }

    public void setPosition()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector4 temp = Vector4.zero;
        temp.x = this.transform.position.x - 0.5f * render.MapDimension.x;
        temp.y = this.transform.position.y - 0.5f * render.MapDimension.y;
        temp.w = this.transform.position.x + 0.5f * render.MapDimension.x;
        temp.z = this.transform.position.y + 0.5f * render.MapDimension.y;

        m.SetVector("border", temp);

        render.TerrainMaterial = m;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(15, 15, 0.1f));
    }
}

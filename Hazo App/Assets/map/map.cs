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

    public void setPosition(float headerSize, Vector2 screenSize)
    {
        Vector2 pos = this.transform.GetComponent<RectTransform>().anchoredPosition;
        pos.y = screenSize.y/2 - headerSize - Mathf.Min(screenSize.x, screenSize.y) * 0.7f / 2 - 30;
        this.transform.GetComponent<RectTransform>().anchoredPosition = pos;
    }

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
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 0.1f));
    }
}

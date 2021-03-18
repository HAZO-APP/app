using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Maps.Unity;

public class map : MonoBehaviour
{
    public MapRenderer render;
    private Material m;

    private Vector2 size = new Vector2(10,10);
    // Start is called before the first frame update
    void Start()
    {
        m = render.TerrainMaterial;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector4 temp = Vector4.zero;

        temp.x = this.transform.position.x - size.x / 2;
        temp.y = this.transform.position.y - size.y / 2;
        temp.w = this.transform.position.x + size.x / 2;
        temp.z = this.transform.position.y + size.y / 2;

        m.SetVector("border", temp);

        render.TerrainMaterial = m;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(15, 15, 0.1f));
    }
}

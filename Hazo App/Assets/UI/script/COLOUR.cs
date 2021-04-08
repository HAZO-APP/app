using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Colour Palete")]
public class COLOUR : ScriptableObject
{
    public Color[] PRIMARY = new Color[]
    {
        new Color(56, 154, 154),
        new Color(189, 242, 242)
    };

    public Color[] SECONDARY = new Color[]
    {
        new Color(188, 238, 87),
        new Color(234, 253, 198)
    };
}

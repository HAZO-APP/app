using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GlassDropdown : MonoBehaviour
{
    public TextMeshProUGUI output;

    public void HandleInputData(int val)
    {
        if (val == 0)
        {
            output.text = "Please Select";
        }
        if (val == 1)
        {
            output.text = "Recycable, Blue Bin!";
        }
        if (val == 2)
        {
            output.text = "Not recycable, Regular Garbage Bin!";
        }
        if (val == 3)
        {
            output.text = "Not recycable, Regular Garbage Bin!";
        }
        if (val == 4)
        {
            output.text = "Not recycable, Regular Garbage Bin!";
        }
        if (val == 5)
        {
            output.text = "Not recycable, Regular Garbage Bin!";
        }
        if (val == 6)
        {
            output.text = "Not recycable, Regular Garbage Bin!";
        }
        if (val == 7)
        {
            output.text = "Not recycable, can be returned to a Take it Back! partner or a Household Hazardous Waste Depot";
        }
        
    }
}

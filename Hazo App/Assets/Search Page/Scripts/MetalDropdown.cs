﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MetalDropdown : MonoBehaviour
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
            output.text = "Recycable, Blue Bin!";
        }
        if(val == 3)
        {
            output.text = "Recycable, Blue Bin!";
        }
        if(val == 4)
        {
            output.text = "Recycable, Blue Bin!";
        }
        if(val == 5)
        {
            output.text = "Recycable, Blue Bin! Clean before recycling!";
        }
        if(val == 6)
        {
            output.text = "Recycable, Blue Bin! Clean before recycling!";
        }
        if(val == 7)
        {
            output.text = "Recycable, Blue Bin! Make sure it's Empty!";
        }
        if(val == 8)
        {
            output.text = "Recycable, Blue Bin!";
        }
        if (val == 9)
        {
            output.text = "Recycable, Blue Bin!";
        }
        if (val == 10)
        {
            output.text = "Not recycable, Regular Garbage Bin!";
        }
        if (val == 11)
        {
            output.text = "Not recycable, Regular Garbage Bin!";
        }
        if (val == 12)
        {
            output.text = "Not recycable, Regular Garbage Bin!";
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CartonDropdown : MonoBehaviour
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
        if (val == 3)
        {
            output.text = "Recycable, Blue Bin!";
        }
        
    }
}

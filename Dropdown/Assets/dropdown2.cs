using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class dropdown2 : MonoBehaviour
{
    public TextMeshProUGUI output;

    public void HandleInputData(int val)
    {
        if (val==0)
        {
            output.text = "Please Select";
        }
        if (val==1)
        {
            output.text = "Non recycable";
        }
        if (val == 2)
        {
            output.text = "Recycable";
        }
    }
}

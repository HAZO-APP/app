using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;

public class ARPage : MonoBehaviour
{
    public TrackableBehaviour [] trackers;
    public string[] Outputs;
    public Text output;
    public PageManager pageManager;



    public void Start()
    {
    }

    public void setPage(Vector2 screenSize)
    {
    }

    public void Update()
    {
        bool check = false;
        for(int i = 0; i < trackers.Length; i++)
        {
            if(trackers[i].CurrentStatus == TrackableBehaviour.Status.DETECTED || trackers[i].CurrentStatus == TrackableBehaviour.Status.TRACKED || trackers[i].CurrentStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                check = true;
                output.text = Outputs[i];
                break;
            }
        }

        if(check == false)
        {
            output.text = "";
        }
    }
}

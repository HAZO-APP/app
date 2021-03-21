using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Android;

public class loadingScreenHandler : MonoBehaviour
{
    DateTime start;
    TimeSpan minWaitStep = TimeSpan.FromSeconds(4);
    bool setUpComplete = false;
    bool locationStart = false;
    int maxLocationInitializingWaitTime = 20;
    // Start is called before the first frame update
    void Start()
    {
        start = DateTime.UtcNow;
        if (!(Permission.HasUserAuthorizedPermission(Permission.CoarseLocation) || Permission.HasUserAuthorizedPermission(Permission.FineLocation)))
        {
            Debug.Log("No permision");
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        setUpComplete = true;

        if (DateTime.UtcNow - start > minWaitStep)
        {
            if(!locationStart)
            {
                Input.location.Start();
                locationStart = true;
            }
            
            start = DateTime.UtcNow;
            //checks if inilialize occurs
            if(Input.location.status == LocationServiceStatus.Initializing)
            {
                if(maxLocationInitializingWaitTime > 0)
                {
                    maxLocationInitializingWaitTime -= 1;
                    setUpComplete = false;
                }
                else
                {
                    //notifies cannot initialize location
                }
            }
            else if(Input.location.status == LocationServiceStatus.Failed)
            {
                //tells user failed to get location
            }
            
            if (setUpComplete)
            {
                SceneManager.LoadScene(1);
            }
            
        }
    }
}

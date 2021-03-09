using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadingScreenHandler : MonoBehaviour
{
    DateTime start;
    TimeSpan minLoadingTime = TimeSpan.FromSeconds(4);
    // Start is called before the first frame update
    void Start()
    {
        start = DateTime.UtcNow;
    }

    // Update is called once per frame
    void Update()
    {
        if(DateTime.UtcNow - start > minLoadingTime)
        {
            SceneManager.LoadScene("Main Page");
        }
    }
}

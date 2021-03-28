using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARPage : Page
{
    public GameObject ARCamera;
    public PageManager pageManager;

    public void enable()
    {
        ARCamera.transform.gameObject.SetActive(true);
        this.transform.gameObject.SetActive(true);
    }

    public void disable()
    {
        ARCamera.transform.gameObject.SetActive(false);
        this.transform.gameObject.SetActive(false);
    }
}

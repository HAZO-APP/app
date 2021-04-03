using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page : MonoBehaviour
{
    public bool active = false;
    public GameObject subPage;
    public PageManager pageManager;

    public void enable()
    {
        transform.gameObject.SetActive(true);
    }

    public void disable()
    {
        transform.gameObject.SetActive(false);
    }

    public void toggle()
    {
        if (this.transform.gameObject.activeSelf)
        {
            disable();
        }
        else
        {
            enable();
        }
    }

    public void toggleSubPage()
    {
        subPage.SetActive(!subPage.activeSelf);

        pageManager.subPageActive = !pageManager.subPageActive;

        pageManager.toggleMenu();
    }
}

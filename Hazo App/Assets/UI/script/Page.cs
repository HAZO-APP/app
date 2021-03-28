using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page : MonoBehaviour
{
    public bool active = false;

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
        if(this.transform.gameObject.active)
        {
            disable();
        }
        else
        {
            enable();
        }
    }
}

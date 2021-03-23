using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class popupwindoow : MonoBehaviour
{
    public string loadMetal;

    public GameObject MetalList;
    public GameObject PlasticList;
    public GameObject GlassList;
    public GameObject PaperList;
    public GameObject AlcoholList;
    public GameObject CartonList;

    public void metal()
    {
        MetalList.SetActive(true);
    }
    public void glass()
    {
        GlassList.SetActive(true);
    }
    public void plastic()
    {
        PlasticList.SetActive(true);
    }
    public void paper()
    {
        PaperList.SetActive(true);
    }
    public void alcohol()
    {
        AlcoholList.SetActive(true);
    }
    public void carton()
    {
        CartonList.SetActive(true);
    }

    public void close()
    {
        SceneManager.LoadScene(4);
    }
}

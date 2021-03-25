using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{

    public void Map()
    {

        SceneManager.LoadScene(1);
    }

  
    public void Exit()
    {

        EditorApplication.isPlaying = false;
        Application.Quit();
        //Debug.Break();
    }
    public void Back()
    {

        SceneManager.LoadScene(0);

    }
    public void Recycle()
    {

        SceneManager.LoadScene(2);
    }


}

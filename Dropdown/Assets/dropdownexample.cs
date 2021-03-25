using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dropdownexample : MonoBehaviour
{
    List<string> items = new List<string>() { "Coffee Cup", "Glass Bottle", "Pizza Box", "Water Bottle" };

    public Dropdown dropdown;
    public Text selectedName;

    public void Dropdown_IndexChanged(int index)
    {
        items[0]= "recycable";
    }

    void Start()
    {
        PopulateList();
    }


    void PopulateList()
   {
       
        dropdown.AddOptions(items);

    }
        

}
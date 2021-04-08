using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SearchPage : Page
{
    private class ButtonMessages
    {
        public string[] dropDown;
        public string[] messages;
        public List<Dropdown.OptionData> menuItems = new List<Dropdown.OptionData>();
        public ButtonMessages(string[] messages)
        {
            this.messages = messages;
        }
        public ButtonMessages(string[] dropDown, string[] messages)
        {
            this.dropDown = dropDown;
            this.messages = messages;

            for(int i1 = 0; i1 < dropDown.Length; i1++)
            {
                menuItems.Add(new Dropdown.OptionData(dropDown[i1]));
            }
        }

        public void setDropDown(Transform dropDownObject)
        {

        }
    }
    public GameObject mainPage;
    public Transform[] buttons;
    private ButtonMessages[] buttonMessages = new ButtonMessages[] { metal, carton, glass, paper, plastic, alcohol };
    private int currentMenu = 0;
    static ButtonMessages metal = new ButtonMessages
        (
            new string[]
            {
                "Please Select",
                "Metal cans",
                "Soft drink cans",
                "Jar lids",
                "Aluminum containers",
                "Aluminum foil",
                "Aerosol cans",
                "Frozen concentrate cans",
                "Potato chip tube",
                "Metal clothes hangers",
                "Scrap metal",
                "Chip bags"
            },
            new string[]
            { 
                "Please Select",
                "Recycable, Blue Bin!",
                "Recycable, Blue Bin!",
                "Recycable, Blue Bin!",
                "Recycable, Blue Bin!",
                "Recycable, Blue Bin! Clean before recycling!",
                "Recycable, Blue Bin! Clean before recycling!",
                "Recycable, Blue Bin! Clean before recycling!",
                "Recycable, Blue Bin!",
                "Recycable, Blue Bin!",
                "Not recycable, Regular Garbage Bin!",
                "Not recycable, Regular Garbage Bin!",
                "Not recycable, Regular Garbage Bin!"
            }
        );

    static ButtonMessages carton = new ButtonMessages
        (
            new string[]
            {
                "Please Select",
                "Milk and juice cartons",
                "Drink boxes",
                "Soup boxes"
            },
            new string[]
            {
                "Please Select",
                "Recycable, Blue Bin!",
                "Recycable, Blue Bin!",
                "Recycable, Blue Bin!"
            }
        );
    static ButtonMessages glass = new ButtonMessages
        (
            new string[]
            {
                "Please Select",
                "Empty bottles and jars",
                "Ceramics",
                "Drinking glasses",
                "Window glass",
                "Light bulbs",
                "Mirrors",
                "CFL bulbs"
            },
            new string[]
            {
                "Please Select",
                "Recycable, Blue Bin!",
                "Not recycable, Regular Garbage Bin!",
                "Not recycable, Regular Garbage Bin!",
                "Not recycable, Regular Garbage Bin!",
                "Not recycable, Regular Garbage Bin!",
                "Not recycable, Regular Garbage Bin!",
                "Not recycable, can be returned to a Take it Back! partner or a Household Hazardous Waste Depot"
            }
        );
    static ButtonMessages paper = new ButtonMessages
        (
            new string[]
            {
                "Please Select",
                "Newspaper and flyers",
                "Magazines and catalogues",
                "Corrugated cardboard",
                "Telephone books",
                "Cereal and cracker boxes",
                "Shoe and laundry detergent boxess",
                "Writing and computer paper",
                "Hard and soft cover books",
                "Paper egg cartons",
                "Toilet paper rolls",
                "Gift wrapping paper",
                "Clean paper shopping bags",
                "Frozen dinner boxes",
                "Clean pizza boxes",
                "Waxed paper",
                "Food soiled pizza boxes",
                "Tissues and paper towels",
                "Coffee cups"
            },
            new string[]
            {
                "Please Select",
                "Recycable, Black Bin!",
                "Recycable, Black Bin!",
                "Recycable, Black Bin!",
                "Recycable, Black Bin!",
                "Recycable, Black Bin!",
                "Recycable, Black Bin!",
                "Recycable, Black Bin!",
                "Recycable, Black Bin!",
                "Recycable, Black Bin!",
                "Recycable, Black Bin!",
                "Recycable, Black Bin!",
                "Recycable, Black Bin!",
                "Recycable, Black Bin!",
                "Recycable, Black Bin!",
                "Green Bin!",
                "Green Bin!",
                "Green Bin!",
                "Green Bin!"
            }
        );
    static ButtonMessages plastic = new ButtonMessages
        (
            new string[]
            {
                "Please Select",
                "Food containers",
                "Household containers",
                "Take-out containers",
                "Pails",
                "Planting trays",
                "Flower pots",
                "Single serve yogurt cups",
                "Clear plastic egg cartons",
                "Plastic bottles, jars and jugs",
                "Tubs and tub lids",
                "Styrofoam",
                "Coffee cup lids",
                "Plastic bag",
                "Hard plastics",
                "Motor oil containers"
            },
            new string[]
            {
                "Please Select",
                "Recycable, except expanded #6 items! Blue Bin!",
                "Recycable, except expanded #6 items! Blue Bin!",
                "Recycable, Blue Bin!",
                "Recycable, Blue Bin!",
                "Recycable, Blue Bin!",
                "Recycable, Blue Bin!",
                "Recycable, Blue Bin!",
                "Recycable, Blue Bin!",
                "Recycable, Blue Bin!",
                "Recycable, Blue Bin!",
                "Not recycable, Regular Garbage Bin!",
                "Not recycable, Regular Garbage Bin!",
                "Not recycable, Regular Garbage Bin!",
                "Not recycable, Regular Garbage Bin!",
                "Not recycable, Regular Garbage Bin!"
            }
        );
    static ButtonMessages alcohol = new ButtonMessages
        (
            new string[] 
            {
                "Empty wine, beer and spirit containers greater than 100 ml purchased in Ontario must be returned, for refund at The Beer Store."
            }
        );

    public void setPage(Vector2 screenSize)
    {
        Vector2 pos;
        Vector2 scale;

        //basic page

        scale = new Vector2(screenSize.x * (100f / 314f) / 100f, screenSize.x * (100f / 314f) / 100f);

        int i1 = 0;

        for(int y = -1; y < 2; y++)
        {
            for(int x = -1; x < 2; x+=2)
            {
                pos = new Vector2(screenSize.x / 2f + x * 100f * scale.x / 1.5f, screenSize.y / 2f + y * 30f * scale.y * 2);
                buttons[i1].GetComponent<RectTransform>().anchoredPosition = pos;
                buttons[i1].GetComponent<RectTransform>().localScale = scale;

                i1++;
            }
        }


        

        //sub page
        subPage.transform.GetChild(2).GetComponent<RectTransform>().localScale = new Vector3(2f / 558f * screenSize.y, 2f / 558f * screenSize.y, 1);

        pos = new Vector2();

        pos.x = screenSize.x * 0.1f;
        pos.y = screenSize.y * (1 - 0.15f);

        subPage.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = pos;

        subPage.transform.GetChild(0).GetComponent<RectTransform>().localPosition = screenSize / 2;

        scale = Vector2.one * (screenSize.x * 0.5f) / 160f;
        subPage.transform.GetChild(0).GetComponent<RectTransform>().localScale = scale;

        subPage.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector2 (screenSize.x * 0.5f, screenSize.y * 0.65f);

        scale = Vector2.one * screenSize.x * (200f / 314f) / 200f;

        subPage.transform.GetChild(1).GetComponent<RectTransform>().localScale = scale;

    }

    public void dropDownMenu()
    {
        Dropdown dropDown = subPage.transform.GetChild(0).GetComponent<Dropdown>();

        subPage.transform.GetChild(1).GetComponent<Text>().text = buttonMessages[currentMenu].messages[dropDown.value];
    }
    public void toggleSubPage(int i1)
    {
        toggleSubPage();

        Dropdown dropDown = subPage.transform.GetChild(0).GetComponent<Dropdown>();

        currentMenu = i1;

        if (!mainPage.activeSelf)
        {
            if (buttonMessages[i1].dropDown == null)
            {
                dropDown.transform.gameObject.SetActive(false);
            }
            else
            {
                dropDown.transform.gameObject.SetActive(true);

                dropDown.ClearOptions();
                dropDown.AddOptions(buttonMessages[i1].menuItems);
                dropDown.value = 0;
            }

            subPage.transform.GetChild(1).GetComponent<Text>().text = buttonMessages[i1].messages[0];
        }
        dropDown.Hide();
    }
    public new void toggleSubPage()
    {
        mainPage.SetActive(!mainPage.activeSelf);
        base.toggleSubPage();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alter : MonoBehaviour
{
    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;
    public GameObject slot4;

    string[] alterArray = new string[4];
    Image[] alterSpritesArray = new Image[4];

    public Inventory inventory;

    public Button alter_btn;

    public long alterfunds;
    private long alterExpected;

    public Text alterText;
    public Text ExpectedAmount;


    private void Start()
    {
        Initalter(alterArray);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            alterInItem();
        }
    }

    void Initalter(string[] alterArray)
    {
        alterArray[0] = null;
        alterArray[1] = null;
        alterArray[2] = null;
    }

    public void alterInItem()
    {
        if (alterArray[0] == null)
        {
            alterArray[0] = "";
            slot1.GetComponent<Image>().sprite = inventory.slots[inventory.choiceNum].itemData.itemImage;
        }
        else if (alterArray[1] == null)
        {
            alterArray[1] = "";
            slot2.GetComponent<Image>().sprite = inventory.slots[inventory.choiceNum].itemData.itemImage;
        }
        else if (alterArray[2] == null)
        {
            alterArray[2] = "";
            slot3.GetComponent<Image>().sprite = inventory.slots[inventory.choiceNum].itemData.itemImage;
        }
        else if (alterArray[3] == null)
        {
            alterArray[3] = "";
            slot4.GetComponent<Image>().sprite = inventory.slots[inventory.choiceNum].itemData.itemImage;
        }

        alterExpected += inventory.slots[inventory.choiceNum].itemData.ItemPrice;
        ExpectedAmount.text = "����ݾ� : " + alterExpected.ToString();
    }

    public void AlterButton()
    {
        for (int i = 0; i < alterArray.Length; i++)
        {
            if (alterArray[i] != null)
            {
                alterfunds += inventory.slots[i].itemData.ItemPrice;
                inventory.slots[i].DeleteItem();
                alterArray[i] = null;
            }
        }

        slot1.GetComponent<Image>().sprite = null;
        slot2.GetComponent<Image>().sprite = null;
        slot3.GetComponent<Image>().sprite = null;
        slot4.GetComponent<Image>().sprite = null;

        alterExpected = 0;
        ExpectedAmount.text = "����ݾ� : ";

        alterText.text = alterfunds.ToString() + " Gold";

        Debug.Log(alterfunds + " ��ҽ��ϴ�.");
    }
}

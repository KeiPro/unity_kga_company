using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.VolumeComponent;

public class Combine : MonoBehaviour
{
    public GameObject slot1;
    public GameObject slot2;

    string[] combineArray = new string[3];
    Image[] combineSpritesArray = new Image[3];

    public Inventory inventory;

    public Button combine_btn;

    private void Start()
    {
        InitCombine(combineArray);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            combineInItem();
        }
    }

    void InitCombine(string[] combineArray)
    {
        combineArray[0] = null;
        combineArray[1] = null;
        combineArray[2] = null;
    }

    public void combineInItem()
    {
        if (combineArray[0] == null)
        {
            combineArray[0] = "아이템1";
            slot1.GetComponent<Image>().sprite = inventory.slots[inventory.choiceNum].itemData.itemImage;
        }
        else if(combineArray[1] == null)
        {
            combineArray[1] = "아이템2";
            slot2.GetComponent<Image>().sprite = inventory.slots[inventory.choiceNum].itemData.itemImage;
        }
    }

    public void CombineButton()
    {
        if(slot1.GetComponent<Image>().sprite != null && slot2.GetComponent<Image>().sprite != null)
        {
            Debug.Log("아이템 합성 완성");
        }
    }
}

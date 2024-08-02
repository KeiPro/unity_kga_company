using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Button shop_btn;

    public Inventory inventory;

    public Alter alter;

    private int swordPrice;

    private void Start()
    {
        swordPrice = 500;
    }

    public void ShopButton()
    {
        if (alter.alterfunds < swordPrice)
        {
            Debug.Log("금액이 부족합니다.");
            return;
        }

        for (int i = 0; i < inventory.slots.Length; i++)
        {
            alter.alterfunds -= swordPrice;

            if (inventory.slots[i] != null)
            {
                inventory.slots[i].GetComponent<InventorySlot>().itemData = ResourceManager.Instance.ItemDataDictionary["Sword"];
                inventory.slots[i].GetComponent<InventorySlot>().itemImage.sprite = ResourceManager.Instance.ItemDataDictionary["Sword"].itemImage;
                Debug.Log("아이템을 구입했습니다.");
                return;
            }
            else
            {
                Debug.Log("인벤토리가 부족합니다.");
            }
        }
    }
}

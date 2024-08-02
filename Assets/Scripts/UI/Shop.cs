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
            Debug.Log("�ݾ��� �����մϴ�.");
            return;
        }

        for (int i = 0; i < inventory.slots.Length; i++)
        {
            alter.alterfunds -= swordPrice;

            if (inventory.slots[i] != null)
            {
                inventory.slots[i].GetComponent<InventorySlot>().itemData = ResourceManager.Instance.ItemDataDictionary["Sword"];
                inventory.slots[i].GetComponent<InventorySlot>().itemImage.sprite = ResourceManager.Instance.ItemDataDictionary["Sword"].itemImage;
                Debug.Log("�������� �����߽��ϴ�.");
                return;
            }
            else
            {
                Debug.Log("�κ��丮�� �����մϴ�.");
            }
        }
    }
}

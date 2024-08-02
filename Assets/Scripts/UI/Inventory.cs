using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Action onCurrentSlotChanged;

    [SerializeField]
    private GameObject go_SlotsParent;

    public InventorySlot[] slots;

    public GameObject choiceSlotImage; // ������ �κ��丮 �̹���
    public int choiceNum;              // ������ �κ��丮 ��ȣ

    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<InventorySlot>();

        choiceNum = 0;
    }

    void Update()
    {
        InventoryChoice();
    }

    //������ �ݱ�
    public void AcquireItem(ItemData _itemData)
    {
        slots[choiceNum].AddItem(_itemData);
        return;
    }

    //������ ������
    public void LoseItem()
    {
        slots[choiceNum].DeleteItem();
        slots[choiceNum].item = null;
        SetActiveMethod();
    }
    void InventoryChoice()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            choiceSlotImage.transform.localPosition = new Vector3(-270.0f, 0f, 0f);
            choiceNum = 0;
            onCurrentSlotChanged();
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            choiceSlotImage.transform.localPosition = new Vector3(-90.0f, 0f, 0f);
            choiceNum = 1;
            onCurrentSlotChanged();
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            choiceSlotImage.transform.localPosition = new Vector3(90.0f, 0f, 0f);
            choiceNum = 2;
            onCurrentSlotChanged();
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            choiceSlotImage.transform.localPosition = new Vector3(270.0f, 0f, 0f);
            choiceNum = 3;
            onCurrentSlotChanged();
        }
    }



    public void SetItem(IItem it)
    {
        slots[choiceNum].item = it;

        SetActiveMethod();

    }
    public IItem LoadItem()
    {
        SetActiveMethod();
        return slots[choiceNum].item;
    }

    void SetActiveMethod()
    {
        foreach (InventorySlot it2 in slots)
        {
            if (it2.item == slots[choiceNum].item)
            {
                if (it2.item is Weapon)
                {
                    Weapon weapon = (Weapon)it2.item;
                    if (weapon != null)
                        weapon.transform.gameObject.SetActive(true);
                }
                else if (it2.item is Item)
                {
                    Item item = (Item)it2.item;
                    if (item != null)
                        item.transform.gameObject.SetActive(true);
                }
                continue;
            }
            if (it2.item is Weapon)
            {
                Weapon weapon = (Weapon)it2.item;
                if (weapon != null)
                    weapon.transform.gameObject.SetActive(false);
            }
            else if (it2.item is Item)
            {
                Item item = (Item)it2.item;
                if (item != null)
                    item.transform.gameObject.SetActive(false);
            }
        }
    }

    public int GetItemCurrentIndex()
    {
        return choiceNum;
    }

    public void SetItemCurrentIndex(int index)
    {
        choiceNum = index;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public ItemData itemData; //ȹ���� ������
    public Image itemImage;   //������ �̹���

    public IItem item;

    //������ ȹ��
    public void AddItem(ItemData _itemData)
    {
        itemData = _itemData;
        itemImage.sprite = itemData.itemImage;
    }

    //������ ���� �ʱ�ȭ
    public void DeleteItem()
    {
        itemData = null;
        itemImage.sprite = null;
    }
}

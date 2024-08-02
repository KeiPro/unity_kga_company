using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterInventory : MonoBehaviour
{
    // public Action<int> onInventoryFilled;
    // public Action<int> onInventoryEmpty;

    // IItem[] m_itemArray = new IItem[4];
    // int m_currentIndex;

    // void Start()
    // {
    //     m_currentIndex = 0;
    // }

    // public void SetItem(IItem it)
    // {
    //     m_itemArray[m_currentIndex] = it;

    //     SetActiveMethod();
    //     CheckArray();
    // }
    // public IItem LoadItem()
    // {
    //     SetActiveMethod();
    //     return m_itemArray[m_currentIndex];
    // }

    // void SetActiveMethod()
    // {
    //     foreach (IItem it2 in m_itemArray)
    //     {
    //         if (it2 == m_itemArray[m_currentIndex])
    //         {
    //             if (it2 is Weapon)
    //             {
    //                 Weapon weapon = (Weapon)it2;
    //                 if (weapon != null)
    //                     weapon.transform.gameObject.SetActive(true);
    //             }
    //             else if (it2 is Item)
    //             {
    //                 Item item = (Item)it2;
    //                 if (item != null)
    //                     item.transform.gameObject.SetActive(true);
    //             }
    //             continue;
    //         }
    //         if (it2 is Weapon)
    //         {
    //             Weapon weapon = (Weapon)it2;
    //             if (weapon != null)
    //                 weapon.transform.gameObject.SetActive(false);
    //         }
    //         else if (it2 is Item)
    //         {
    //             Item item = (Item)it2;
    //             if (item != null)
    //                 item.transform.gameObject.SetActive(false);
    //         }
    //     }
    // }

    // void CheckArray()
    // {
    //     int i = 0;
    //     int j = 0;
    //     foreach (IItem it in m_itemArray)
    //     {
    //         if (it != null)
    //         {
    //             onInventoryFilled(i);
    //         }
    //         else
    //         {
    //             onInventoryEmpty(j);
    //         }
    //         i += 1;
    //         j += 1;
    //     }
    // }

    // public int GetItemCurrentIndex()
    // {
    //     return m_currentIndex;
    // }

    // public void SetItemCurrentIndex(int index)
    // {
    //     m_currentIndex = index;
    // }

}

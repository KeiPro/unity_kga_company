using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : Singleton<ResourceManager>
{
    public Dictionary<string, ItemData> ItemDataDictionary { get; private set; }

    void Start()
    {
        ItemDataDictionary = new Dictionary<string, ItemData>();

        var loadedItems = Resources.LoadAll("Item");

        foreach (var item in loadedItems)
        {
            var itemData = item as ItemData;
            if (itemData != null)
            {
                ItemDataDictionary.Add(itemData.itemName, itemData);
                Debug.Log(itemData);
            }
            else
            {
                Debug.LogWarning("Loaded item is not of type ItemData");
            }
        }
    }

}

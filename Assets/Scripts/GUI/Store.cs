using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Store : MonoBehaviour
{
    [Serializable]
    public class ItemsList
    {
        //the "content" component of a single store scrollbar
        public GameObject storeContent;
        public GameObject[] itemsPrefabs;
    }

    public GameObject storeButtonPrefab;
    public ItemsList[] Stores;

    void Awake()
    {
        foreach (ItemsList itemsList in Stores)
            {
            int itemIndex = 0;
            foreach (GameObject itemPrefab in itemsList.itemsPrefabs)
            {
                Transform storeButton = itemsList.storeContent.transform.GetChild(itemIndex);
                if (storeButton != null)
                {
                    storeButton.GetComponent<BuyItemButton>().itemPrefab = itemPrefab;
                }
                itemIndex ++;
            }
            
        }
    }
}
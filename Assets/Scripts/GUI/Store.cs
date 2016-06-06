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

    void Start()
    {
        foreach (ItemsList itemsList in Stores)
            {
            RectTransform storeContentRectTransform = itemsList.storeContent.GetComponent<RectTransform>();
            storeContentRectTransform.offsetMax = new Vector2(itemsList.itemsPrefabs.Length * 70, 0);
            foreach (GameObject itemPrefab in itemsList.itemsPrefabs)
            {
                GameObject newStoreButton = Instantiate(storeButtonPrefab);
                newStoreButton.GetComponent<BuyItemButton>().itemPrefab = itemPrefab;
                newStoreButton.transform.parent = itemsList.storeContent.transform;
            }
            itemsList.storeContent.GetComponent<HorizontalLayoutGroup>().enabled = true;
        }
    }
}
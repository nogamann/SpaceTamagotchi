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
		public string[] items;
    }

    public GameObject storeButtonPrefab;
    public ItemsList[] Stores;

    void Awake()
    {
        foreach (ItemsList itemsList in Stores)
            {
            int itemIndex = 0;
			foreach (string item in itemsList.items)
            {
                Transform storeButton = itemsList.storeContent.transform.GetChild(itemIndex);
                if (storeButton != null)
                {
                    //TODO images for store buttons
                }
                itemIndex ++;
            }
            
        }
    }
}
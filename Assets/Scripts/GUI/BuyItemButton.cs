using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuyItemButton : MonoBehaviour {

    public GameObject itemPrefab;
    void Start()
    {
        this.GetComponent<Image>().sprite = itemPrefab.GetComponent<SpriteRenderer>().sprite;
        this.GetComponentInChildren<Text>().text = itemPrefab.GetComponent<ThingObject>().price + "$";
    }

	public void TryToBuy()
    {
        if (GameManager.buyItem(itemPrefab.GetComponent<ThingObject>()))
        {
            Instantiate(itemPrefab);
        }
    }

}

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
		Debug.Log ("try to buy");

		GameObject[] objects = GameObject.FindGameObjectsWithTag ("Finger");
		PlayerController local = null;
		foreach (GameObject go in objects){
			PlayerController pc = go.GetComponent<PlayerController>();
			if (pc.isLocalPlayer) {
				local = pc;
			}
		}
		Debug.Log ("itemPrefab: " + itemPrefab.ToString ());
		Debug.Log ("local: " + local.ToString ());
		local.BuyItem (itemPrefab);
    }

}
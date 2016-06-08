using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuyItemButton : MonoBehaviour {

    public GameObject itemPrefab;
    //public GameObject moneyError;

    void Start()
    {
        this.GetComponent<Image>().sprite = itemPrefab.GetComponent<SpriteRenderer>().sprite;
        this.GetComponentInChildren<Text>().text = itemPrefab.GetComponent<ThingObject>().price + "$";
    }

	public void TryToBuy()
    {
		
//		Instantiate(itemPrefab, new Vector3(5f,-4f,0), Quaternion.identity);
		GameObject[] objects = GameObject.FindGameObjectsWithTag("Finger");
		PlayerController local = null;
		foreach (GameObject go in objects) {
			PlayerController pc = go.GetComponent<PlayerController> ();
			if (pc.isLocalPlayer) {
				local = pc;
			}
		}
		int price = itemPrefab.GetComponent<ThingObject> ().price;
		local.BuyItem (price);


//        if (GameManager.buyItem(itemPrefab.GetComponent<ThingObject>()))
//        {
//            Instantiate(itemPrefab, new Vector3(5f,-4f,0), Quaternion.identity);
//        }
//        else
//        {
//            Debug.Log("not enough money!");
//            transform.Find("DontHaveEnoughMoneyError").gameObject.SetActive(true);
//        }
    }
}

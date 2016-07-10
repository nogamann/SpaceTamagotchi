using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuyItemButton : MonoBehaviour {

    public GameObject itemPrefab;
    //public GameObject moneyError;

    void Start()
    {
        this.GetComponent<Image>().sprite = itemPrefab.GetComponent<SpriteRenderer>().sprite;
		Debug.Log ("sprite on button");
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
<<<<<<< HEAD
		local.BuyItem (price);
		this.GetComponent<Button> ().enabled = false;
=======
		local.BuyItem (price, itemPrefab.GetComponent<ThingObject>().specificTypeItem);
>>>>>>> b4b9f7d494c638cab68de4c364eb5bc5f492b78f


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

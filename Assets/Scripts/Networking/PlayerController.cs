using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	public GameManager gameManager;

	void Awake () {
		gameManager = FindObjectOfType<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isLocalPlayer) {
			return;
		}

		// player movement
		// player movement
		var x = Input.GetAxis("Horizontal")*0.1f;
		var z = Input.GetAxis("Vertical")*0.1f;

		transform.Translate (x, 0, z);
	}


	public void GrabItem(GameObject obj){
		CmdGrabItem (obj);
	}


	[Command]
	void CmdGrabItem(GameObject obj){
		obj.GetComponent<NetworkIdentity> ().AssignClientAuthority (connectionToClient);
	}

	public void FreeItem(GameObject obj){
		CmdFreeItem (obj);	
	}

	[Command]
	void CmdFreeItem(GameObject obj){
		obj.GetComponent<NetworkIdentity> ().RemoveClientAuthority (connectionToClient);
	}
		
	// returns true if the purchase was successful, false otherwise
	public bool BuyItem(GameObject item)
	{
		Debug.Log ("buy item");
		float beforeMoney = gameManager.floatMoney;
		int price = item.GetComponent<ThingObject> ().price;
		CmdBuy (item);
		// TODO improve
		return (gameManager.floatMoney - beforeMoney == price);  
	}

	[Command]
	void CmdBuy(GameObject item){
//		gameManager.GetComponent<NetworkIdentity> ().AssignClientAuthority (connectionToClient);

		int price = item.GetComponent<ThingObject> ().price;
		Debug.Log ("in cmd");

		if (gameManager.floatMoney - price >= 0) { 
			// subtract money
			gameManager.floatMoney -= price; 	
			Debug.Log ("bought");

			// add to inventory
			gameManager.addObject(item);
		}
//		gameManager.GetComponent<NetworkIdentity> ().RemoveClientAuthority (connectionToClient);

	}
}

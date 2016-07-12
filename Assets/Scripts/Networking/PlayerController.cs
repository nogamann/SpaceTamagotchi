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
	public bool BuyItem(int price, ThingObject.SpecificItemType type)
	{
		float beforeMoney = gameManager.floatMoney;
		CmdBuy (price, type);
		// TODO improve
		return (gameManager.floatMoney - beforeMoney == price);  
	}

	[Command]
	void CmdBuy(int price, ThingObject.SpecificItemType type)
    {

		if (gameManager.floatMoney - price >= 0) { 
			// subtract money
			gameManager.decreaseMoney(price); 	

			// add to inventory
			gameManager.addObject(type);
		}
	}

	public void doAction(GameObject item, Creature.CreatureParams action){
		CmdDoAction(item,action);
		float love = item.GetComponent<ThingObject> ().Love;
		if (isServer) {
			CmdUpdatePlayerOneLove (love);
		} else {
			CmdUpdatePlayerTwoLove (love);
		}
	}

	[Command]
	void CmdDoAction(GameObject item, Creature.CreatureParams action){
		gameManager.doAction (item, action);
	}

	[Command]
	void CmdUpdatePlayerOneLove(float amount){
		gameManager.UpdatePlayerOneLove (amount);
	}

	[Command]
	void CmdUpdatePlayerTwoLove(float amount){
		gameManager.UpdatePlayerTwoLove (amount);
	}

	public void updatePlayerOneLove(float amount){
		CmdUpdatePlayerOneLove (amount);
	}

	public void updatePlayerTwoLove(float amount){
		CmdUpdatePlayerTwoLove (amount);
	}

//
//	public void GreyOutItem(GameObject item){
//		item.GetComponent<SpriteRenderer> ().color = new Color (.5f, .5f, .5f, .5f);
//	}
//
//	public void RecolorItem(GameObject item){
//		item.GetComponent<SpriteRenderer> ().color = new Color (1,1,1,1);
//	}

//	public void playActionAnimation(Creature.CreatureParams action){
//		CmdPlayActionAnimation (action);
//	}
//
//	[Command]
//	void CmdPlayActionAnimation(Creature.CreatureParams action){
//		gameManager.playActionAnimation (action);
//	}
//
//	public void playMoodAnimation(Creature.CreatureParams mood){
//		CmdPlayActionAnimation (mood);
//	}
//
//	[Command]
//	void CmdPlayMoodAnimation(Creature.CreatureParams mood){
//		gameManager.playMoodAnimation (mood);
//	}

}

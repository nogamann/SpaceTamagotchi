using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;


public class GameManager : NetworkBehaviour 
{
    public int money;
	public Canvas gameOverCanvas;
	public Canvas gameCanvas;

	[SyncVar]
    public float floatMoney;
    public float moneyEarnedPerSec;
    public Text moneyText;

	[SyncVar]
	public float playerOneLove;
	[SyncVar]
	public float playerTwoLove;

    [Serializable]
    public class ItemsPrefabs{
        public ThingObject.SpecificItemType specificItemType;
        public GameObject itemPrefab;
    }

    public ItemsPrefabs[] itemsPrefabs;

    public Dictionary<ThingObject.SpecificItemType, GameObject> itemsPrefabsDictionary = new Dictionary<ThingObject.SpecificItemType, GameObject>();

    void Start()
    {
//		animator = this.GetComponent<Animator>();

        itemsPrefabsDictionary = new Dictionary<ThingObject.SpecificItemType, GameObject>();
       
        foreach (ItemsPrefabs item in itemsPrefabs)
        {
            itemsPrefabsDictionary[item.specificItemType] = item.itemPrefab;
        }

		playerOneLove = 0.8f;
		playerTwoLove = 0.8f;
    }


    public void decreaseMoney(float amount){
		if (!isServer) {
			return;
		}
		floatMoney -= amount;
	}

    void FixedUpdate()
    {
		if (isServer) {
			floatMoney += Time.deltaTime * moneyEarnedPerSec;
		}
        money = Mathf.FloorToInt(floatMoney);
		moneyText.text = money + "$";
    }

	public void addObject(ThingObject.SpecificItemType type)
    {
		Vector3 position = new Vector3 (5, UnityEngine.Random.Range(-3,3), 0);
		var go = (GameObject)Instantiate (itemsPrefabsDictionary[type], position, Quaternion.identity);
		NetworkServer.Spawn (go);
	}

	public void doAction(GameObject item, Creature.CreatureParams action){
		RpcDoAction (item, action);
	}


	[ClientRpc]
	void RpcDoAction(GameObject item, Creature.CreatureParams action){
		GameObject obj = GameObject.FindGameObjectWithTag ("Creature");
		Creature creature = obj.GetComponent<Creature> ();
		creature.DoAction (item, action);
	}

	public void UpdatePlayerOneLove(float amount){
		if (!isServer) {
			return;
		}
		playerOneLove += amount;
		if (playerOneLove > 1) {
			playerOneLove = 1;
		}
		if (playerOneLove < 0) {
			playerOneLove = 0;
		}
	}

	public void UpdatePlayerTwoLove(float amount){
		if (!isServer) {
			return;
		}
		playerTwoLove += amount;
		if (playerTwoLove > 1) {
			playerTwoLove = 1;
		}
		if (playerTwoLove < 0) {
			playerTwoLove = 0;
		}
	}

	public void GameOver(){
		gameCanvas.enabled = false;
		gameOverCanvas.enabled = true;
	}

//	public void playActionAnimation(Creature.CreatureParams action){
//		animator.SetInteger("action", (int)action);
//		animator.SetTrigger("canChange");
//		animator.SetInteger("mood", 0);
//	}
//
//	public void playMoodAnimation(Creature.CreatureParams mood){
//		if (animator.GetInteger("mood") != (int)mood)
//		{
//			animator.SetInteger("mood", (int)mood);
//			animator.SetTrigger("canChange");
//		}
//	}

}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;


public class GameManager : NetworkBehaviour 
{
    public int money;
	public GameObject creature;

	[SyncVar]
    public float floatMoney;
    public float moneyEarnedPerSec;
    public Text moneyText;

    [Serializable]
    public class ItemsPrefabs{
        public ThingObject.SpecificItemType specificItemType;
        public GameObject itemPrefab;
    }

    public ItemsPrefabs[] itemsPrefabs;

    public Dictionary<ThingObject.SpecificItemType, GameObject> itemsPrefabsDictionary = new Dictionary<ThingObject.SpecificItemType, GameObject>();

    void Start()
    {

        itemsPrefabsDictionary = new Dictionary<ThingObject.SpecificItemType, GameObject>();
       
        foreach (ItemsPrefabs item in itemsPrefabs)
        {
            itemsPrefabsDictionary[item.specificItemType] = item.itemPrefab;
        }
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
		Debug.Log (position.ToString ());
		var go = (GameObject)Instantiate (itemsPrefabsDictionary[type], position, Quaternion.identity);
		NetworkServer.Spawn (go);
	}

}

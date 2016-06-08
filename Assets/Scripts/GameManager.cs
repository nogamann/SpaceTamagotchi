using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour 
{
    public int money;
	[SyncVar]
    public float floatMoney;
    public float moneyEarnedPerSec;
    public Text moneyText;
	//TODO inventory list
	  
    void FixedUpdate()
    {
        floatMoney += Time.deltaTime * moneyEarnedPerSec;
        money = Mathf.FloorToInt(floatMoney);
        moneyText.text = money + "$";
    }

	public void addObject(GameObject prefab){
		var go = (GameObject)Instantiate (prefab, transform.position + new Vector3 (2, 2, 0), Quaternion.identity);
		NetworkServer.Spawn (go);
	}

}


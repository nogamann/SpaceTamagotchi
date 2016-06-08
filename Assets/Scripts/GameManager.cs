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

	public GameObject prefab;
	  
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

	public void addObject(){
		var go = (GameObject) Instantiate (prefab, transform.position + new Vector3 (2, 2, 0), Quaternion.identity);
		NetworkServer.Spawn (go);
	}
}


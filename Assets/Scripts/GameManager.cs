using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int money;
    private float floatMoney;
    public float moneyEarnedPerSec;
    public Text moneyText;


    void Start()
    {

    }

    void FixedUpdate()
    {
        floatMoney += Time.deltaTime * moneyEarnedPerSec;
        money = Mathf.FloorToInt(floatMoney);
        moneyText.text = money + "$";
    }

    //returns true if the purchase was successful, false otherwise
    public static bool buyItem(ThingObject thingObject)
    {
        if (money >= thingObject.price)
        {
            //TODO add lock mechanism
            money -= thingObject.price;
            return true;
        }
        return false;
    }

}


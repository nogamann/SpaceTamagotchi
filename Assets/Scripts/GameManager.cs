using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int money;
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

}


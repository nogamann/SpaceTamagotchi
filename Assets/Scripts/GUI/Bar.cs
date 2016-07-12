using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Bar : MonoBehaviour
{
    public enum BarType
    {
        HealthBar,
        LovePlayerOneBar,
		LovePlayerTwoBar,
        HungerBar,
        JoyBar
    }

    public BarType barType;
    public Scrollbar scrollBar;
    public Creature creature;
	public GameManager gameManager;
	public bool gameStarted = false;

    // Update is called once per frame
    void Update()
    {
		if (gameStarted) {
			switch (barType) {
			case BarType.HealthBar:
				scrollBar.size = creature.metersDictionary [Creature.CreatureParams.health];
				break;

			case BarType.HungerBar:
				scrollBar.size = creature.metersDictionary [Creature.CreatureParams.hunger];
				break;

			case BarType.JoyBar:
				scrollBar.size = creature.metersDictionary [Creature.CreatureParams.joy];
				break;

			case BarType.LovePlayerOneBar:
				scrollBar.size = gameManager.playerOneLove;
				break;
			
			case BarType.LovePlayerTwoBar:
				scrollBar.size = gameManager.playerTwoLove;
				break;

			//TODO add cases for playerOneLove and playerTwoLove
			default:
				break;
			}
		}
        
    }
}

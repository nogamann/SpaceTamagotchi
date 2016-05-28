using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Bar : MonoBehaviour
{
    public enum BarType
    {
        HealthBar,
        LoveBar,
        HungerBar,
        JoyBar
    }

    public BarType barType;
    public Scrollbar scrollBar;
    public Creature creature;

    // Update is called once per frame
    void Update()
    {
        switch (barType)
        {
            case BarType.HealthBar:
                scrollBar.size = creature.health;
                break;

            case BarType.HungerBar:
                scrollBar.size = creature.hunger;
                break;

            case BarType.JoyBar:
                scrollBar.size = creature.joy;
                break;

            case BarType.LoveBar:
<<<<<<< HEAD
				scrollBar.size = creature.generalLove;
=======
                scrollBar.size = creature.generalLove;
>>>>>>> 193831d838e2f7a5ccca1859cb7950aec0e7c031
                break;

                //TODO add cases for playerOneLove and playerTwoLove
            default:
                break;
        }
        
    }
}

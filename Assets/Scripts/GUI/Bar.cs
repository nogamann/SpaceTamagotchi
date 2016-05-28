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
				scrollBar.size = creature.generalLove;
                break;

            default:
                break;
        }
        
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ThingObject : MonoBehaviour
{

    public enum ItemType
    {
        Food,
        Game,
        Medicine
    }

    public enum SpecificItemType
    {
        Sandwich,
        SpaceBurget,
        Chocolate,
        Carrot,
        Pill1,
        Pill2,
        Syrup,
        Injection,
        Ball,
        Yoyo,
        tranpoline,
		cardboard
	
    }


    // Fileds
    public float health;
	public float love;
	public float hunger;
	public float joy;
    public float myLove;
    public float otherLove;
    public int price;

	// the level of atractiveness of the item to the creature
	public float attractiveness;

    public ItemType itemType;
    public SpecificItemType specificTypeItem;

    public Dictionary<Creature.CreatureParams, float> metersEffect;
    

    void Awake(){
		metersEffect = new Dictionary<Creature.CreatureParams, float> () {
			{ Creature.CreatureParams.joy, joy },
			{ Creature.CreatureParams.health, health },
			{ Creature.CreatureParams.hunger, hunger },
			{ Creature.CreatureParams.myLove, myLove },
			{ Creature.CreatureParams.otherLove, OtherLove },
            { Creature.CreatureParams.generalLove, love }
		};
	}


	public float Joy {
		get {
			return joy;
		}
		set {
			joy = value;
		}
	}

	public float Hunger {
		get {
			return hunger;
		}
		set {
			hunger = value;
		}
	}

	public float Love {
		get {
			return love;
		}
		set {
			love = value;
		}
	}

	public float Health {
		get {
			return health;
		}
		set {
			health = value;
		}
	}

    public float OtherLove
    {
        get
        {
			return otherLove;
        }
        set
        {
			otherLove = value;
        }
    }

    public float MyLove
    {
        get
        {
			return myLove;
        }
        set
        {
			myLove = value;
        }
    }

}

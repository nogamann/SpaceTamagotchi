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

	// Fileds
	public float health;
	public float love;
	public float hunger;
	public float joy;
    public float playerOneLove;
    public float playerTwoLove;
    public int price;

	// the level of atractiveness of the item to the creature
	public float attractiveness;

    public ItemType itemType;

	public Dictionary<Creature.CreatureParams, float> metersEffect;
    

    void Awake(){
		metersEffect = new Dictionary<Creature.CreatureParams, float> () {
			{ Creature.CreatureParams.joy, joy },
			{ Creature.CreatureParams.health, health },
			{ Creature.CreatureParams.hunger, hunger },
            { Creature.CreatureParams.playerOneLove, playerOneLove },
            { Creature.CreatureParams.playerTwoLove, playerTwoLove },
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

    public float PlayerTwoLove
    {
        get
        {
            return playerTwoLove;
        }
        set
        {
            playerTwoLove = value;
        }
    }

    public float PlayerOneLove
    {
        get
        {
            return playerOneLove;
        }
        set
        {
            playerOneLove = value;
        }
    }

}

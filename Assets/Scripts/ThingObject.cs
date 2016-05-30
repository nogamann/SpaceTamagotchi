using UnityEngine;
using System.Collections;

public class ThingObject : UnityEngine.Object{

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

	// the level of atractiveness of the item to the creature
	public float attractiveness;

    public ItemType itemType;


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
}

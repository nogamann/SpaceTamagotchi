using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Creature : MonoBehaviour {

	// fields (meters):
	public float lovePlayrOne = 0.5f;
	public float lovePlayrTwo = 0.5f;
	// TODO: should be updated everytime love changes
	public float loveGeneral = (lovePlayrOne + lovePlayrTwo) / 2;
	public float hunger = 0.5f;
	public float health = 0.5f;
	public float joy = 0.5f;
	public MoodObject currentMood;

	public MoodObject CurrentMood {
		get {
			return currentMood;
		}
		set {
			CurrentMood = value;
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

	public float LovePlayrOne {
		get {
			return lovePlayrOne;
		}
		set {
			LovePlayrOne = value;
		}
	}

	public float LovePlayrTwo {
		get {
			return lovePlayrTwo;
		}
		set {
			LovePlayrTwo = value;
		}
	}

	public float LoveGeneral {
		get {
			return loveGeneral;
		}
		set {
			LoveGeneral = value;
		}
	}

	/// <summary>
	/// eat, play or take medicine
	/// </summary>
	/// <param name="thing">food, toy or medicine.</param>
	void DoAction(ThingObject thing){
		//TODO: play relevant animation

		health += (thing.Health * health^3);
		hunger += (thing.Hunger * hunger^3);
		// TODO: change the love of the current player only
		lovePlayrOne += (thing.Love * lovePlayrOne^3);

	}
		
	/// <summary>
	/// Calculate what is the creature's mood based on his meters, and update currentMood.
	/// </summary>
	void CalculateAndUpdateMood(){
		Dictionary<MoodObject, float> moodsDict;

		moodsDict.Add(bored, (1 - joy) * 0.8f + (1 - loveGeneral) * 0.2f);
		moodsDict.Add(sick, (1 - health) * 0.7f + (1 - hunger) * 0.2f + (1 - loveGeneral) * 0.1f);
		moodsDict.Add(hungry, (1 - hunger) * 0.5f + (1 - joy) * 0.3f + (1 - loveGeneral) * 0.1f + (1 - health) * 0.1f);
		moodsDict.Add(angry, (1 - loveGeneral) * 0.4f + (1 - joy) * 0.3f + (1 - hunger) * 0.3f);
		moodsDict.Add(happy, (hunger + joy + loveGeneral + health) / 4.0f);
		moodsDict.Add(sad, 1 - moodsDict[happy]);

		KeyValuePair<MoodObject, float> highestMood = moodsDict [0];
		foreach (KeyValuePair<MoodObject, float> mood in moodsDict) {
			if (mood.Value > highestMood.Value) {
				highestMood = mood;
			}
		}

		currentMood = highestMood;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//check if the creature is dead

		// update mood
		CalculateAndUpdateMood ();
	}
}

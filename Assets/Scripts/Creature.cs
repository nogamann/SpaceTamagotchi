using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Creature : MonoBehaviour {

<<<<<<< HEAD
	// fields (meters):
	public float lovePlayrOne = 0.5f;
	public float lovePlayrTwo = 0.5f;
    // TODO: should be updated everytime love changes
    public float loveGeneral = 0.5f; //(lovePlayrOne + lovePlayrTwo) / 2;
=======
	// parameters enum
	enum Parameters{
		// TODO
	}

	// meters
>>>>>>> ee2a590df8b90a3adf132089a6ea9fcc349d8e45
	public float hunger = 0.5f;
	public float health = 0.5f;
	public float joy = 0.5f;
	public float playerOneLove = 0.5f;
	public float playrTwoLove = 0.5f;
	public float generalLove = 0.5f;
	// TODO: should be updated everytime love changes
//	public float generalLove = (playrOneLove + playrTwoLove) / 2;

	// meters update time
	public float hungerUpdate;
	public float healthUpdate;
	public float joyUpdate;
	public float playerOneLoveUpdate;
	public float playrTwoLoveUpdate;
	public float generalLoveUpdate;

	// A dictionary to hold the available moods of the creature
	Dictionary<string, Dictionary<string, float>> moods = new Dictionary<string, Dictionary<string, float>> ();

	// Dictionaries for the moods of the creature. Every possible mood is represented by a dictionary.
	Dictionary<string, float> happy = new Dictionary<string, float> ();
	Dictionary<string, float> bored = new Dictionary<string, float> ();
	// TODO add more moods

	/// <summary>
	/// Initializes the moods dictionaries.
	/// </summary>
	private void InitMoodDictionaries() {
		// init happy
		happy.Add("interval", 0.01f);
		happy.Add("decrease", 0.01f);
		happy.Add("joyCoefficient", 0.0f);
		happy.Add("healthCoefficient", 0.0f);
		happy.Add("hungerCoefficient", 0.0f);
		happy.Add("playerOneLoveCoefficient", 0.0f);
		happy.Add("score", 0.0f);

		// init bored
		bored.Add("interval", 0.01f);
		bored.Add("decrease", 0.01f);
		bored.Add("joyCoefficient", 0.8f);
		bored.Add("healthCoefficient", 0.0f);
		bored.Add("hungerCoefficient", 0.0f);
		bored.Add("playerOneLoveCoefficient", 0.0f);
		bored.Add("playerTwoLoveCoefficient", 0.0f);
		bored.Add("generalLoveCoefficient", 0.2f);
		bored.Add("score", 0.0f);

		// init the moods dictionary
		moods.Add ("happy", happy);
		moods.Add ("bored", bored);
	}

	public string currentMood;

	/// <summary>
	/// eat, play or take medicine
	/// </summary>
	/// <param name="thing">food, toy or medicine.</param>
	void DoAction(ThingObject thing){
		//TODO: play relevant animation

		health += (thing.Health * Mathf.Pow(health, 3));
		hunger += (thing.Hunger * Mathf.Pow(hunger, 3));
		joy += (thing.Joy * Mathf.Pow(joy, 3));

		// TODO: change the love of the current player only
		playerOneLove += (thing.Love * Mathf.Pow(playerOneLove, 3));

		// TODO add impact of the love to the performing player (should be implemented as a formula in the enum)
	}

	/// <summary>
	/// Calculates the mood score.
	/// </summary>
	/// <returns>The mood score, according to the rellevant meters.</returns>
	/// <param name="mood">string. The mood we want to calculate it's score.</param>
	private float CalculateMoodScore (string mood) {
		
		float joyCoefficient = moods [mood] ["joyCoefficient"];
		float healthCoefficient = moods [mood] ["healthCoefficient"];
		float hungerCoefficient = moods [mood] ["hungerCoefficient"];
		float playerOnelLoveCoefficient = moods [mood] ["playerOnelLoveCoefficient"];
		float playerTwoLoveCoefficient = moods [mood] ["playerTwoLoveCoefficient"];
		float generalLoveCoefficient = moods [mood] ["generalLoveCoefficient"];


		float moodScore = (1 - health) * healthCoefficient + (1 - hunger) * hungerCoefficient + (1 - generalLove) * generalLoveCoefficient +
			(1 - joy) * joyCoefficient + (1 - playerOneLove) * playerOnelLoveCoefficient + (1 - playrTwoLove) * playerTwoLoveCoefficient;

		return moodScore;
	}
		
	/// <summary>
	/// Calculate what is the creature's mood based on his meters, and update currentMood.
	/// </summary>
	void CalculateAndUpdateMood(){
		// TODO maybe need a different init?
		string nextMood = "bored";

		foreach (var mood in moods) {
			mood.Value ["score"] = CalculateMoodScore (mood.Key);
			if (mood.Value["score"] > moods[nextMood]["score"]) {
				nextMood = mood.Key;
			}
		}

		currentMood = nextMood;
	}

	void UpdateMeters() {
		float time = Time.time;
		float joyInterval = moods [currentMood] ["joyInterval"];

		if ((time - joyUpdate) >= joyInterval) {
			joy -= moods [currentMood] ["joyDecrease"];
		}
		// TODO update the rest of the meters (update meters should take place as the example in FixedUpdate function)
	}

	// Use this for initialization
	void Start () {
		float time = Time.time;
		InitMoodDictionaries ();

		// initialize the update times of the meters at initialization time
		hungerUpdate = time;
		healthUpdate = time;
		joyUpdate = time;
		playerOneLoveUpdate = time;
		playrTwoLoveUpdate = time;
		generalLoveUpdate = time;

		// the first mood of the creature at birth is Happy
		currentMood = "happy";
	}
	
	// Update is called once per frame
	void Update () {
		//check if the creature is dead

		// update mood
		CalculateAndUpdateMood ();
	}

	void FixedUpdate() {
		// TODO update meter example, don't forget to remove!
		float meter = 0;
		float change = -0.002f; // in seconds * -1

		meter += change * Time.fixedDeltaTime;
	}


	// getters and setters

	public string CurrentMood {
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
			return playerOneLove;
		}
		set {
			LovePlayrOne = value;
		}
	}

	public float LovePlayrTwo {
		get {
			return playrTwoLove;
		}
		set {
			LovePlayrTwo = value;
		}
	}

	public float LoveGeneral {
		get {
			return generalLove;
		}
		set {
			LoveGeneral = value;
		}
	}
		
	public class moodClass {
		// TODO needed?
	}

	/// <summary>
	/// Formula component.
	/// </summary>
<<<<<<< HEAD
	/// <param name="thing">food, toy or medicine.</param>
	void DoAction(ThingObject thing){
        //TODO: play relevant animation

        health += (thing.Health); // * health^3);
        hunger += (thing.Hunger); // * hunger^3);
                                  // TODO: change the love of the current player only
        lovePlayrOne += (thing.Love); // * lovePlayrOne^3);
=======
	[Serializable]
	public class FormulaComponent {
		/// <summary>
		/// The name of the formula component as it appears in the enum.
		/// </summary>
		public Parameters parameter;
>>>>>>> ee2a590df8b90a3adf132089a6ea9fcc349d8e45

		/// <summary>
		/// The value of the component.
		/// </summary>
		public float value;
	}
<<<<<<< HEAD
		
	/// <summary>
	/// Calculate what is the creature's mood based on his meters, and update currentMood.
	/// </summary>
	void CalculateAndUpdateMood(){
		Dictionary<MoodObject, float> moodsDict;

		//moodsDict.Add(bored, (1 - joy) * 0.8f + (1 - loveGeneral) * 0.2f);
		//moodsDict.Add(sick, (1 - health) * 0.7f + (1 - hunger) * 0.2f + (1 - loveGeneral) * 0.1f);
		//moodsDict.Add(hungry, (1 - hunger) * 0.5f + (1 - joy) * 0.3f + (1 - loveGeneral) * 0.1f + (1 - health) * 0.1f);
		//moodsDict.Add(angry, (1 - loveGeneral) * 0.4f + (1 - joy) * 0.3f + (1 - hunger) * 0.3f);
		//moodsDict.Add(happy, (hunger + joy + loveGeneral + health) / 4.0f);
		//moodsDict.Add(sad, 1 - moodsDict[happy]);

		//KeyValuePair<MoodObject, float> highestMood = moodsDict [0];
		//foreach (KeyValuePair<MoodObject, float> mood in moodsDict) {
		//	if (mood.Value > highestMood.Value) {
		//		highestMood = mood;
		//	}
		//}

		//currentMood = highestMood;
	}
=======

	public class Formua {
		/// <summary>
		/// The name of the formula as it appears in the Parameters enum.
		/// </summary>
		public Parameters parameter;
>>>>>>> ee2a590df8b90a3adf132089a6ea9fcc349d8e45

		/// <summary>
		/// The components of the formula.
		/// </summary>
		public FormulaComponent[] components;

		/// <summary>
		/// Evaluates the formula.
		/// </summary>
		/// <returns>The formula.</returns>
		public float EvaluateFormula() {
			// TODO sum all of the formula's components and return result as float.
			return 0f;
		}
	}

    public void tickled()
    {
        //TODO: play tickle animation
        this.loveGeneral += 0.03f;
        this.joy += 0.03f;
    }

    public void onTriggerEnter(Collider2D other)
    {
        ThingObject thingObjectScript = other.GetComponent<ThingObject>();
        if (thingObjectScript != null)
        {
            DoAction(thingObjectScript);

            switch (thingObjectScript.itemType)
            {
                case ThingObject.ItemType.Game:
                        break;
                    
                case ThingObject.ItemType.Food:
                    Destroy(other);
                    Debug.Log("yummy");
                    break;
            }
        }
    }
}

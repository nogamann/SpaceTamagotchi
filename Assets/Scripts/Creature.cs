using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Creature : MonoBehaviour {

	// fields (meters):
	public float hunger = 0.5f;
	public float health = 0.5f;
	public float joy = 0.5f;
	public float playrOneLove = 0.5f;
	public float playrTwoLove = 0.5f;
	// TODO: should be updated everytime love changes
	public float generalLove = (playrOneLove + playrTwoLove) / 2;

	// moods
	Dictionary<string, Dictionary<string, float>> moods = new Dictionary<string, Dictionary<string, float>> {
		{new KeyValuePair<string, Dictionary<string, float>> (
			"happy", 
				new Dictionary<string, float> {
				{"interval", 0.01f},
				{"decrease", 0.01f},
				{"joyCoefficient", 0.0f},
				{"healthCoefficient", 0.0f},
				{"hungerCoefficient", 0.0f},
				{"playerOneLoveCoefficient", 0.0f},
				{"playerTwoLoveCoefficient", 0.0f},
				{"generalLoveCoefficient", 0.0f},
				{"score", 0}
		})},
		{new KeyValuePair<string, Dictionary<string, float>> (
			"sad", 
			new Dictionary<string, float> {
				{"interval", 0.01f},
				{"decrease", 0.01f},
				{"joyCoefficient", 0.0f},
				{"healthCoefficient", 0.0f},
				{"hungerCoefficient", 0.0f},
				{"playerOneLoveCoefficient", 0.0f},
				{"playerTwoLoveCoefficient", 0.0f},
				{"generalLoveCoefficient", 0.0f},
				{"score", 0}
			})},
		{new KeyValuePair<string, Dictionary<string, float>> (
			"bored", 
			new Dictionary<string, float> {
				{"interval", 0.01f},
				{"decrease", 0.01f},
				{"joyCoefficient", 0.8f},
				{"healthCoefficient", 0.0f},
				{"hungerCoefficient", 0.0f},
				{"playerOneLoveCoefficient", 0.0f},
				{"playerTwoLoveCoefficient", 0.0f},
				{"generalLoveCoefficient", 0.2f},
				{"score", 0}
			})}
		// TODO add more moods
	};

	public string currentMood;

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
			return playrOneLove;
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

	/// <summary>
	/// eat, play or take medicine
	/// </summary>
	/// <param name="thing">food, toy or medicine.</param>
	void DoAction(ThingObject thing){
		//TODO: play relevant animation

		health += (thing.Health * health^3);
		hunger += (thing.Hunger * hunger^3);
		// TODO: change the love of the current player only
		playrOneLove += (thing.Love * playrOneLove^3);

	}

	/// <summary>
	/// Calculates the mood score.
	/// </summary>
	/// <returns>The mood score, according to the rellevant meters.</returns>
	/// <param name="mood">string. The mood we want to calculate it's score.</param>
	private float calculateMoodScore (string mood) {
		
		float joyCoefficient = moods [mood] ["joyCoefficient"];
		float healthCoefficient = moods [mood] ["healthCoefficient"];
		float hungerCoefficient = moods [mood] ["hungerCoefficient"];
		float playerOnelLoveCoefficient = moods [mood] ["playerOnelLoveCoefficient"];
		float playerTwoLoveCoefficient = moods [mood] ["playerTwoLoveCoefficient"];
		float generalLoveCoefficient = moods [mood] ["generalLoveCoefficient"];


		float moodScore = (1 - health) * healthCoefficient + (1 - hunger) * hungerCoefficient + (1 - generalLove) * generalLoveCoefficient +
			(1 - joy) * joyCoefficient + (1 - playrOneLove) * playerOnelLoveCoefficient + (1 - playrTwoLove) * playerTwoLoveCoefficient;

		return moodScore;
	}
		
	/// <summary>
	/// Calculate what is the creature's mood based on his meters, and update currentMood.
	/// </summary>
	void CalculateAndUpdateMood(){
		// TODO maybe need a different init?
		string nextMood = "bored";

		foreach (var mood in moods) {
			mood.Value ["score"] = calculateMoodScore (mood.Key);
			if (mood.Value["score"] > moods[nextMood]["score"]) {
				nextMood = mood.Key;
			}
		}

		currentMood = nextMood;
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


//		Dictionary<MoodObject, float> moodsDict;

//		moodsDict.Add(bored, (1 - joy) * 0.8f + (1 - loveGeneral) * 0.2f);
//		moodsDict.Add(sick, (1 - health) * 0.7f + (1 - hunger) * 0.2f + (1 - loveGeneral) * 0.1f);
//		moodsDict.Add(hungry, (1 - hunger) * 0.5f + (1 - joy) * 0.3f + (1 - loveGeneral) * 0.1f + (1 - health) * 0.1f);
//		moodsDict.Add(angry, (1 - loveGeneral) * 0.4f + (1 - joy) * 0.3f + (1 - hunger) * 0.3f);
//		moodsDict.Add(happy, (hunger + joy + loveGeneral + health) / 4.0f);
//		moodsDict.Add(sad, 1 - moodsDict[happy]);

//		KeyValuePair<MoodObject, float> highestMood = moodsDict [0];
//		foreach (KeyValuePair<MoodObject, float> mood in moodsDict) {
//			if (mood.Value > highestMood.Value) {
//				highestMood = mood;
//			}
//		}

//		currentMood = highestMood;
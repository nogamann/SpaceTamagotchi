using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Creature : MonoBehaviour
{
	/// <summary>
	/// Parameters enum. Holds mood coefficients, mood formulas and behavior formulas
	/// Names of coefficient parameters begin with 'c' and names of formulas begin with 'f', followed 
	/// by the mood/behavior/operation that it belomngs to and ends with the meter that it will affect.
	/// </summary>
    public enum Parameters
    {
		joy,
		health,
		hunger,
		generalLove,
		playerOneLove,
		playerTwoLove,
		cDecreaseJoy,
		cDecreaseHealth,
		cDecreaseHunger,
		cDecreaseGeneralLove,
		cHappyJoy,
		cHappyHealth,
		cHappyHunger,
		cHappyGeneralLove,
		cHappyPlayerOneLove,
		cHappyPlayerTwoLove,
		happy,
		sad,
		eat,
		cEatJoy,
		cEatHealth,
		cEatHunger,
		cEatGeneralLove,
		cEatPlayerOneLove,
		cEatPlayerTwoLove
    }

    // meters
	public float joy = 0.5f;
	public float health = 0.5f;
    public float hunger = 0.5f;
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

	public Creature creature;

	public Formula[] _formulas;

	Dictionary<Parameters, Formula> moodsDictionary;
	Dictionary<Parameters, Formula> actionsDictionary;

	Dictionary<Parameters, float> metersDictionary;
	Dictionary<Parameters, float> coefficientsDictionary;

	public Parameters currentMood;

	void Awake ()
	{
		creature = GetComponent<Creature> ();

		metersDictionary = new Dictionary<Parameters, float>() {
			{Parameters.joy, joy}, 
			{Parameters.health, health}, 
			{Parameters.hunger, hunger}, 
			{Parameters.generalLove, generalLove}
		};

		coefficientsDictionary = new Dictionary<Parameters, float> () { 
			{Parameters.cHappyJoy, 0.2f},
			{Parameters.cHappyHealth, 0.2f},
			{Parameters.cHappyHunger, 0.2f},
			{Parameters.cHappyGeneralLove, 0.2f},
			{Parameters.cHappyPlayerOneLove, 0.2f},
			{Parameters.cHappyPlayerTwoLove, 0.2f}
		};
		
		_formulas = new Formula[20];

		// happy mood utility
		FormulaComponent utilityHappyJoyComponent = new FormulaComponent { parameter = Parameters.cHappyJoy, value = -0.02f };
		FormulaComponent utilityHappyHealthComponent = new FormulaComponent { parameter = Parameters.cHappyHealth, value = -0.02f };
		FormulaComponent utilityHappyHungerComponent = new FormulaComponent { parameter = Parameters.cHappyHunger, value = -0.02f };
		FormulaComponent utilityHappyGeneralLoveComponent = new FormulaComponent { parameter = Parameters.cHappyGeneralLove, value = -0.02f };

		FormulaComponent[] utilityHappyComponentList = new FormulaComponent[4];

		utilityHappyComponentList[0] = utilityHappyJoyComponent;
		utilityHappyComponentList[1] = utilityHappyHealthComponent;
		utilityHappyComponentList[2] = utilityHappyHungerComponent;
		utilityHappyComponentList[3] = utilityHappyGeneralLoveComponent;
		Formula utilityHappy = new Formula {creature = this.creature, parameter = Parameters.happy, components = utilityHappyComponentList};
		_formulas [0] = utilityHappy;

		// eat action utility
		FormulaComponent utilityEatJoyComponent = new FormulaComponent { parameter = Parameters.cEatJoy, value = 0.03f };
		FormulaComponent utilityEatHealthComponent = new FormulaComponent { parameter = Parameters.cEatHealth, value = 0.02f };
		FormulaComponent utilityEatHungerComponent = new FormulaComponent { parameter = Parameters.cEatHunger, value = 0.7f };
		FormulaComponent utilityEatGeneralLoveComponent = new FormulaComponent { parameter = Parameters.cEatGeneralLove, value = 0.0f };
		FormulaComponent utilityEatPlayerOneLoveComponent = new FormulaComponent { parameter = Parameters.cEatPlayerOneLove, value = 0.2f };
		FormulaComponent utilityEatPlayerTwoLoveComponent = new FormulaComponent { parameter = Parameters.cEatPlayerTwoLove, value = 0.05f };

		FormulaComponent[] utilityEatComponentList = new FormulaComponent[6];

		utilityEatComponentList [0] = utilityEatJoyComponent;
		utilityEatComponentList [1] = utilityEatHealthComponent;
		utilityEatComponentList [2] = utilityEatHungerComponent;
		utilityEatComponentList [3] = utilityEatGeneralLoveComponent;
		utilityEatComponentList [4] = utilityEatPlayerOneLoveComponent;
		utilityEatComponentList [5] = utilityEatPlayerTwoLoveComponent;
		Formula utilityEat = new Formula { creature = this.creature, parameter = Parameters.eat, components = utilityEatComponentList };
		_formulas [1] = utilityEat;

		moodsDictionary = new Dictionary<Parameters, Formula> ();
		moodsDictionary [Parameters.happy] = utilityHappy;
		moodsDictionary [Parameters.eat] = utilityEat;
	}

	// Use this for initialization
	void Start()
	{
		// the first mood of the creature at birth is Happy
		currentMood = Parameters.happy;
	}

	// Update is called once per frame
	void Update()
	{
		//check if the creature is dead
		CalculateMoodScore(Parameters.happy);
		// update mood
		CalculateAndUpdateMood();
	}

	void FixedUpdate()
	{
		// TODO update meter example, don't forget to remove!
		//        float meter = 0;
		//        float change = -0.002f; // in seconds * -1
		//
		//        meter += change * Time.fixedDeltaTime;


		DecreaseMeters ();
	}

    /// <summary>
    /// eat, play or take medicine
    /// </summary>
    /// <param name="thing">food, toy or medicine.</param>
    void DoAction(ThingObject thing)
    {
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
	private float CalculateMoodScore(Parameters mood)
    {
		float result = moodsDictionary [mood].Eval (coefficientsDictionary);
		Debug.Log ("Calculating mood " + mood + " score\n Score is: " + result);
		return result;
    }

    /// <summary>
    /// Calculate what is the creature's mood based on his meters, and update currentMood.
    /// </summary>
    void CalculateAndUpdateMood()
    {
		// init the next mood and it's score with the current mood
		var nextMood = currentMood;
		var nextMoodScore = CalculateMoodScore (currentMood);

		// find the mood with higest score
		foreach (var mood in moodsDictionary) {
			var moodScore = CalculateMoodScore (mood);
			if (moodScore > nextMoodScore) {
				nextMood = mood;
				nextMoodScore = moodScore;
			}
		}

		// change the current mood to the mood with the highest score
		currentMood = nextMood;
    }

	/// <summary>
	/// Decreases the meters of the creatures according to the current mood.
	/// </summary>
    void DecreaseMeters()
    {
		// TODO change the xUpdate parameters with the enum coefficients.
		joy += joyUpdate * Time.fixedDeltaTime;
		health += healthUpdate * Time.fixedDeltaTime;
		hunger += hungerUpdate * Time.fixedDeltaTime;
		generalLove += generalLoveUpdate * Time.fixedDeltaTime;
    }

	/// <summary>
	/// Gets the value.
	/// </summary>
	/// <returns>The value.</returns>
	/// <param name="parameter">Parameter.</param>
	/// <param name="values">Values.</param>
	public float GetValue(Parameters parameter, Dictionary<Parameters, float> values) {
		if (values.ContainsKey(parameter)) return values[parameter];
		if (moodsDictionary.ContainsKey(parameter)) return moodsDictionary[parameter].Eval(values);
		throw new Exception("No parameter " + parameter + " given and no formula to calculate it");
	}

	/// <summary>
	/// Called when a thing is used on the creature.
	/// This function will destroy the thing if it's consumable, or just use it if it's a game.
	/// </summary>
	/// <param name="collision">Collision.</param>
	void OnCollisionEnter(Collision collision)
	{
		// TODO change name and maybe the signature (if the object should not be effected by the collision, but by being over the creature).

		// get the object that touched the creature
		ThingObject thing = collision.collider.GetComponent<ThingObject>();

		// 
		if (thing == null) {
			Debug.LogError ("Collided with something that is not a ThingObject!");
			return;
		}

		// perform the relevant action with the thing
		DoAction (thing);

		// destroy the object if it's consumable (food or medicine)
		if (thing.itemType != ThingObject.ItemType.Game) {
			Destroy (thing);
		}
	}

    
	// getters and setters

	public Parameters CurrentMood
    {
        get
        {
            return currentMood;
        }
        set
        {
            CurrentMood = value;
        }
    }

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

    public float Joy
    {
        get
        {
            return joy;
        }
        set
        {
            joy = value;
        }
    }

    public float Hunger
    {
        get
        {
            return hunger;
        }
        set
        {
            hunger = value;
        }
    }

    public float LovePlayrOne
    {
        get
        {
            return playerOneLove;
        }
        set
        {
            LovePlayrOne = value;
        }
    }

    public float LovePlayrTwo
    {
        get
        {
            return playrTwoLove;
        }
        set
        {
            LovePlayrTwo = value;
        }
    }

    public float LoveGeneral
    {
        get
        {
            return generalLove;
        }
        set
        {
            LoveGeneral = value;
        }
    }

    public class moodClass
    {
        // TODO needed?
    }
}

/// <summary>
/// Formula component.
/// </summary>
[Serializable]
public class FormulaComponent
{
	/// <summary>
	/// The name of the formula component as it appears in the enum.
	/// </summary>
	public Creature.Parameters parameter;

	/// <summary>
	/// The meter that this coefficient will effect.
	/// </summary>
	public Creature.Parameters meter;

	/// <summary>
	/// The value of the component.
	/// </summary>
	public float value;
}

/// <summary>
/// Represents Formua.
/// </summary>
[Serializable]
public class Formula : MonoBehaviour
{
	public Creature creature;

	/// <summary>
	/// The name of the formula as it appears in the Parameters enum.
	/// </summary>
	public Creature.Parameters parameter;

	/// <summary>
	/// The components of the formula.
	/// </summary>
	public FormulaComponent[] components;

	public void Awake ()
	{
		
	}

	/// <summary>
	/// Evaluates the formula.
	/// </summary>
	/// <returns>The result of the formula.</returns>
	public float EvaluateFormula()
	{
		// TODO sum all of the formula's components and return result as float.
		float result = 0;
		foreach (var component in components) {
			result += component.value;
		}
		return result;
	}

	/// <summary>
	/// Eval the specified values.
	/// </summary>
	/// <param name="values">Values.</param>
	public float Eval(Dictionary<Creature.Parameters, float> values) {
		float result = 0f;
		foreach (var formulaComp in components) {
			var value = creature.GetValue (formulaComp.parameter, values);
			result += value * formulaComp.value;
		}
		return result;
	}
}

///// <summary>
///// This class represents the creature in the game. All of it's mood and behaviors declered and determined here.
///// </summary>
//public class Creature : MonoBehaviour
//{
//	// meters
//	public float joy = 0.5f;
//	public float health = 0.5f;
//    public float hunger = 0.5f;
//    public float playerOneLove = 0.5f;
//    public float playrTwoLove = 0.5f;
//    public float generalLove = 0.5f;
//
//    // meters update time
//    public float hungerDecrease;
//    public float healthDecrease;
//    public float joyDecrease;
//    public float playerOneLoveDecrease;
//    public float playrTwoLoveDecrease;
//    public float generalLoveDecrease;
//
//
//	/// <summary>
//	/// Decreases the meters of the creatures according to the current mood.
//	/// </summary>
//    void DecreaseMeters()
//    {
//		joy += joyDecrease * Time.fixedDeltaTime;
//		health += healthDecrease * Time.fixedDeltaTime;
//		hunger += hungerDecrease * Time.fixedDeltaTime;
//		generalLove += generalLoveDecrease * Time.fixedDeltaTime;
//		playerOneLove += playerOneLoveDecrease * Time.fixedDeltaTime;
//		playrTwoLove += playrTwoLoveDecrease * Time.fixedDeltaTime;
//    }
//
//	void FixedUpdate()
//	{
//		DecreaseMeters ();
//	}
//
//
//
//	/// <summary>
//	/// This class represents a formula. Formulas are used to caculate
//	/// the influences of actions on the creature's meters.
//	/// </summary>
//	[Serializable]
//	public class Formula
//	{
//		// the creature object
//		private Creature creature;
//
//		// coefficients of the meters in the formula
//		public float cJoy;
//		public float cHealth;
//		public float cHunger;
//		public float cGeneralLove;
//		public float cPlayerOneLove;
//		public float cPlayerTwoLove;
//
//		/// <summary>
//		/// Evaluates the formula.
//		/// </summary>
//		/// <returns>The formula's result.</returns>
//		public float evaluateFormula()
//		{
//			float result = cJoy * creature.joy + cHealth * creature.health + cHunger
//				* creature.hunger + cGeneralLove + creature.generalLove + cPlayerOneLove
//				* creature.playerOneLove + cPlayerTwoLove * creature.playrTwoLove;
//			return result;
//		}
//	}
//}
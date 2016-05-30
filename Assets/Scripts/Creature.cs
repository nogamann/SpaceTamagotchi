using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

///// <summary>
///// This class represents the creature in the game. All of it's mood and behaviors declered and determined here.
///// </summary>
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

	// arrays of possible actions of each type of item
	Parameters[] _foodActions;
	Parameters[] _medicineActions;
	Parameters[] _gameActions;

	// a dictionary to connect between the action arrays to the appropriate item type
	Dictionary<ThingObject.ItemType, Parameters[]> itemPossibleActions;

	public Parameters currentMood;

	// TODO remove these after debug!
	ThingObject food;



	void Awake ()
	{
		creature = GetComponent<Creature> ();

		// init dictionaries
		moodsDictionary = new Dictionary<Parameters, Formula> ();
		actionsDictionary = new Dictionary<Parameters, Formula> ();
		itemPossibleActions = new Dictionary<ThingObject.ItemType, Parameters[]> () {
			{ThingObject.ItemType.Food, _foodActions},
			{ThingObject.ItemType.Medicine, _medicineActions},
			{ThingObject.ItemType.Game, _gameActions}
		};

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

		// init arrays
		_formulas = new Formula[20];
		_foodActions = new Parameters[1];
		_medicineActions = new Parameters[1];
		_gameActions = new Parameters[1];

		// happy mood utility
		FormulaComponent utilityHappyJoyComponent = new FormulaComponent { parameter = Parameters.cHappyJoy, value = 0.6f };
		FormulaComponent utilityHappyHealthComponent = new FormulaComponent { parameter = Parameters.cHappyHealth, value = 0.1f };
		FormulaComponent utilityHappyHungerComponent = new FormulaComponent { parameter = Parameters.cHappyHunger, value = 0.2f };
		FormulaComponent utilityHappyGeneralLoveComponent = new FormulaComponent { parameter = Parameters.cHappyGeneralLove, value = 0.1f };

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
		_foodActions[0] = Parameters.eat;

		moodsDictionary [Parameters.happy] = utilityHappy;

		actionsDictionary [Parameters.eat] = utilityEat;

		// TODO remove the next objects after debug!
		food = new ThingObject () { itemType = ThingObject.ItemType.Food };
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
		// TODO decide what are the levels of the meters that determine death

		// decrease meters
		// TODO decrease meters by the current mood
		DecreaseMeters();

		// update mood
		CalculateAndUpdateMood();

		// TODO remove after debug!
		if (Input.GetKeyDown (KeyCode.C)) {
			Debug.Log ("'c' is pressed!");
			ChooseAction (food);
		}

	}

	void FixedUpdate()
	{
		// update meters as time passes
		DecreaseMeters ();
	}

	/// <summary>
	/// Calculates the action score.
	/// </summary>
	/// <returns>The action score.</returns>
	/// <param name="action">Action.</param>
	float CalculateActionScore(Parameters action)
	{
		float result = actionsDictionary [action].Eval (coefficientsDictionary);
		Debug.Log ("Calculating action " + action + " score\n Score is: " + result);
		return result;
	}

    /// <summary>
    /// eat, play or take medicine
    /// </summary>
    /// <param name="thing">food, toy or medicine.</param>
    void ChooseAction(ThingObject thing)
    {
		// TODO create an idle action to prevent null pointer exception in the rare case no action was chosen
		Parameters chosenAction = Parameters.eat;
		float actionScore = 0.0f;

		// TODO remove
		Debug.LogError ("itemType: " + thing.itemType);

		// TODO remove
		Debug.LogError ("itemPossibleActions[thing.itemType]: " + itemPossibleActions[thing.itemType]);

		// TODO remove
		Debug.LogError ("itemPossibleActions: " + itemPossibleActions);

		// check the type of the item
		Parameters[] possibleActions = itemPossibleActions[thing.itemType];

		// TODO remove
		Debug.LogError ("possibleActions: " + possibleActions);

		// calculate the possible actions' scores according to it's type
		foreach (var action in possibleActions) {

			// TODO remove
			Debug.LogError ("action: " + action);

			float currentActionScore = CalculateActionScore(action);

			if (currentActionScore > actionScore) {
				actionScore = currentActionScore;
				chosenAction = action;
			}
		}

		Debug.Log ("Chosen action is: " + chosenAction);

		// perform the chosen action
		DoAction(thing, chosenAction);
    }

	/// <summary>
	/// Dos the action.
	/// </summary>
	/// <param name="item">Item.</param>
	/// <param name="action">Action.</param>
	void DoAction(ThingObject item, Parameters action)
	{
		Debug.Log ("Performing action: " + action + " on " + item);

		// play relevant animation
		// update the relevant meters according to the effect of the action




		//        //TODO: play relevant animation
		//
		//        health += (thing.Health * Mathf.Pow(health, 3));
		//        hunger += (thing.Hunger * Mathf.Pow(hunger, 3));
		//        joy += (thing.Joy * Mathf.Pow(joy, 3));
		//
		//        // TODO: change the love of the current player only
		//        playerOneLove += (thing.Love * Mathf.Pow(playerOneLove, 3));
		//
		//        // TODO add impact of the love to the performing player (should be implemented as a formula in the enum)
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
			var moodScore = CalculateMoodScore (mood.Key);
			if (moodScore > nextMoodScore) {
				nextMood = mood.Key;
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
		ChooseAction (thing);

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
public class Formula
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

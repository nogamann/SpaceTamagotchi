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
		happy,
		sad,
		eat
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

	Dictionary<Parameters, Formula> formulasDictionary;
	Dictionary<Parameters, float> metersDictionary;

	public Parameters currentMood;


	void Awake ()
	{
		creature = GetComponent<Creature> ();


		formulasDictionary = new Dictionary<Parameters, Formula> ();
		foreach (var item in _formulas) {
			formulasDictionary [item.parameter] = item;
		}

		metersDictionary = new Dictionary<Parameters, float> () {
			{Parameters.joy, joy},
			{Parameters.hunger, hunger},
			{Parameters.health, health},
			{Parameters.generalLove, generalLove},
			{Parameters.playerOneLove, playerOneLove},
			{Parameters.playerTwoLove, playrTwoLoveUpdate}
		};
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
		// check if the creature is dead
		IsDead();

		// update mood
//		CalculateAndUpdateMood();

		// TODO remove after debug!
//		if (Input.GetKeyDown (KeyCode.C)) {
//			Debug.Log ("'c' is pressed!");
//			ChooseAction (food);
//		}
	}

	void FixedUpdate()
	{
		// update meters as time passes
		DecreaseMeters ();
	}

	void IsDead()
	{
		if (joy <= 0.1 && health <= 0.1 && generalLove <= 0.1)
		{
			// TODO game over
		}
	}


    /// <summary>
    /// eat, play or take medicine
    /// </summary>
    /// <param name="thing">food, toy or medicine.</param>
    void ChooseAction(ThingObject thing)
    {
		// iterate through actions dictionary
		// calculate 

		// TODO create an idle action to prevent null pointer exception in the rare case no action was chosen
		Parameters chosenAction = Parameters.eat;
		float actionScore = 0.0f;

		// calculate the possible actions' scores according to it's type
		foreach (Parameters action in formulasDictionary.Keys) {

			// TODO remove
			Debug.LogError ("action: " + action);

			float currentActionScore = GetValue(action, metersDictionary);

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
		float result = formulasDictionary [mood].Eval (metersDictionary);
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
		var nextMoodScore = GetValue (currentMood, metersDictionary);

		// find the mood with higest score
		foreach (var mood in formulasDictionary.Keys) {
//			var moodScore = CalculateMoodScore (mood.Key);
			var moodScore = GetValue (mood, metersDictionary);
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
	/// Gets the value of a Parameter.
	/// </summary>
	/// <returns>The value of the parameter.</returns>
	/// <param name="parameter">Parameter.</param>
	/// <param name="values">Values.</param>
	public float GetValue(Parameters parameter, Dictionary<Parameters, float> values) {
		if (values.ContainsKey(parameter)) return values[parameter];
		if (formulasDictionary.ContainsKey(parameter)) return formulasDictionary[parameter].Eval(values);
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
	/// The value of the component.
	/// </summary>
	public float coefficient;
}

/// <summary>
/// Represents Formua.
/// </summary>
[Serializable]
public class Formula
{
	// TODO get reference to the creature in Awake function
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
		Debug.Log ("values: " + values);
		foreach (var formulaComp in components) {
			Debug.Assert (values != null);
			Debug.Assert (formulaComp != null);
			var value = creature.GetValue (formulaComp.parameter, values);
			result += value * formulaComp.coefficient;
		}
		return result;
	}
}

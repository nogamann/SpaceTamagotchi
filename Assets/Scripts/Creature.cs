﻿using UnityEngine;
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
    public enum CreatureParams
    {
		joy = 0,
		health = 1,
		hunger = 2,
		generalLove = 3,
		playerOneLove = 4,
		playerTwoLove = 5,
		happy = 6,
		sad = 7,
		hungry = 8,
		bored = 9,
		ill = 10,
		angry = 11,
		eating = 16,
		playing = 17,
		loving = 18,
		notWanting = 19,
		blinking = 20,
		takingMedicine = 21
    }

    // TODO: should be updated everytime love changes
    //	public float generalLove = (playrOneLove + playrTwoLove) / 2;

	public float updateMoodInterval;

    // meters update time
    public float hungerUpdate;
    public float healthUpdate;
    public float joyUpdate;
    public float playerOneLoveUpdate;
    public float playrTwoLoveUpdate;
    public float generalLoveUpdate;

	public Creature creature;

	public Formula[] _formulas;
	public CreatureParams[] _moods;
	public CreatureParams[] _actions;

	public Dictionary<CreatureParams, float> metersDictionary;
	Dictionary<CreatureParams, Formula> formulasDictionary;
	Dictionary<CreatureParams, Formula> moodsDictionary;
	Dictionary<CreatureParams, Formula> actionsDictionary;

	public CreatureParams currentMood;

	ThingObject burgerItem;
	GameObject hamburger;


	void Awake ()
	{
		// init formulas dictonary
		formulasDictionary = new Dictionary<CreatureParams, Formula> ();
		foreach (var item in _formulas) {
			formulasDictionary [item.parameter] = item;
		}

		// init moods dictionary
		moodsDictionary = new Dictionary<CreatureParams, Formula> ();
		foreach (var item in _formulas) {
			if (Array.Exists<CreatureParams> (_moods, element => element == item.parameter)) {
				moodsDictionary [item.parameter] = item;
			}
		}

		// innit actions dictionary
		actionsDictionary = new Dictionary<CreatureParams, Formula> ();
		foreach (var item in _formulas) {
			if (Array.Exists<CreatureParams> (_actions, element => element == item.parameter)) {
				actionsDictionary [item.parameter] = item;
			}
		}

		// init meters dictionary
		metersDictionary = new Dictionary<CreatureParams, float> () {
			{CreatureParams.joy, 0},
			{CreatureParams.hunger, 0},
			{CreatureParams.health, 0},
			{CreatureParams.generalLove, 0},
			{CreatureParams.playerOneLove, 0},
			{CreatureParams.playerTwoLove, 0}
		};
			
//		burgerItem = new ThingObject() {itemType = ThingObject.ItemType.Food, joy = 1, health = -2, hunger = 5, love = 1};
//		hamburger = new GameObject ();
//		hamburger.AddComponent (burgerItem);
	}

	// Use this for initialization
	void Start()
	{
		Debug.Log ("actions in actionsDictionary:");
		foreach (var action in actionsDictionary) {
			Debug.Log (action.Key);
		}
	}

	// Update is called once per frame
	void Update()
	{
		// check if the creature is dead
		IsDead();

		// update mood
		StartCoroutine(CalculateAndUpdateMood ());

		// TODO remove after debug!
//		if (Input.GetKeyDown (KeyCode.C)) {
//			Debug.Log ("'c' is pressed!");
//			Debug.Assert (this.hamburger != null);
//			ChooseAction (this.hamburger);
//		}
	}

	void FixedUpdate()
	{
		// update meters as time passes
		DecreaseMeters ();
	}

	void IsDead()
	{
		if (metersDictionary[CreatureParams.joy] <= 0.1 && metersDictionary[CreatureParams.health] <= 0.1 && metersDictionary[CreatureParams.generalLove] <= 0.1)
		{
			// TODO game over
		}
	}


    /// <summary>
    /// eat, play or take medicine
    /// </summary>
    /// <param name="thing">food, toy or medicine.</param>
	void ChooseAction(ThingObject item)
    {
		Debug.Assert (item != null);

		// TODO create an idle action to prevent null pointer exception in the rare case no action was chosen
		CreatureParams chosenAction = CreatureParams.eating;
		float actionScore = 0.0f;

		// calculate the possible actions' scores according to it's type
		foreach (CreatureParams action in actionsDictionary.Keys) {

			float currentActionScore = GetValue(action, metersDictionary);

			if (currentActionScore > actionScore) {
				actionScore = currentActionScore;
				chosenAction = action;
			}
		}

		Debug.Log ("Chosen action is: " + chosenAction);

		// perform the chosen action
		DoAction(item, chosenAction);
    }

	/// <summary>
	/// Dos the action.
	/// </summary>
	/// <param name="item">Item.</param>
	/// <param name="action">Action.</param>
	void DoAction(ThingObject item, CreatureParams action)
	{
		Debug.Assert (item != null);

		Debug.Log ("Performing action: " + action + " on " + item);

		// TODO play relevant animation

		// update the relevant meters according to the effect of the action
		foreach (var meter in metersDictionary.Keys) {
			metersDictionary[meter] += item.metersEffect [meter];
		}

        // TODO add impact of the love to the performing player (should be implemented as a formula in the enum)
	}

    /// <summary>
    /// Calculate what is the creature's mood based on his meters, and update currentMood.
    /// </summary>
	IEnumerator CalculateAndUpdateMood()
    {
		while (true) {
			// init the next mood and it's score with the current mood
			var nextMood = currentMood;
			Debug.Assert (metersDictionary != null);
			var nextMoodScore = GetValue (currentMood, metersDictionary);

			// find the mood with higest score
			foreach (var mood in moodsDictionary.Keys) {
				var moodScore = GetValue (mood, metersDictionary);
				if (moodScore > nextMoodScore) {
					nextMood = mood;
					nextMoodScore = moodScore;
				}
			}

			// change the current mood to the mood with the highest score
			currentMood = nextMood;

			// TODO remove
//			Debug.LogError ("MOOD is: " + currentMood);

			// delay the mood calculation
			yield return new WaitForSeconds (updateMoodInterval);
		}
    }

	/// <summary>
	/// Decreases the meters of the creatures according to the current mood.
	/// </summary>
    void DecreaseMeters()
    {
		// TODO change the xUpdate parameters with the enum coefficients.
		metersDictionary[CreatureParams.joy] += joyUpdate * Time.fixedDeltaTime;
		metersDictionary[CreatureParams.health] += healthUpdate * Time.fixedDeltaTime;
		metersDictionary[CreatureParams.hunger] += hungerUpdate * Time.fixedDeltaTime;
		metersDictionary[CreatureParams.generalLove] += generalLoveUpdate * Time.fixedDeltaTime;
    }

	/// <summary>
	/// Gets the value of a Parameter.
	/// </summary>
	/// <returns>The value of the parameter.</returns>
	/// <param name="parameter">Parameter.</param>
	/// <param name="values">Values.</param>
	public float GetValue(CreatureParams parameter, Dictionary<CreatureParams, float> values) {
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
	public Creature.CreatureParams parameter;

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
	public Creature.CreatureParams parameter;

	/// <summary>
	/// The components of the formula.
	/// </summary>
	public FormulaComponent[] components;

	/// <summary>
	/// Eval the specified values.
	/// </summary>
	/// <param name="values">Values.</param>
	public float Eval(Dictionary<Creature.CreatureParams, float> values) {
		float result = 0f;

		foreach (var formulaComp in components) {
			Debug.Assert (values != null);
			Debug.Assert (formulaComp != null);
			var value = creature.GetValue (formulaComp.parameter, values);
			result += value * formulaComp.coefficient;
		}

		return result;
	}
}

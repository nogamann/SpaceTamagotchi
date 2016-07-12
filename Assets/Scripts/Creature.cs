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
    public enum CreatureParams
    {
		joy = 0,
		health = 1,
		hunger = 2,
		generalLove = 3,
		myLove = 4,
		otherLove = 5,
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
		takingMedicine = 21,
		one = 22,
		joyComplement = 23,
		healthComplement = 24,
		hungerComplement = 25,
		generalLoveComplement = 26,
		myLoveComplement = 27,
		otherLoveComplement = 28,
		notWantingToPlay = 29,
		notWantingToEat = 30,
		notWantingToTakeMed = 31
    }

    // TODO: should be updated everytime love changes
    //	public float generalLove = (playrOneLove + playrTwoLove) / 2;

	public float updateMoodInterval;

    // meters update time
    public float hungerUpdate;
    public float healthUpdate;
    public float joyUpdate;
//    public float playerOneLoveUpdate;
//    public float playerTwoLoveUpdate;

	// one is server, two is client
	public int player;

	public Creature creature;
	public GameObject yoyo;

	public Formula[] _formulas;
	public CreatureParams[] _moods;
	public CreatureParams[] _actions;

	public CreatureParams[] _foodActions;
	public CreatureParams[] _gameActions;
	public CreatureParams[] _medicineActions;

	public Dictionary<CreatureParams, float> metersDictionary;
	Dictionary<CreatureParams, Formula> formulasDictionary;
	Dictionary<CreatureParams, Formula> moodsDictionary;
	Dictionary<CreatureParams, Formula> actionsDictionary;

	Dictionary<CreatureParams, Formula> foodActionsDictionary;
	Dictionary<CreatureParams, Formula> gameActionsDictionary;
	Dictionary<CreatureParams, Formula> medicineActionsDictionary;

	public CreatureParams currentMood;

//	ThingObject burgerItem;
//	GameObject hamburger;

	public GameObject gameManager;

    Animator animator;

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

		// innit food actions dictionary
		foodActionsDictionary = new Dictionary<CreatureParams, Formula> ();
		foreach (var item in _formulas) {
			if (Array.Exists<CreatureParams> (_foodActions, element => element == item.parameter)) {
				foodActionsDictionary [item.parameter] = item;
			}
		}

		// innit game actions dictionary
		gameActionsDictionary = new Dictionary<CreatureParams, Formula> ();
		foreach (var item in _formulas) {
			if (Array.Exists<CreatureParams> (_gameActions, element => element == item.parameter)) {
				gameActionsDictionary [item.parameter] = item;
			}
		}

		// innit medicine actions dictionary
		medicineActionsDictionary = new Dictionary<CreatureParams, Formula> ();
		foreach (var item in _formulas) {
			if (Array.Exists<CreatureParams> (_medicineActions, element => element == item.parameter)) {
				medicineActionsDictionary [item.parameter] = item;
			}
		}

		// init meters dictionary
		metersDictionary = new Dictionary<CreatureParams, float> () {
			{CreatureParams.joy, 0.8f},
			{CreatureParams.hunger, 0.8f},
			{CreatureParams.health, 0.8f},
			{CreatureParams.myLove, 0.8f},
			{CreatureParams.otherLove, 0.8f},
			{CreatureParams.one, 1}
		};
			
		//burgerItem = new ThingObject() {itemType = ThingObject.ItemType.Food, joy = 1, health = -2, hunger = 5, love = 1};
		//hamburger = new GameObject ();
		//hamburger.AddComponent(burgerItem);
	}

	// Use this for initialization
	void Start()
	{
        animator = this.GetComponentInChildren<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		// check if the creature is dead
		IsDead();

		// update mood
		StartCoroutine(CalculateAndUpdateMood ());

//		// TODO remove after debug!
//		if (Input.GetKeyDown (KeyCode.C)) {
//			Debug.Log ("'c' is pressed!");
//			Debug.Assert (this.hamburger != null);
//			//ChooseAction (this.hamburger);
//		}
	}

	void FixedUpdate()
	{
		if (player == 1) {
			metersDictionary [CreatureParams.myLove] = gameManager.GetComponent<GameManager> ().playerOneLove;
			metersDictionary [CreatureParams.otherLove] = gameManager.GetComponent<GameManager> ().playerTwoLove;
		} else {
			metersDictionary [CreatureParams.myLove] = gameManager.GetComponent<GameManager> ().playerTwoLove;
			metersDictionary [CreatureParams.otherLove] = gameManager.GetComponent<GameManager> ().playerOneLove;
		}

		// update meters as time passes
		DecreaseMeters ();
	}

	void IsDead()
	{
		if (GetValue(CreatureParams.joy, metersDictionary) <= 0.1 &&
			GetValue(CreatureParams.hunger, metersDictionary) <= 0.1 &&
			GetValue(CreatureParams.health, metersDictionary) <= 0.1 &&
			GetValue(CreatureParams.generalLove, metersDictionary) <= 0.1 )
		{
			gameManager.GetComponent<GameManager>().GameOver();
			this.GetComponentInChildren<SpriteRenderer> ().color = new Color (1,1,1,0.2f);
		}
	}


    /// <summary>
    /// eat, play or take medicine
    /// </summary>
    /// <param name="thing">food, toy or medicine.</param>
//	public void ChooseAction(ThingObject item)
	public void ChooseAction(GameObject item)
    {
		Debug.Assert (item != null);

		Dictionary<CreatureParams, Formula> relevantActions = formulasDictionary;

//		switch (item.itemType) {
		switch (item.GetComponent<ThingObject>().itemType) {
		case ThingObject.ItemType.Food:
			relevantActions = foodActionsDictionary;
			break;
		case ThingObject.ItemType.Game:
			relevantActions = gameActionsDictionary;
			break;
		case ThingObject.ItemType.Medicine:
			relevantActions = medicineActionsDictionary;
			break;
		}


		// TODO create an idle action to prevent null pointer exception in the rare case no action was chosen
		CreatureParams chosenAction = CreatureParams.blinking;
		float actionScore = 0.0f;

		// calculate the possible actions' scores according to it's type
		foreach (CreatureParams action in relevantActions.Keys) {
			float currentActionScore = GetValue(action, metersDictionary);
			if (currentActionScore > actionScore) {
				actionScore = currentActionScore;
				chosenAction = action;
			}
		}

		Debug.Log ("Chosen action is: " + chosenAction);

        // perform the chosen action
		GameObject[] objects = GameObject.FindGameObjectsWithTag ("Finger");
		PlayerController local = null;
		foreach (GameObject go in objects){
			PlayerController pc = go.GetComponent<PlayerController>();
			if (pc.isLocalPlayer) {
				local = pc;
			}
		}
		Debug.Assert (gameObject != null);
		Debug.Assert (local != null);
		local.doAction (item, chosenAction);

//		gameManager.GetComponent<GameManager>().doAction(item, chosenAction);
//        DoAction(item, chosenAction);
    }

	/// <summary>
	/// Dos the action.
	/// </summary>
	/// <param name="item">Item.</param>
	/// <param name="action">Action.</param>
//	void DoAction(ThingObject item, CreatureParams action)
	public void DoAction(GameObject item, CreatureParams action)
	{
		Debug.Assert (item != null);
		Debug.Log ("Performing action: " + action + " " + (int)action +  " on " + item);

		// update meters only if action is not not-wanting
		if ((int)action < 22 ) {
			// update the relevant meters according to the effect of the action
			for (int i = 0; i <= 2; i++) {
//			metersDictionary[(CreatureParams)i] += item.metersEffect[(CreatureParams)i];
				// make sure meter isn't over 1 or under 0
				metersDictionary [(CreatureParams)i] += item.GetComponent<ThingObject> ().metersEffect [(CreatureParams)i];
				if (metersDictionary [(CreatureParams)i] < 0) {
					metersDictionary [(CreatureParams)i] = 0;
				}

				if (metersDictionary [(CreatureParams)i] > 1) {
					metersDictionary [(CreatureParams)i] = 1;
				}
			}

			float love = item.GetComponent<ThingObject> ().metersEffect [CreatureParams.generalLove];
			if (player == 1) {
				gameManager.GetComponent<GameManager> ().UpdatePlayerOneLove (love);
			} else {
				gameManager.GetComponent<GameManager> ().UpdatePlayerTwoLove (love);
			}



			//		if (item.itemType != ThingObject.ItemType.Game)

			//Debug.Log("item isnt a game");
			if (action == CreatureParams.eating || action == CreatureParams.takingMedicine)
			{
				//Debug.Log("destroy item");
				//Destroy(item);
				item.transform.position = new Vector3(-100,-100,-999);
				item.SetActive(false);
				item.GetComponent<TouchControl> ().disabled = true;
			}
			//if is game
			if (action == CreatureParams.playing) {
				// if item is a game, make it disappear and re-appear in another place (not on the creature)
				item.SetActive(false);

				// if it's yoyo, make it disappear while animation is playing
				if (item.GetComponent<ThingObject> ().specificTypeItem == ThingObject.SpecificItemType.Yoyo) {
					yoyo = item;
				}
				// play happy if playing with something other than yoyo
				else {
					action = CreatureParams.happy;
					item.SetActive (true);
					item.transform.position = new Vector3 (5, UnityEngine.Random.Range (-3, 3), 0);
				}

			}
		}



		animator.SetInteger("action", (int)action);
		animator.SetTrigger("canChange");
		animator.SetInteger("mood", 0);
    }

	public void AnimationEnded(){
		yoyo.SetActive (true);
		yoyo.transform.position = new Vector3 (5, UnityEngine.Random.Range (-3, 3), 0);

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

//			local.playMoodAnimation (currentMood);

            if (animator.GetInteger("mood") != (int)currentMood)
            {
                animator.SetInteger("mood", (int)currentMood);
                animator.SetTrigger("canChange");
            }
           
            // TODO remove
//            Debug.LogError("MOOD is: " + (int)currentMood + currentMood);

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

	public void tickled(){
		CreatureParams chosenAction = CreatureParams.blinking;

		float lovingScore = GetValue(CreatureParams.loving, metersDictionary);
		if (lovingScore > 1 - lovingScore) {
			chosenAction = CreatureParams.loving;

			// update joy
			metersDictionary[CreatureParams.joy] += 0.1f;
			if (metersDictionary [CreatureParams.joy] < 0) {
				metersDictionary [CreatureParams.joy] = 0;
			}

			if (metersDictionary [CreatureParams.joy] > 1) {
				metersDictionary [CreatureParams.joy] = 1;
			}

			// update love
			float love = 0.1f;
			if (player == 1) {
				gameManager.GetComponent<GameManager> ().UpdatePlayerOneLove (love);
			} else {
				gameManager.GetComponent<GameManager> ().UpdatePlayerTwoLove (love);
			}

		} else {
			chosenAction = CreatureParams.notWanting;
		}

		Debug.Log ("Chosen action is: " + chosenAction);
		Debug.Log ("Performing action: " + chosenAction + " " + (int)chosenAction);

		animator.SetInteger("action", (int)chosenAction);
		animator.SetTrigger("canChange");
		animator.SetInteger("mood", 0);
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
		//ThingObject thing = collision.collider.GetComponent<ThingObject>();

		// 
		//if (thing == null) {
		//	Debug.LogError ("Collided with something that is not a ThingObject!");
		//	return;
		//	}

		// perform the relevant action with the thing
		//ChooseAction (thing);

		// destroy the object if it's consumable (food or medicine)
		//if (thing.itemType != ThingObject.ItemType.Game) {
		//	Destroy (thing);
		//}
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

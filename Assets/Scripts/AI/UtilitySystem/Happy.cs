using UnityEngine;
using System.Collections;
using System.Timers;

public class Happy : MoodObject {
	public float joyUpdate;
	public float healthUpdate;
	public float hungerUpdate;
	public float loveUpdate;

	public Creature creature;
	// TODO add another two love timers. One for each player.

	// TODO how to implement an abstract function
//	void updateMeters(){
//		
//	}

	// Use this for initialization
	void Start () {
		joyUpdate = Time.realtimeSinceStartup;
		healthUpdate = Time.realtimeSinceStartup;
		hungerUpdate = Time.realtimeSinceStartup;
		loveUpdate = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update () {
		float realTime = Time.realtimeSinceStartup;

		float joyDecrease = 0.001f;
		float healthDecrease = 0.001f;
		float hungerDecrease = 0.001f;
		float loveDecrease = 0.001f;

		// update joy meter
		if ((realTime - joyUpdate) > 1500) {
			Debug.Log (" -- Joy decreased by 0.01");
			creature.joy -= 0.01f;
		}

		// update health meter
		if ((realTime - healthUpdate) > 1500) {
			Debug.Log (" -- Health decreased by 0.01");
			Creature.health -= 0.01f;
		}

		// update hunger meter
		if ((realTime - hungerUpdate) > 500) {
			Debug.Log (" -- Hunger decreased by " + hungerDecrease);
			Creature.hunger -= hungerDecrease;
		}

		// update love meter
		if ((realTime - loveUpdate) > 2000) {
			Debug.Log (" -- Love decreased by 0.01");
			Creature.love -= 0.01f;
		}
	}

	void Run() {
		
	}
}

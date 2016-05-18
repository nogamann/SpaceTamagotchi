using UnityEngine;
using System.Collections;
using System.Timers;

public class Happy : MoodObject {
	public float joyUpdate;
	public float healthUpdate;
	public float hungerInterval;
	public float loveInterval;
	// TODO add another two love timers. One for each player.

	public void updateMeters(){
		
	}

	// Use this for initialization
	void Start () {
		joyUpdate = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update () {
		float realTime = Time.realtimeSinceStartup;

		// update joy meter
		if ((realTime - joyUpdate) > 1500) {
			Creature.setJoy (-0.01f);
		}

		// update health meter
		if ((realTime - healthUpdate) > 1500) {
			Creature.setHealth (-0.01f);
		}

		// update hunger meter
		if ((realTime - hungerUpdate) > 1500) {
			Creature.setHunger (-0.01f);
		}

		// update love meter
		if ((realTime - loveUpdate) > 1500) {
			Creature.setLove (-0.01f);
		}
	}

	void Run() {
		
	}
}

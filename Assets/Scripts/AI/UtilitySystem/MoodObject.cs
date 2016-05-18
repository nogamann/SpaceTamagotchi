using UnityEngine;
using System.Collections;

public abstract class MoodObject : MonoBehaviour {

	/// <summary>
	/// Updates the creature's meters.
	/// </summary>
	protected abstract void updateMeters ();

	/// <summary>
	/// Plaies the right animation of the creature according to the current mood.
	/// </summary>
	protected abstract void playAnimation ();
}

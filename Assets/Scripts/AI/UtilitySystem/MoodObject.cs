using UnityEngine;
using System.Collections;

public abstract class MoodObject : MonoBehaviour {
    string moodName;

    /// <summary>
    /// Updates the creature's meters.
    /// </summary>
    protected void updateMeters() { }

    /// <summary>
    /// Plaies the right animation of the creature according to the current mood.
    /// </summary>
    protected void playAnimation() { }
}

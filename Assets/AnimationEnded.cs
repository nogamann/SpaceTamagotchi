using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimationEnded : MonoBehaviour {

	public void ReturnYoyo(){
		this.GetComponentInParent<Creature> ().AnimationEnded ();
	}
}

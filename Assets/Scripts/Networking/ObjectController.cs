using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ObjectController : NetworkBehaviour {

	// Update is called once per frame
	void Update () {
		if (!hasAuthority) {
			return;
		}

		// player movement
		var x = Input.GetAxis("Horizontal")*0.1f;
		var z = Input.GetAxis("Vertical")*0.1f;

		transform.Translate (x, 0, z);
	}

	void OnGUI(){
		if (GUI.Button(new Rect(1, 1, 200, 20), "grab item"))
		{
			GameObject[] objects = GameObject.FindGameObjectsWithTag ("Finger");
			PlayerController local = null;
			foreach (GameObject obj in objects){
				PlayerController go = obj.GetComponent<PlayerController>();
				if (go.isLocalPlayer) {
					local = go;
				}
			}
			local.GrabItem (gameObject);
		}

		if (GUI.Button(new Rect(100, 100, 200, 20), "free item"))
		{
//			PlayerController go = GameObject.FindGameObjectWithTag ("Finger").GetComponent<PlayerController>();
//			go.FreeItem (gameObject);
			GameObject[] objects = GameObject.FindGameObjectsWithTag ("Finger");
			PlayerController local = null;
			foreach (GameObject obj in objects){
				PlayerController go = obj.GetComponent<PlayerController>();
				if (go.isLocalPlayer) {
					local = go;
				}
			}
			local.FreeItem (gameObject);
		}
	}
		
}

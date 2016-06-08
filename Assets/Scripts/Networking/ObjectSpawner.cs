using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ObjectSpawner : NetworkBehaviour {

	public GameObject prefab;
	public GameObject canvas;

	public override void OnStartServer(){
		var go = (GameObject)Instantiate (prefab, transform.position + new Vector3 (2, 2, 0), Quaternion.identity);
		go.transform.parent = canvas.transform;
		NetworkServer.Spawn (go);
	} 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}

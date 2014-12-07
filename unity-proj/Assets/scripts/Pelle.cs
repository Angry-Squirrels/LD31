using UnityEngine;
using System.Collections;

public class Pelle : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider bunny){
		if (bunny.gameObject.layer == LayerMask.NameToLayer("bunny")){
			BunnyAI bunnyScript = bunny.GetComponent<BunnyAI>();
			bunnyScript.TakeDammage(5);
		}
	}
}

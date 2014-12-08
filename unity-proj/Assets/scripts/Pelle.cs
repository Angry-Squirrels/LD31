using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pelle : MonoBehaviour {

	public AudioSource source;
	public List<AudioClip> soundToPlay;

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

			source.clip = soundToPlay[Random.Range(0,2)];
			source.Play();
		}
	}

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pelle : MonoBehaviour {

	public AudioSource source;
	public List<AudioClip> soundToPlay;

	float lastTimePlayed = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		lastTimePlayed += Time.deltaTime;
	}

	void OnTriggerEnter(Collider bunny){
		if (bunny.gameObject.layer == LayerMask.NameToLayer("bunny")){
			BunnyAI bunnyScript = bunny.GetComponent<BunnyAI>();
			bunnyScript.TakeDammage(5);
			if(lastTimePlayed > 0.25)
			{
				source.clip = soundToPlay[Random.Range(0,2)];
				source.Play();
				lastTimePlayed = 0.0f;
			}
		}
	}

}

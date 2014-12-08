using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LapinAnimEvent : MonoBehaviour {

	public AudioSource audioSource;
	public AudioClip jumpSFX;
	public AudioClip deathSFX;
	public AudioClip eatSFX;
	public List<AudioClip> growlsSFX;

	public BunnyAI ai;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void OnStartJump(){
		audioSource.clip = jumpSFX;
		audioSource.Play();
		ai.InAir();
	}

	public void OnLand(){
		ai.LandJump();
	}

	public void OnTransformEnded(){
		ai.TransformEnded();
	}
}

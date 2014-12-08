using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LapinAnimEvent : MonoBehaviour {

	public AudioSource audioSource;
	public AudioSource splashSource;
	public AudioClip jumpSFX;
	public AudioClip deathSFX;
	public AudioClip eatSFX;
	public List<AudioClip> growlsSFX;

	public BunnyAI ai;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{

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
	
	public void playDeathSound()
	{
		audioSource.clip = deathSFX;
		audioSource.Play ();
		splashSource.Play ();
	}
	
	public void playEatSound()
	{
		audioSource.clip = eatSFX;
		audioSource.Play ();
	}
	
	public void playGrowlSound()
	{
		audioSource.clip = growlsSFX[Random.Range(0, 2)];
		audioSource.Play ();
	}
}

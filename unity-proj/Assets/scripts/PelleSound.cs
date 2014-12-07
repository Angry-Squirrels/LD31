using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PelleSound : MonoBehaviour {
	
	public AudioSource source;
	public List<AudioClip> clipToPlay;
	public Pelle pelle;
	
	// Use this for initialization
	void Start () {
		source.clip = clipToPlay[0];
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnAnimationHit(){
		source.Play();
	}
}

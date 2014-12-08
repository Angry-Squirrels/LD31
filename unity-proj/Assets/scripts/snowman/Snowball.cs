﻿using UnityEngine;
using System.Collections;

public class Snowball : MonoBehaviour {

	public float speed = 20;
	public float damage = 5;
	public float lifeSpan = 2;

	private Snowman parent;
	private bool isLaunched = false;
	private float lifeTime = 0;
	
	// Update is called once per frame
	void Update ()
	{
		if (isLaunched)
		{
			lifeTime += Time.deltaTime;
			transform.Translate(Vector3.forward * speed * Time.deltaTime);

			if (lifeTime > lifeSpan)
			{
				die ();
			}
		}
	}

	public void Launch(Snowman _parent)
	{
		parent = _parent;
		isLaunched = true;
		lifeTime = 0;
	}

	private void die()
	{
		isLaunched = false;
		gameObject.SetActive (false);
		parent.takeBackSnowball (this);
	}

	void OnTriggerEnter(Collider _other)
	{
		Debug.Log (_other.gameObject.name);
		if (_other.gameObject.layer == LayerMask.NameToLayer("bunny"))
		{
			Debug.Log ("rabbit touched !!");
			die ();
		}
		else if (_other.gameObject.layer == LayerMask.NameToLayer("world"))
		{
			Debug.Log ("ground touched !!");
			die ();
		}
	}
}
using UnityEngine;
using System.Collections;

public class Snowball : MonoBehaviour {

	public float speed = 20;
	public int damage = 5;
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
		if (_other.gameObject.layer == LayerMask.NameToLayer("bunny"))
		{
			BunnyAI bunnyAi = _other.gameObject.GetComponent<BunnyAI>();
			bunnyAi.TakeDammage(damage);
			die ();
		}
		else if (_other.gameObject.layer == LayerMask.NameToLayer("world"))
		{
			die ();
		}
	}
}

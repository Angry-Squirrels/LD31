using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Snowman : MonoBehaviour {

	private List<Transform> reachableRabbits;
	private Transform targetedRabbit;

	public Snowball snowball;
	private List<Snowball> snowballs;

	public float fireRate = 0.5f;
	private float fireTime = 0;
	public Transform launchPoint;

	// Use this for initialization
	void Start ()
	{
		reachableRabbits = new List<Transform> ();
		snowballs = new List<Snowball> ();
		instantiateSnowballs ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (reachableRabbits.Count > 0)
		{
			targetedRabbit = getClosestRabbit();
			if (targetedRabbit != null)
			{
				fire ();
			}
		}
		else
		{
			targetedRabbit = null;
		}

		if (targetedRabbit != null)
		{
			Vector3 target = targetedRabbit.position;
			target.y = transform.position.y;
			transform.LookAt (target);
		}
	}

	private void instantiateSnowballs()
	{
		for (int i = 0; i < 10; i++)
		{
			Snowball s = (Snowball)Instantiate(snowball);
			snowballs.Add(s);
			s.gameObject.SetActive(false);
		}
	}

	public void addRabbit(Transform _rabbitTrsf)
	{
		reachableRabbits.Add (_rabbitTrsf);
	}

	public void removeRabbit(Transform _rabbitTrsf)
	{
		reachableRabbits.Remove (_rabbitTrsf);
	}

	private Transform getClosestRabbit()
	{
		float minDistance = 10000000;
		Transform rabbit = null;

		foreach (Transform trsf in reachableRabbits)
		{
			float distance = Vector3.Distance(transform.position, trsf.position);
			if (distance < minDistance)
			{
				minDistance = distance;
				rabbit = trsf;
			}
		}

		return rabbit;
	}

	private void fire()
	{
		fireTime += Time.deltaTime;

		if (fireTime >= fireRate)
		{
			fireTime = 0;
			throwSnowball();
		}
	}

	private void throwSnowball()
	{
		Snowball snowball = snowballs [0];
		snowballs.Remove (snowball);

		snowball.transform.position = launchPoint.position;
		snowball.transform.LookAt (targetedRabbit);
		snowball.gameObject.SetActive (true);
		snowball.Launch (this);
	}

	public void takeBackSnowball(Snowball _snowball)
	{
		snowballs.Add (_snowball);
	}
}

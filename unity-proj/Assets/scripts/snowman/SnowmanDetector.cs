using UnityEngine;
using System.Collections;

public class SnowmanDetector : MonoBehaviour {

	public Snowman snowman;

	void OnTriggerEnter(Collider _other)
	{
		if (_other.gameObject.layer == LayerMask.NameToLayer("bunny"))
		{
			snowman.addRabbit(_other.transform);
		}
	}

	void OnTriggerExit(Collider _other)
	{
		if (_other.gameObject.layer == LayerMask.NameToLayer("bunny"))
		{
			snowman.removeRabbit(_other.transform);
		}
	}
}

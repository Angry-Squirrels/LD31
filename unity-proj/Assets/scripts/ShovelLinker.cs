using UnityEngine;
using System.Collections;

public class ShovelLinker : MonoBehaviour {

	public Transform shovelTrsf;
	
	// Update is called once per frame
	void Update () {
		transform.position = shovelTrsf.position;
		transform.rotation = shovelTrsf.rotation;
	}
}

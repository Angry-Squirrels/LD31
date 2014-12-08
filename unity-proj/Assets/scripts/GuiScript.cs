using UnityEngine;
using System.Collections;

public class GuiScript : MonoBehaviour {

	public MegaCarrot carrot;
	public RectTransform bar;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float t = (float)carrot.mLife / (float)carrot.maxLife;
		bar.transform.localScale = new Vector3(t,1.0f,1.0f);
	}
}

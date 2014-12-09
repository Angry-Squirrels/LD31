using UnityEngine;
using System.Collections;

public class VariableStore : MonoBehaviour {

	public static VariableStore instance;

	public GameController gameController;

	void Awake()
	{
		instance = this;
	}
}

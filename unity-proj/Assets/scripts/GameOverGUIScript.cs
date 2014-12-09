using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverGUIScript : MonoBehaviour {
	
	public Text nbDay;
	public Text nbCarrot;
	public Text nbRabbitKilled;

	public void updateText(int day, int carrot, int rabbit)
	{
		nbDay.text = "" + day;
		nbCarrot.text = "x" + carrot;
		nbRabbitKilled.text = "x" + rabbit;
	}
}

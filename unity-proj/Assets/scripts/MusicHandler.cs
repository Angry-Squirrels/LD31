using UnityEngine;
using System.Collections;

public class MusicHandler : MonoBehaviour {

	public Base_SoundManager soundManager;
	public DayNightCycleManager cycleManager;

	void Awake()
	{
		cycleManager.onNewCycle += newCycleHandler;
	}

	private void newCycleHandler(int _cycle)
	{
		soundManager.StopAllMusics ();
		print (_cycle);
		switch (_cycle)
		{
			case 0:
				soundManager.PlayMusic("dawn_theme");
				break;
			case 1:
				soundManager.PlayMusic("day_theme");
				break;
			case 2:
				soundManager.PlayMusic("twilight_theme");
			break;
			case 3:
				soundManager.PlayMusic("night_theme");
				break;
			case 4:
				soundManager.PlayMusic("main_theme");
				break;
		}
	}
}

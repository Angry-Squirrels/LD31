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
		switch (_cycle)
		{
			case 0:
				soundManager.PlayMusic("dawn_theme");
				soundManager.PlayMusic("cocorico");
				break;
			case 1:
				soundManager.PlayMusic("day_theme");
				soundManager.PlayMusic("birds");
				break;
			case 2:
				soundManager.PlayMusic("twilight_theme");
			break;
			case 3:
				soundManager.PlayMusic("night_theme");
				soundManager.PlayMusic("wolf_howl");
				break;
			case 4:
				soundManager.PlayMusic("main_theme");
				break;
		}
	}
}

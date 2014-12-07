using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Gestionnaire des sons.
/// </summary>
/// <exception cref='KeyNotFoundException'>
/// Est envoyé lors d'un appel à une fonction avec un nom de son ou de musique qui n'existe pas.
/// </exception>
/// <exception cref='UnityException'>
/// Est envoyé lors d'un mauvais appel de fonction.
/// </exception>
public class Base_SoundManager : MonoBehaviour
{
	[System.Serializable]
	/// <summary>
	/// Défini les propriétés nécessaires à la création d'un son.
	/// </summary>
	public class Sound
	{
		/// <summary>
		/// Son à jouer.
		/// </summary>
		public AudioClip audioClip;
		/// <summary>
		/// Objet qui joue le son.
		/// </summary>
		public GameObject whoIsPlayingIt;
		/// <summary>
		/// Nom du son : utilisé dans le Dictionnaire comme clef pour ce son.
		/// </summary>
		public string soundName;
		/// <summary>
		/// Volume du son : compris entre 0 et 1.
		/// </summary>
		public float volume = 1;
		/// <summary>
		/// Est-ce que le son boucle ?
		/// </summary>
		public bool playLoop = false;
		/// <summary>
		/// Est-ce une musique ou un son ?
		/// </summary>
		public bool isMusic = false;
		/// <summary>
		/// Côté duquel le son est joué : entre -1 (tout à gauche) et 1 (tout à droite).
		/// </summary>
		public float pan2D = 0;
		
		/// <summary>
		/// Est-ce un son 3D ?
		/// </summary>
		public bool is3D = false;
		
		
		// les propriétés en dessous ne sont nécessaires que si le son est 3D
		
		/// <summary>
		/// En dessous de cette distance, le son sera entendu à plein volume.
		/// </summary>
		public float minDistance;
		/// <summary>
		/// Au dessus de cette distance, le son sera inaudible.
		/// </summary>
		public float maxDistance;
		/// <summary>
		/// A quel point le son est en 3D : entre 0 (son 2D) et 1 (son full 3D).
		/// </summary>
		public float panLevel = 1;
		/// <summary>
		/// Comment le son se propage en 3D : entre 0 (le son ne sera entendu qu'à l'endroit exact où il est joué) et 360 (le son se propagera aux baffles proches).
		/// </summary>
		public float spreadLevel = 180;
	}
	
	/// <summary>
	/// Liste des sons à instancier.
	/// </summary>
	public List<Sound> listOfSounds;
	
	/// <summary>
	/// Sons instanciés.
	/// </summary>
	private Dictionary<string, AudioSource> instantiatedSounds;
	/// <summary>
	/// Musiques instanciées.
	/// </summary>
	private Dictionary<string, AudioSource> instantiatedMusics;
	/// <summary>
	/// Sons actuellement en lecture pouvant être pausés.
	/// </summary>
	private List<string> pausableSoundsIds;
	
	/// <summary>
	/// Flag permettant de déterminer si tous les sons sont actuellement pausés (utilisé par Un/PauseAllSound).
	/// </summary>
	private bool soundsArePaused = false;
	
	
	
	
	// --- FONCTIONS VITALES

	/// <summary>
	/// Appelé à l'initialisation de l'instance.
	/// </summary>
	void Awake ()
	{
		// Initialisation des sons
		InitSounds();
	}
	
	
	
	
	// --- FONCTIONS
	
	/// <summary>
	/// Initialise les sons.
	/// </summary>
	private void InitSounds()
	{
		instantiatedSounds = new Dictionary<string, AudioSource>();
		instantiatedMusics = new Dictionary<string, AudioSource>();
		pausableSoundsIds = new List<string>();
		
		foreach (Sound sound in listOfSounds)
		{
			// Défini depuis quel objet le son va se jouer
			// Si null, alors c'est le GST_Sound qui le jouera
			GameObject targetForSound;
			if (sound.whoIsPlayingIt != null)
			{
				targetForSound = sound.whoIsPlayingIt;
			}
			else
			{
				targetForSound = gameObject;
			}
			AudioSource asSource = targetForSound.AddComponent<AudioSource>();
			
			// Applique les propriétés
			asSource.clip = sound.audioClip;
			asSource.volume = sound.volume;
			asSource.loop = sound.playLoop;
			asSource.pan = sound.pan2D;
			asSource.playOnAwake = false;
			if (sound.is3D)
			{
				asSource.minDistance = sound.minDistance;
				asSource.maxDistance = sound.maxDistance;
				asSource.panLevel = sound.panLevel;
				asSource.spread = sound.spreadLevel;
			}
			
			// Ajoute le son à la bonne liste (son ou musique)
			if (sound.isMusic)
			{
				instantiatedMusics.Add(sound.soundName, asSource);
			}
			else
			{
				instantiatedSounds.Add(sound.soundName, asSource);
			}
		}
	}
	
	/// <summary>
	/// Joue un son depuis son nom.
	/// </summary>
	/// <param name='_soundName'>
	/// _Nom du son à jouer.
	/// </param>
	/// <exception cref='KeyNotFoundException'>
	/// Est envoyé si le nom du son n'est pas trouvé.
	/// </exception>
	public void PlaySound(string _soundName)
	{
		// Vérifie si le son existe
		if (instantiatedSounds.ContainsKey(_soundName))
		{
			// Récupère le son
			AudioSource sound = instantiatedSounds[_soundName];
			// Incrémente la liste des sons joués
			if (!pausableSoundsIds.Contains(_soundName))
			{
				pausableSoundsIds.Add (_soundName);
			}
			// Joue le son
			sound.Play();
		}
		else
		{
			throw new KeyNotFoundException("This sound doesn't exist : " + _soundName + ". Check its name, or may be try PlayMusic instead.");
		}
	}
	
	/// <summary>
	/// Pause un son depuis son nom.
	/// </summary>
	/// <param name='_soundName'>
	/// _Nom du son à pauser.
	/// </param>
	/// <exception cref='KeyNotFoundException'>
	/// Est envoyé si le nom du son n'est pas trouvé.
	/// </exception>
	public void PauseSound(string _soundName)
	{
		// Vérifie si le son est en train d'être joué
		if (pausableSoundsIds.Contains(_soundName))
		{
			// Récupère le son
			AudioSource sound = instantiatedSounds[_soundName];
			// Pause le son
			sound.Pause();
		}
		else
		{
			throw new KeyNotFoundException("This sound is not currently playing : " + _soundName + ".");
		}
	}
	
	/// <summary>
	/// Stoppe un son depuis son nom.
	/// </summary>
	/// <param name='_soundName'>
	/// _Nom du son à stopper.
	/// </param>
	/// <exception cref='KeyNotFoundException'>
	/// Est envoyé si le nom du son n'est pas trouvé.
	/// </exception>
	public void StopSound(string _soundName)
	{
		// Vérifie si le son est en train d'être jouée
		if (pausableSoundsIds.Contains(_soundName))
		{
			// Récupère le son
			AudioSource sound = instantiatedSounds[_soundName];
			// Retire le son de la liste des sons joués
			pausableSoundsIds.Remove(_soundName);
			// Stoppe le son
			sound.Stop();
		}
		else
		{
			throw new KeyNotFoundException("This sound is not currently playing : " + _soundName + ".");
		}
	}
	
	/// <summary>
	/// Joue une musique depuis son nom.
	/// </summary>
	/// <param name='_musicName'>
	/// _Nom de la musique à jouer
	/// </param>
	/// <exception cref='KeyNotFoundException'>
	/// Est envoyé si le nom de la musique n'est pas trouvé.
	/// </exception>
	public void PlayMusic(string _musicName)
	{
		// Vérifie si la musique existe
		if (instantiatedMusics.ContainsKey(_musicName))
		{
			// Récupère la musique
			AudioSource music = instantiatedMusics[_musicName];
			// Incrémente la liste des sons joués
			if (!pausableSoundsIds.Contains(_musicName))
			{
				pausableSoundsIds.Add (_musicName);
			}
			// Joue la musique
			music.Play();
		}
		else
		{
			throw new KeyNotFoundException("This music doesn't exist : " + _musicName + ". Check its name, or may be try PlaySound instead.");
		}
	}
	
	/// <summary>
	/// Pause un son depuis son nom.
	/// </summary>
	/// <param name='_musicName'>
	/// _Nom de la musique à pauser.
	/// </param>
	/// <exception cref='KeyNotFoundException'>
	/// Est envoyé si le nom de la musique n'est pas trouvé.
	/// </exception>
	public void PauseMusic(string _musicName)
	{
		// Vérifie si le son est en train d'être jouée
		if (pausableSoundsIds.Contains(_musicName))
		{
			// Récupère la musique
			AudioSource music = instantiatedMusics[_musicName];
			// Pause la musique
			music.Pause();
		}
		else
		{
			throw new KeyNotFoundException("This sound is not currently playing : " + _musicName + ".");
		}
	}
	
	/// <summary>
	/// Stoppe un son depuis son nom.
	/// </summary>
	/// <param name='_musicName'>
	/// _Nom de la musique à stopper.
	/// </param>
	/// <exception cref='KeyNotFoundException'>
	/// Est envoyé si le nom de la musique n'est pas trouvé.
	/// </exception>
	public void StopMusic(string _musicName)
	{
		// Vérifie si la musique est en train d'être jouée
		if (pausableSoundsIds.Contains(_musicName))
		{
			// Récupère la musique
			AudioSource sound = instantiatedMusics[_musicName];
			// Retire le son de la liste des sons joués
			pausableSoundsIds.Remove(_musicName);
			// Stoppe la musique
			sound.Stop();
		}
		else
		{
			throw new KeyNotFoundException("This music is not currently playing : " + _musicName + ".");
		}
	}
	
	/// <summary>
	/// Met en pause tous les sons et toutes les musiques.
	/// </summary>
	public void PauseAllSounds()
	{
		foreach (string soundId in pausableSoundsIds)
		{
			AudioSource sound;
			if (instantiatedMusics.ContainsKey(soundId))
			{
				sound = instantiatedMusics[soundId];
			}
			else
			{
				sound = instantiatedSounds[soundId];
			}
			// Pause le son
			sound.Pause();
		}
		
		// Passe le flag à true
		soundsArePaused = true;
	}
	
	/// <summary>
	/// Remet en lecture tous les sons et toutes les musiques.
	/// </summary>
	/// <exception cref='UnityException'>
	/// Est envoyé si la fonction PauseAllSounds n'a pas été appelée avant.
	/// </exception>
	public void TakeBackAllSounds()
	{
		// Vérifie le flag
		if (soundsArePaused)
		{
			foreach (string soundId in pausableSoundsIds)
			{
				AudioSource sound;
				if (instantiatedMusics.ContainsKey(soundId))
				{
					sound = instantiatedMusics[soundId];
				}
				else
				{
					sound = instantiatedSounds[soundId];
				}
				
				// Remet le son en lecture
				sound.Play();
			}
			
			// Passe le flag de nouveau à false
			soundsArePaused = false;
		}
		else
		{
			throw new UnityException("PauseAllSounds hasn't been called yet. TakeBackAllSounds cannot be invoked then.");
		}
	}
	
	/// <summary>
	/// Stoppe tous les sons et tous les musiques.
	/// </summary>
	public void StopAllSounds()
	{
		foreach (string soundId in pausableSoundsIds)
		{
			AudioSource sound = instantiatedSounds[soundId];
			pausableSoundsIds.Remove(soundId);
			sound.Stop();
		}
	}
	
	/// <summary>
	/// Stoppe toutes les musiques.
	/// </summary>
	public void StopAllMusics()
	{
		foreach (AudioSource music in instantiatedMusics.Values)
		{
			music.Stop();
		}
	}
	
	/// <summary>
	/// Met tous les sons en silence.
	/// </summary>
	public void MuteSounds()
	{
		foreach (AudioSource sound in instantiatedSounds.Values)
		{
			sound.mute = true;
		}
	}
	
	/// <summary>
	/// Retire le silence des sons.
	/// </summary>
	public void UnmuteSounds()
	{
		foreach (AudioSource sound in instantiatedSounds.Values)
		{
			sound.mute = false;
		}
	}
	
	/// <summary>
	/// Met toutes les musiques en silence.
	/// </summary>
	public void MuteMusic()
	{
		foreach (AudioSource music in instantiatedMusics.Values)
		{
			music.mute = true;
		}
	}
	
	/// <summary>
	/// Retire le silence des musique.
	/// </summary>
	public void UnmuteMusic()
	{
		foreach (AudioSource music in instantiatedMusics.Values)
		{
			music.mute = false;
		}
	}
	
	/// <summary>
	/// Met tous les sons et toutes les musiques en silence.
	/// </summary>
	public void MuteAllSounds()
	{
		MuteSounds();
		MuteMusic();
	}
	
	/// <summary>
	/// Retire le silence de tous les sons et de toutes les musiques.
	/// </summary>
	public void UnmuteAllSounds()
	{
		UnmuteSounds();
		UnmuteMusic();
	}
	
	/// <summary>
	/// Joue un son depuis son nom avec un fondu d'entrée.
	/// </summary>
	/// <param name='_soundName'>
	/// _Nom du son à jouer
	/// </param>
	/// <param name='_time'>
	/// _Durée du fondu
	/// </param>
	/// <exception cref='KeyNotFoundException'>
	/// Est envoyé si le nom du son n'est pas trouvé.
	/// </exception>
	public IEnumerator FadeInSound(string _soundName, float _time)
	{
		if (instantiatedSounds.ContainsKey(_soundName))
		{
			AudioSource sound = instantiatedSounds[_soundName];
			if (!pausableSoundsIds.Contains(_soundName))
			{
				pausableSoundsIds.Add (_soundName);
			}
			
			// Stocke le volume final
			float finalVolume = sound.volume;
			// Passe le volume du son à 0
			sound.volume = 0;
			
			// Joue le son avec le fondu d'entrée
			sound.Play();
			while (sound.volume < finalVolume)
			{
				yield return new WaitForEndOfFrame();
				sound.volume += finalVolume / (_time / Time.deltaTime);
			}
		}
		else
		{
			throw new KeyNotFoundException("This sound doesn't exist : " + _soundName + ". Check its name, or may be try FadeInMusic instead.");
		}
	}
	
	/// <summary>
	/// Joue une musique depuis son nom avec un fondu d'entrée.
	/// </summary>
	/// <param name='_musicName'>
	///  Nom de la musique.
	/// </param>
	/// <param name='_time'>
	/// _Durée du fondu.
	/// </param>
	/// <exception cref='KeyNotFoundException'>
	/// Est envoyé si le nom de la musique n'est pas trouvé.
	/// </exception>
	public IEnumerator FadeInMusic(string _musicName, float _time)
	{
		if (instantiatedMusics.ContainsKey(_musicName))
		{
			AudioSource music = instantiatedMusics[_musicName];
			if (!pausableSoundsIds.Contains(_musicName))
			{
				pausableSoundsIds.Add (_musicName);
			}
			
			// Stocke le volume final
			float finalVolume = music.volume;
			// Passe le volume de la musique à 0
			music.volume = 0;
			
			// Joue la musique avec le fondu d'entrée
			music.Play();
			while (music.volume < finalVolume)
			{
				yield return new WaitForEndOfFrame();
				music.volume += finalVolume / (_time / Time.deltaTime);
			}
		}
		else
		{
			throw new KeyNotFoundException("This music doesn't exist : " + _musicName + ". Check its name, or may be try FadeInSound instead.");
		}
	}
	
	/// <summary>
	/// Stoppe un son ou une musique depuis son nom avec un fondu de sortie.
	/// </summary>
	/// <param name='_soundName'>
	/// _Nom du son ou de la musique.
	/// </param>
	/// <param name='_time'>
	/// _Durée du fondu.
	/// </param>
	/// <exception cref='KeyNotFoundException'>
	/// Est envoyé si le nom du son ou de la musique n'est pas trouvé.
	/// </exception>
	public IEnumerator FadeOutSound(string _soundName, float _time)
	{
		if (pausableSoundsIds.Contains(_soundName))
		{
			AudioSource sound;
			if (instantiatedSounds.ContainsKey(_soundName))
			{
				sound = instantiatedSounds[_soundName];
			}
			else
			{
				sound = instantiatedMusics[_soundName];
			}
			pausableSoundsIds.Remove(_soundName);
			
			// Stocke le volume d'origine
			float originalVolume = sound.volume;
			
			// Stoppe le son avec le fondu de sortie
			while (sound.volume > 0)
			{
				yield return new WaitForEndOfFrame();
				sound.volume -= originalVolume / (_time / Time.deltaTime);
			}
			sound.Stop();
			// Une fois le son stoppé, on lui redonne son volume d'origine pour pouvoir le jouer de nouveau
			sound.volume = originalVolume;
		}
		else
		{
			throw new KeyNotFoundException("This sound is not currently playing : " + _soundName + ".");
		}
	}
	
	
	
	
	// FONCTIONS POUR ALTERER UN SON
	
	/// <summary>
	/// Change le pitch d'un son ou d'une musique.
	/// </summary>
	/// <param name='_soundName'>
	/// _Nom du son ou de la musique.
	/// </param>
	/// <param name='_newPitch'>
	/// _Nouveau pitch.
	/// </param>
	/// <exception cref='KeyNotFoundException'>
	/// Est envoyé si le nom du son ou de la musique n'est pas trouvé.
	/// </exception>
	public void ChangeSoundPitch(string _soundName, float _newPitch)
	{
		if (instantiatedSounds.ContainsKey(_soundName))
		{
			AudioSource sound = instantiatedSounds[_soundName];
			sound.pitch = _newPitch;
		}
		else if (instantiatedMusics.ContainsKey(_soundName))
		{
			AudioSource music = instantiatedMusics[_soundName];
			music.pitch = _newPitch;
		}
		else
		{
			throw new KeyNotFoundException("This sound doesn't exist : " + _soundName + ".");
		}
	}
	
	/// <summary>
	/// Rétabli le pitch à 1 d'un son ou d'une musique.
	/// </summary>
	/// <param name='_soundName'>
	/// _Nom du son ou de la musique.
	/// </param>
	/// <exception cref='KeyNotFoundException'>
	/// Est envoyé si le nom du son ou de la musique n'est pas trouvé.
	/// </exception>
	public void ResetSoundPitch(string _soundName)
	{
		if (instantiatedSounds.ContainsKey(_soundName))
		{
			AudioSource sound = instantiatedSounds[_soundName];
			sound.pitch = 1;
		}
		else if (instantiatedMusics.ContainsKey(_soundName))
		{
			AudioSource music = instantiatedMusics[_soundName];
			music.pitch = 1;
		}
		else
		{
			throw new KeyNotFoundException("This sound doesn't exist : " + _soundName + ".");
		}
	}
}

//Description : ManageAudio.cs : Some function to manipulate audio
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManageAudio : MonoBehaviour {
	private AudioSource 		_audio;									// Access Audiosource on this gameObject
	private float 				AudioRefVolume = 0 ;					// Save volume when game start
	public 	List<AudioClip> 	list_Sounds = new List<AudioClip>();
	public 	List<AudioSource> 	list_AudioSource = new List<AudioSource>();

	void Start () {														// --> Init
		_audio = GetComponent<AudioSource>();								// Access Audiosource component 
		if(_audio)AudioRefVolume = _audio.volume;							// Save volume if the gameObject has a Audiosource component
	}
	
	public void audioFadeIn () {										// --> Call to fade In Sound
		StopAllCoroutines();
		StartCoroutine(I_audioFadeIn());
	}

	IEnumerator I_audioFadeOut  (){
		float t = 0;
		while(_audio.volume > .05F){									// While volume = 0
			t += Time.deltaTime*.01F;
			float tmpVolume = Mathf.LerpUnclamped(_audio.volume, 0, t);
			_audio.volume = tmpVolume;
			yield return null;
		}
		_audio.volume = 0;
		_audio.Stop();
	}

	public void audioFadeOut () {										// --> Call to fade Out Sound
		StopAllCoroutines();
		StartCoroutine(I_audioFadeOut());
	}

	IEnumerator  I_audioFadeIn(){
		if(!_audio.isPlaying)_audio.Play();
		float t = 0;
		while(_audio.volume < AudioRefVolume-.05F){						// While volume = 1
			t += Time.deltaTime*.1F;
			float tmpVolume = Mathf.LerpUnclamped(_audio.volume, 1, t);
			_audio.volume = tmpVolume;
			yield return null;
		}
		_audio.volume = AudioRefVolume;
	}

	public void PlayASound(int value){							// --> This function is used with UI_Button prefab that represent button to load new LCD Game On G_Menu GameObject
		if(list_Sounds[value]){											// Play a sound from list_Sounds
			_audio.PlayOneShot(list_Sounds[value]);
		}
	}

	public void PlayASoundFromAudioSource(int value){			// --> This function is used other buttons. It prevent bug when a sound is played and the button is deactivated at the same time
		if(list_AudioSource[value]){									// Play a sound from list_AudioSource
			list_AudioSource[value].Play();
		}
	}

}

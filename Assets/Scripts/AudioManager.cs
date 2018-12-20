using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	public AudioClip[] audioClips;

	public static AudioManager instance;

	AudioSource m_audio;
	AudioSource m_audioLoop;

	IEnumerator Play(){
		m_audio.Play ();
		yield return new WaitWhile (()=> m_audio.isPlaying);
	}

	public void PlaySound(string audioName) {
		foreach (AudioClip audioClip in audioClips) {
			if (audioClip.name == audioName) {
				m_audio.clip = audioClip;
			}
		}
		m_audio.Play ();
	}

	public void PlaySoundMulti(List<string> audioName) {
		while (audioName.Count > 0) {
			string name = audioName [0];
			foreach (AudioClip audioClip in audioClips) {
				if (audioClip.name == name) {
					m_audio.clip = audioClip;
				}
			}
			audioName.RemoveAt (0);
			StartCoroutine (Play ());
		}
	}
	public void PlaySoundLoop(string audioName) {
		foreach (AudioClip audioClip in audioClips) {
			if (audioClip.name == audioName) {
				m_audioLoop.clip = audioClip;
			}
		}
		m_audioLoop.loop = true;
		m_audioLoop.Play ();
	}

	void Awake() {
		instance = this;
	}

	// Use this for initialization
	void Start () {
		m_audio = GetComponent<AudioSource>();
		m_audioLoop = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	}
}
using UnityEngine;
using System.Collections;

public class AudioManager : Singleton<AudioManager> {
	
	public float volume = 1;
	
	public int audioSourceIndex = 0;
	public AudioSource[] audioSources;
	
	[Header("Menu")]
	public AudioClip select;
	public AudioClip confirm;
	public AudioClip cancel;
	public AudioClip equip;
	
	[Header("General")]
	public AudioClip hit;
	public AudioClip kill;
	public AudioClip beat;
	
	[Header("Instra")]
	public AudioClip comet;
	public AudioClip crush;
	public AudioClip dive;
	public AudioClip counterpoint;
	
	// Use this for initialization
	void Awake () {
		for(int a = 0; a < audioSources.Length; a++) {
			audioSources[a].volume = volume;
		}
	}
	
	public static void PlayAudio(AudioClip audioClip) {
		if(audioClip == null) return;
		instance.audioSources[instance.audioSourceIndex].PlayOneShot(audioClip);
		instance.audioSourceIndex++;
		if(instance.audioSourceIndex == instance.audioSources.Length) instance.audioSourceIndex = 0;
	}
	
	public IEnumerator PlayAudioRepeated(AudioClip audioClip, int repeats, float delay) {
		int r = 0;
		
		while(r < repeats) {
			PlayAudio(audioClip);
			r++;
			yield return new WaitForUnscaledSeconds(delay);
		}
	}	
	
	public IEnumerator PlayAudioDelayed(AudioClip audioClip, float delay) {
		yield return new WaitForUnscaledSeconds(delay);
		
		PlayAudio(audioClip);
	}																																																																																															
}
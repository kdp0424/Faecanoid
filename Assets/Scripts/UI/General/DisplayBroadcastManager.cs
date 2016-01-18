using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayBroadcastManager : Singleton<DisplayBroadcastManager> {
	
	public Text displayText;
	public CanvasGroup displayTextCanvasGroup;
	
	public float displayTime;
	public float fadeTime;
	private IEnumerator fadeAlpha;
	
	float duration;
	
	void Awake () {
		displayText.text = "";	
		displayTextCanvasGroup.Hide();
	}
	
	public static void DisplayMessage (string message, float _duration = 0f) {
		instance.StopAllCoroutines();
		instance.StartCoroutine(instance.DisplayMessageSequence(message, _duration));
	}
	
	
	IEnumerator DisplayMessageSequence (string message, float _duration = 0f) {
		//Stop any running coroutine
//		if (fadeAlpha != null) {
//			StopCoroutine (fadeAlpha);
//		}
		//Fade the existing message out
		fadeAlpha = FadeOutSequence();
		yield return StartCoroutine (fadeAlpha);
		//Assign the new message and duration
		displayText.text = message;
		duration = _duration;
		//Fade the new message in
		if(message != "") {
			fadeAlpha = FadeInSequence();
			yield return StartCoroutine (fadeAlpha);
			//Wait until the duration has completed
			if(duration != 0f) {
				yield return new WaitForUnscaledSeconds (duration);
			} else
			{
			    yield return new WaitForUnscaledSeconds (displayTime);
			}
			//Fade the message out
			fadeAlpha = FadeOutSequence();
			yield return StartCoroutine (fadeAlpha);
		}
	}
	
//	public static void FadeIn() {
//		if (instance.fadeAlpha != null) {
//			instance.StopCoroutine (instance.fadeAlpha);
//		}
//		instance.fadeAlpha = instance.FadeOutSequence ();
//		instance.StartCoroutine (instance.fadeAlpha);		
//	}	
	
	IEnumerator FadeInSequence() {
		while (displayTextCanvasGroup.alpha < 1) {
			displayTextCanvasGroup.alpha += Time.unscaledDeltaTime / fadeTime;
			yield return null;
		}
		
	}	
	
//	public static void FadeOut() {
//		if (instance.fadeAlpha != null) {
//			instance.StopCoroutine (instance.fadeAlpha);
//		}
//		instance.fadeAlpha = instance.FadeOutSequence ();
//		instance.StartCoroutine (instance.fadeAlpha);		
//	}

	IEnumerator FadeOutSequence() {
		while (displayTextCanvasGroup.alpha > 0) {
			displayTextCanvasGroup.alpha -= Time.unscaledDeltaTime / fadeTime;
			yield return null;
		}

	}
	
}
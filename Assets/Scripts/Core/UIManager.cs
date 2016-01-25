using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager> {

	public Camera uiCamera;
	public Canvas uiCanvas;

	public Slider timeSlider;

	public Text player1ScoreText;
	public Text player2ScoreText;

	public CanvasGroup menuGroup;

	// Use this for initialization
	void Start () {
	
	}
	
	void UpdateUI() {

	}

	IEnumerator MatchTimerUISequence() {

		while(GameController.mode == GameController.Mode.Action) {
			timeSlider.MatchValues(GameController.instance.matchTimer);
			yield return null;
		}
	}

	void ShowMenu() {
		menuGroup.SetInteractive(true, 0.25f);
	}

	void HideMenu() {
		menuGroup.SetInteractive(false, 0.25f);
	}

	void OnEnable() {
		GameController.OnModeMainMenu += ShowMenu;
		GameController.OnModeMainMenuExit += HideMenu;
	}

	void OnDisable() {

	}
}

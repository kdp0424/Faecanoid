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


	CoroutineManager.Item matchTimerUISequence = new CoroutineManager.Item();
	// Use this for initialization
	void Start () {
		
	}

	void Initialize() {
		Debug.Log(("UI Manager initialized").Colored(Colors.aqua));

		for(int n = 0; n < Player.players.Length; n++) {
			//Debug.Log("UI Manager is trying to assign to Player number " + n + ".");
			if(Player.players[n] == null) continue;
			Player.players[n].score.OnValueChanged += UpdateUI;
		}

		matchTimerUISequence.sequence = MatchTimerUISequence();

		UpdateUI();

		//Debug.Log("Turned on match timer slider");
	}

	void Uninitialize() {


		matchTimerUISequence.sequence = null;

		//Debug.Log("Turned off match timer slider");
	}

	void UpdateUI() {
		player1ScoreText.text = "" + Player.players[0].score.value;
		player2ScoreText.text = "" + Player.players[1].score.value;
	}

	IEnumerator MatchTimerUISequence() {

		while(true) {
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

		GameController.OnModeGameStart += Initialize;
		GameController.OnModeGameOver += Uninitialize;
	}

	void OnDisable() {

	}
}

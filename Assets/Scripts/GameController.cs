using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LineRenderer))]
/// Manages game state and player input.
public class GameController : Singleton<GameController> {

	public enum State { MainMenu, GameStart, Action, GameOver };

    public static StateManager<State> state = StateManager<State>.CreateNew(); 

	public static bool tutorialEnabled = false;

    static bool _controlsEnabled = false;

    public static bool controlsEnabled
    {
        get
        {
            return _controlsEnabled;
        }
        set
        {
            _controlsEnabled = value;

        }
    }

	public delegate void EventHandler();

    public static event EventHandler OnPause;
    public static event EventHandler OnUnPause;

    public MinMaxEventFloat matchTimer = new MinMaxEventFloat(0f, 60f, 60f);

    CoroutineManager.Item matchTimerSequence = new CoroutineManager.Item();

    [Space(10)]
	public bool paused = false;
	public bool pausePlayer = false; //!< Whether a pause was initiated with the player

	public static bool isQuitting = false;
	
	void Awake() {
		
		Initialize();
	}
	
	/// <summary>
	/// Call any necessary Initialize functions in other classes. The order is important.
	/// </summary>
	static void Initialize()
	{
        state.values[State.MainMenu].OnEnter += instance.EnterModeMainMenu;
        state.values[State.GameStart].OnEnter += instance.EnterModeGameStart;
	    state.values[State.Action].OnEnter += instance.EnterModeAction;
        state.values[State.GameOver].OnEnter += instance.EnterModeGameOver;

        //state.values[State.MainMenu].OnEnter += instance.ExitModeMainMenu;
        //state.values[State.GameStart].OnEnter += instance.ExitModeGameStart;
        state.values[State.Action].OnEnter += instance.ExitModeAction;
        //state.values[State.GameOver].OnEnter += instance.ExitModeGameOver;
    }
	
	// Use this for initialization
	void Start () {

	}
	
	public void BeginGame() {
		state.value = State.GameStart;

		matchTimer.OnValueMin += EndGame;
	}

	public void EndGame() {
        state.value = State.GameOver;
		matchTimer.OnValueMin -= EndGame;
	}
	
	// Update is called once per frame
	void Update () {
		InputManager();
	}

	IEnumerator MatchTimerSequence() {
		while(matchTimer.value > 0f) {
			matchTimer.value -= Time.deltaTime;
			yield return null;
		}
	}


	public void EnterModeMainMenu() {
		//StartCoroutine(RestartProcess());
	}
	public IEnumerator RestartProcess() {

        yield return null;
        state.value = State.GameStart;
	}
	public void EnterModeGameStart() {

		StartCoroutine(EnterModeActionProcess());
		matchTimer.SetToMax();
		AudioManager.PlayAudio(AudioManager.instance.confirm);
	}
	
	public void EnterModeAction() {
		matchTimerSequence.sequence = MatchTimerSequence();
	}
	
	IEnumerator EnterModeActionProcess() {
		

		yield return new WaitForSeconds(0.5f);

        state.value = State.Action;

	}
	
	public void ExitModeAction() {
		matchTimerSequence.sequence = null;
	}

	public void EnterModeGameOver() {
        state.value = State.MainMenu;
	}
	
	public void InputManager() {
//		if(Input.GetButtonUp("Select"))
//		{
//
//		} 
//
//		if (Input.GetButtonUp("Confirm"))
//		{
//
//		}

        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

		if(Input.GetKey(KeyCode.LeftControl) && Input.GetButtonUp("DeveloperMode")) {
			if(DataCore.developerMode == false) { 
				DataCore.developerMode = true;
//				if(!UIManager.instance.UI_Canvas.gameObject.activeInHierarchy) {
//					UIManager.instance.UI_Canvas.gameObject.SetActive(true);
//				}
				DeveloperConsole.instance.inputField.ActivateInputField();
				DeveloperConsole.instance.inputField.Select();
				
			} else {
				DataCore.developerMode = false;
				DeveloperConsole.instance.inputField.DeactivateInputField();
			}
		} 

		if (Input.GetKey("escape") && (Input.GetKey("left ctrl") )) {
			Application.Quit();
		}	
		
		if(!DataCore.developerMode) {

            if (Input.GetButtonDown("Pause"))
            {
                TogglePause();
            }

            if (Input.GetButtonDown("Cancel"))
            {

            }

            if (Input.GetButtonDown("ScreenShot"))
            {
                //StartCoroutine(saveScreenshot());
            }


		}
	}		
				
	public void TogglePause() {
		if(paused) {
			pausePlayer = false;
			UnPause();            
        } else {
			pausePlayer = true;
			Pause();		    
		}	

		AudioManager.PlayAudio (AudioManager.instance.pause);
	}		
	
	public void Pause() {
		paused = true;
		Time.timeScale = 0.0f;
        if (OnPause != null) OnPause();
    }
	
	public void UnPause() {
		paused = false;
		Time.timeScale = 1.0f;
        if (OnUnPause != null) OnUnPause();
    }
	

	
//	private IEnumerator saveScreenshot ()
//	{
//		GameController.instance.Pause();
//
//		return new System.NotImplementedException ();
//
//		GameController.instance.UnPause();
//	}
	
}
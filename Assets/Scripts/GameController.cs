using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LineRenderer))]
/// Manages game state and player input.
public class GameController : Singleton<GameController> {

	public enum Mode { MainMenu, GameStart, Action, GameOver };
	private static Mode _mode = Mode.MainMenu;

	public static Mode mode {
		get {
			return _mode;
		}
		set {
            if (mode == value) return;
            SetMode(value);
			instance.editorVisibleMode = _mode;
		}
	}

	public Mode editorVisibleMode;

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
    public static event EventHandler OnModeGameStart;
	public static event EventHandler OnModeGameStartExit;
	
	public static event EventHandler OnModeMainMenu;
	public static event EventHandler OnModeMainMenuExit;
	
	public static event EventHandler OnModeAction;
	public static event EventHandler OnModeActionExit;
	
	public static event EventHandler OnModeGameOver;
	public static event EventHandler OnModeGameOverExit;


    public static event EventHandler OnPause;
    public static event EventHandler OnUnPause;

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
	static void Initialize() {

	}
	
	// Use this for initialization
	void Start () {

	}
	
	public void BeginGame() {

	}
	
	// Update is called once per frame
	void Update () {
		InputManager();
	}
	
	/// <summary>
	/// Sets the Game's mode to a new mode, and calls relevant mode entry and exit events.
	/// </summary>
	/// <param name="newMode">New mode.</param>
	private static void SetMode(Mode newMode) {
		if(mode == newMode) return;
		Debug.Log(("Changing GameController.mode to: " + newMode).Colored(Colors.orange));
		

        //Run the appropriate mode exit functions
        switch (mode) {
			case Mode.MainMenu:
				if(OnModeMainMenuExit != null) OnModeMainMenuExit();
				break;
			case Mode.GameStart:
				if(OnModeGameStartExit != null) OnModeGameStartExit();
				break;				
			case Mode.Action:
				if(OnModeActionExit != null) OnModeActionExit();
				instance.ExitModeAction();
				break;
			case Mode.GameOver:
				if(OnModeGameOverExit != null) OnModeGameOverExit();
				break;								
		}
		//Sets the private variable to the new mode
		_mode = newMode;
		
		//Run the appropriate mode enter functions
		switch(mode) {
			case Mode.MainMenu:
				if(OnModeMainMenu != null) OnModeMainMenu();
				instance.EnterModeMainMenu();
				break;
			case Mode.GameStart:
				if(OnModeGameStart != null) OnModeGameStart();
				instance.EnterModeGameStart();
				break;				
			case Mode.Action:
				if(OnModeAction != null) OnModeAction();
				instance.EnterModeAction();
				break;
			case Mode.GameOver:
				if(OnModeGameOver != null) OnModeGameOver();
				instance.EnterModeGameOver();
				break;								
		}		
	
	}
	
	public void EnterModeMainMenu() {
		StartCoroutine(RestartProcess());
	}
	public IEnumerator RestartProcess() {

        yield return null;
		mode = Mode.GameStart;
	}
	public void EnterModeGameStart() {

		mode = Mode.Action;
	}
	
	public void EnterModeAction() {

	}
	
	IEnumerator EnterModeActionProcess() {
		

		yield return new WaitForSeconds(0.5f);

	}
	
	public void ExitModeAction() {

	}

	public void EnterModeGameOver() {

	}
	
	public void InputManager() {
		if(Input.GetButtonUp("Select"))
		{

		} 

		if (Input.GetButtonUp("Confirm"))
		{

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
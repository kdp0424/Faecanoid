using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DeveloperConsole : MonoBehaviour {

	private static DeveloperConsole _instance;
	public static DeveloperConsole instance
	{
		get 
		{
			if(_instance == null) 
			{
				_instance = GameObject.FindObjectOfType<DeveloperConsole>();
				//Enables object persistence between different scenes
				//DontDestroyOnLoad(_instance.gameObject);	
			}
			return _instance;
		}
	}	

	public InputField inputField;
	CanvasGroup canvasGroup;
	
	public List<string> previousCommands = new List<string>();
	
	public MinMaxInt index = new MinMaxInt(0, 0, 0);
	
	bool selected = false;
	
	// Use this for initialization
	void Start () {
		canvasGroup = GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	void Update () {
		if(DataCore.developerMode == false) {
			canvasGroup.Hide();
		} else {
			canvasGroup.Show();
		}
		
		if(selected) {
			if(Input.GetKeyDown(KeyCode.UpArrow))
			{
			    if (inputField.text == "" || inputField.text == "Enter developer command...")
			    {
                    index.value++;
                    inputField.text = previousCommands[index.value];
                    Debug.Log("Now viewing developer console index " + index.value);

                    return;
			    }
			    index.value--;
				inputField.text = previousCommands[index.value];
				Debug.Log("Now viewing developer console index " + index.value);
			} else if(Input.GetKeyDown(KeyCode.DownArrow))
			{
			    index.value++;
				inputField.text = previousCommands[index.value];
				Debug.Log("Now viewing developer console index " + index.value);
			}
		}
		
	}
	
	public void EnterCommand() {
		if(inputField.text == "" || inputField.text == "g") {
			inputField.text = "";
			return;
		}
		
		DeveloperConsoleCommands.Begin(inputField.text);
	
		previousCommands.Add(inputField.text);
	    if(previousCommands.Count > 1) index.max++;
        index.SetToMax();
        
		//Debug.Log("Developer console index is now " + index);
		inputField.text = "";
		inputField.DeactivateInputField();
		
		DataCore.developerMode = false;
	}

}

using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public MinMaxEventInt score = new MinMaxEventInt(0, 1000, 0);

	public static Player[] players = new Player[4];

	Vector3 startPosition;

	public MinMaxFloat speed = new MinMaxFloat(15f, 45f, 30f);
	string horizontalAxis = "Horizontal";
	string verticalAxis = "Vertical";

	public int playerNumber = 0;

	Rigidbody2D rigidbody;

	CoroutineManager.Item controlSequence = new CoroutineManager.Item();

	public GameController.EventHandler OnBlockDestroyed;

	// Use this for initialization
	void Awake () {
		AssignPlayerNumber();

		startPosition = transform.position;

		rigidbody = GetComponent<Rigidbody2D>();

		GameController.OnModeGameOverExit += Reset;
        GameController.OnModeMainMenuExit += Initialize;

	}
	
	void Initialize() {
		controlSequence.sequence = ControlSequence();

		for(int n = 0; n < 4; n++) {
			if(players[n] == null || players[n] == this) {
				continue;
			} else {
				players[n].OnBlockDestroyed -= AddScore;
			}
		}

		for(int n = 0; n < 4; n++) {
			if(players[n] == null || players[n] == this) {
				continue;
			} else {
				players[n].OnBlockDestroyed += AddScore;
			}
		}

		if(playerNumber > 0) horizontalAxis = "Horizontal" + (playerNumber + 1);
		if(playerNumber > 0) verticalAxis = "Vertical" + (playerNumber + 1);
	}


	void AssignPlayerNumber() {
		for(int n = 0; n < 4; n++) {
			if(players[n] == null) {
				players[n] = this;

				playerNumber = n;

				Debug.Log("Player number " + n + " has been assigned.");
				break;
			} 
		}
	}

	IEnumerator ControlSequence () {
		while(true) {
			float h = Input.GetAxis(horizontalAxis);
        	float v = Input.GetAxis(verticalAxis);
                
        	rigidbody.velocity = new Vector2(h, v).normalized * speed.value;

        	yield return null;
        }
    }

    public void RaiseBlockDestroyed() {
    	if(OnBlockDestroyed != null) OnBlockDestroyed();
    }

	void Reset() {
		controlSequence.sequence = null;
		transform.position = startPosition;
		rigidbody.velocity = Vector2.zero;
		score.SetToMin();
		Debug.Log("Player " + playerNumber + " score is: " + score.value);
	}


	public void AddScore() {
		score.value++;
	}
}

using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

	public int playerOwnerNumber;

	void OnTriggerEnter2D(Collider2D other)
    {
        // Destroy the whole block

        if(other.gameObject.name == "Ball") {
        
        	GameController.mode = GameController.Mode.GameOver;
        }
    }


}

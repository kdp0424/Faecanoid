using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

	public int playerOwnerNumber;

	void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        // Destroy the whole block
        
        if(collisionInfo.gameObject.name == "Ball") {
        
        	GameController.mode = GameController.Mode.GameOver;
        }
    }


}

using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        // Destroy the whole block
        Destroy(gameObject);
    }
}

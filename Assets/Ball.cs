using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
    public float speed = 30;
	Rigidbody2D rigidbody;

	// Use this for initialization
	void Start () {
        // Initial Velocity
		rigidbody = GetComponent<Rigidbody2D> (); 
        rigidbody.velocity = Vector2.right * speed;
	}

    float hitFactor(Vector2 ballPos, Vector2 racketPos,
                float racketHeight)
    {
        // ascii art:
        // ||  1 <- at the top of the racket
        // ||
        // ||  0 <- at the middle of the racket
        // ||
        // || -1 <- at the bottom of the racket
        return (ballPos.y - racketPos.y) / racketHeight;
    }

    void OnCollisionEnter2D(Collision2D col)  {
		
		//'Bounce' off surface
		foreach( ContactPoint2D contact in col.contacts ) //Find collision point
		{
			//Find the BOUNCE of the object
			rigidbody.velocity = 2 * ( Vector2.Dot( rigidbody.velocity, contact.normal ) ) * contact.normal - rigidbody.velocity; //Following formula  v' = 2 * (v . n) * n - v
			
			rigidbody.velocity *= -1; //Had to multiply everything by -1. Don't know why, but it was all backwards.
		}

        // Hit the Left Racket?
		if (col.gameObject.name == "RacketLeft" || col.gameObject.name == "RacketRight") {
			// Calculate hit Factor
			float y = hitFactor (transform.position,
                                col.transform.position,
                                col.collider.bounds.size.y);
			// Calculate direction, make Length=1 via .normalized
			Vector2 dir = new Vector2 (rigidbody.velocity.normalized.x, y).normalized;

			// Set Velocity with dir * speed
			rigidbody.velocity = dir * speed;

			AudioManager.PlayAudio (AudioManager.instance.player);
		} else {
			AudioManager.PlayAudio(AudioManager.instance.wall);
		}
    }
 }

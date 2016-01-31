using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
    public MinMaxFloat speed = new MinMaxFloat(15f, 100f, 30f);
	Rigidbody2D rigidbody;

	Vector3 startPos;

	// Use this for initialization
	void Awake () {
        // Initial Velocity
		rigidbody = GetComponent<Rigidbody2D> (); 
        startPos = transform.position;

        GameController.OnModeGameOverExit += Reset;
        GameController.OnModeAction += Initialize;
	}

	void Initialize() {
		bool right = Random.value < 0.5f;

		rigidbody.velocity = (right == true ? Vector2.right : Vector2.left) * speed.value;
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
		if (col.gameObject.name == "PlayerCharacter") {
			// Calculate hit Factor
			float y = hitFactor (transform.position,
                                col.transform.position,
                                col.collider.bounds.size.y);
			// Calculate direction, make Length=1 via .normalized
			Vector2 dir = new Vector2 (rigidbody.velocity.normalized.x, y).normalized;

            // Set Velocity with dir * speed
            Debug.Log(dir * speed.value);
			rigidbody.velocity = dir * speed.value;

			AudioManager.PlayAudio (AudioManager.instance.player, 0.5f + Random.value);
		} else {
			AudioManager.PlayAudio(AudioManager.instance.wall, 0.5f + Random.value);
		}

		speed.value = speed.value * 1.01f;
        ("collided with" + col.gameObject.name).DebugLogJustin();
        //Debug.Log(rigidbody.velocity.magnitude);
    }
    
    void Reset() {
    	transform.position = startPos;
    	rigidbody.velocity = Vector2.zero;
    	speed.value = 20f;
    }
 }

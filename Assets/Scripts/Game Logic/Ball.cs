using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
    public MinMaxFloat speed = new MinMaxFloat(15f, 100f, 30f);
	Rigidbody2D rigidbody;

	Vector3 startPos;

    CoroutineManager.Item escapeDetectionRoutine = new CoroutineManager.Item();

	// Use this for initialization
	void Awake () {
        // Initial Velocity
		rigidbody = GetComponent<Rigidbody2D> (); 
        startPos = transform.position;

        GameController.state.values[GameController.State.GameOver].OnExit += Reset;
        GameController.state.values[GameController.State.Action].OnEnter += Initialize;
	}

	void Initialize() {


        string[] randomPhrases = new string[]
        {
            "shmex it",
            "hai",
            "hey friends",
            "you're all so attractive",
            "SHMEX",
            "shmeeeeeexxxxx",
            "this game is called Shmexball? Weird.",
            "Get Ready",
            "you've made us all so happy",
            "you are enjoying this game",
            "do you know something about algebra?",
            "i hope you slept well!!",
            "my brother loves this game",
            "oh, let's get married!!",
            "i love you",
            "ni xiang buxiang bofang SHMEX?",
            "GET FUELED.",
            "welcome",
            "this is an innovative experience"
        };

        DisplayBroadcastManager.DisplayMessage(randomPhrases.PickRandom(), 3f);

        //GetComponent<BoxCollider2D>().enabled = false;

        StartCoroutine(InitializeRoutine());
    }

    IEnumerator InitializeRoutine()
    {


        yield return new WaitForSeconds(2f);

        bool right = Random.value < 0.5f;

        rigidbody.velocity = (right == true ? Vector2.right : Vector2.left) * speed.value;
        escapeDetectionRoutine.sequence = EscapeDetectionRoutine();

        //GetComponent<BoxCollider2D>().enabled = true;
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
		if (col.gameObject.layer == LayerMask.NameToLayer("Player")) {
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
        escapeDetectionRoutine.sequence = EscapeDetectionRoutine();
    }
    
    void Reset() {
        StartCoroutine(ResetRoutine());
    }

    public IEnumerator ResetRoutine()
    {

        yield return new WaitForSeconds(1f);

        transform.position = startPos;
    	rigidbody.velocity = Vector2.zero;
    	speed.value = 20f;

    }

    public IEnumerator EscapeDetectionRoutine()
    {
        yield return new WaitForSeconds(3f);

        if (GameController.state.value != GameController.State.Action) yield break;

        string[] randomPhrases = new string[]
        {
            "what did you do with the ball???",
            "why",
            "stop knocking the ball out of bounds",
            "stop it",
            "hey maybe don't do that",
            "the shmexmanity",
            "that's okay! try again!!",
            "we're still friends, right?",
            "did you brush your teeth today?",
            "i do really like butterflies.",
            "ni hui buhui bofang shmex?! chihan!",
            "OK OK try again!!",
            "no crying, keep trying!"
        };

        DisplayBroadcastManager.DisplayMessage(randomPhrases.PickRandom(), 4f);

        yield return new WaitForSeconds(4f);

        Reset();

        Initialize();
        
        
    }
 }

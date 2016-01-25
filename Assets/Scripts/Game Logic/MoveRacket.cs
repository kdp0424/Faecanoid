using UnityEngine;
using System.Collections;

public class MoveRacket : MonoBehaviour {
    public MinMaxFloat speed = new MinMaxFloat(15f, 45f, 30f);
	public string horizontalAxis = "Horizontal";
    public string verticalAxis = "Vertical";

	void FixedUpdate () {
		float h = Input.GetAxis(horizontalAxis);
        float v = Input.GetAxis(verticalAxis);
        
        
        GetComponent<Rigidbody2D>().velocity = new Vector2(h, v).normalized * speed.value;
    }
}

using UnityEngine;
using System.Collections;

public class LookAtBehavior : MonoBehaviour {
	
	public Component target;
	
	void Start () {
		if(target == null) this.enabled = false;
	}
	
	void Update () {
		//RULECORE._SeekTarget(this.gameObject, target.transform.position, 0.0f);
		transform.LookAt(target.transform);
	}
}

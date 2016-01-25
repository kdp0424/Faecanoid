using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DestructibleMeshBehavior : MonoBehaviour {

	public GameObject destructionFX;
	public GameObject destructionLargeFX;

	public int extraExplosions = 1;
	public float minDistance = 0f;
	public float maxDistance = 8f;
	public float explosionSize = 5f;

	public bool randomExplosionsEnabled = false;
	public float explosionIntervalMin = 0.1f;
	public float explosionIntervalMax = 1f;
	
	public float minLifetime = 2f;
	public float maxLifetime = 15f;
	
	bool complete = false;

	public List<Rigidbody> groups;
	public List<GameObject> subGroups;

	public MultipleAudioSourceManager multiAudio;
	public AudioClip[] audioClips;
	public int clipsPerPlay = 3;
	public float delayBetweenClips = 0.1f;
	

	void Start () {
		if(randomExplosionsEnabled) StartCoroutine(RandomExplosions());
		if(groups.Count > 0) {
			StartCoroutine(GroupDestructionSequence());
		} else {
			StartCoroutine(RandomDestructionSequence());
		}
		Destroy(gameObject, 30.0f);	
	}
	
	public IEnumerator RandomExplosions() {
		while(!complete) {
			//Randomly choose directions for the explosion vectors
			int direction1 = (Random.Range(0, 1) == 0) ? 1 : -1;
			int direction2 = (Random.Range(0, 1) == 0) ? 1 : -1;
			int direction3 = (Random.Range(0, 1) == 0) ? 1 : -1;
					
			Vector3 explosionPos = new Vector3(
				Random.Range(minDistance, maxDistance) * direction1, //x offset
				Random.Range(minDistance / 4, maxDistance / 4) * direction2, //y offset            
				Random.Range(minDistance, maxDistance) * direction3  //z offset
			);
		
			if(destructionFX) Instantiate(destructionFX, transform.position + explosionPos, Quaternion.identity);
			PlaySfxSequence();
			yield return new WaitForSeconds(Random.Range(explosionIntervalMin, explosionIntervalMax ));
		}
	}

	public IEnumerator RandomDestructionSequence() {		
		
		//if(destructionLargeFX) Instantiate(destructionLargeFX, transform.position, Quaternion.identity);
		//PlaySfxSequence();
		
		AddForceAll();
		
		yield return new WaitForSeconds( 0.75f);		
		
		//Debug.Log("number of explosions : " + extraExplosions);
		for(int n = 0; n < extraExplosions; n++) {
			//Randomly choose directions for the explosion vectors
			int direction1 = (Random.Range(0, 1) == 0) ? 1 : -1;
			int direction2 = (Random.Range(0, 1) == 0) ? 1 : -1;
			int direction3 = (Random.Range(0, 1) == 0) ? 1 : -1;
			
			Vector3 explosionPos = new Vector3(
				Random.Range(minDistance, maxDistance) * direction1, //x offset
				Random.Range(minDistance / 4, maxDistance / 4) * direction2, //y offset            
				Random.Range(minDistance, maxDistance) * direction3  //z offset
			);
			Collider[] children = Physics.OverlapSphere(transform.position + explosionPos, explosionSize);
			//Debug.Log("Number of children in destruction overlap sphere : " + children.Length); 
			for(int c = 0; c < children.Length; c++) {
				
				if(!children[c].transform.IsChildOf(transform)) continue;
				
				Rigidbody rb = children[c].GetComponent<Rigidbody>();
				if(rb == null) continue;
				children[c].transform.SetParent(GameController.instance.transform, true);
				
				ConfigurableJoint joint = children[c].GetComponent<ConfigurableJoint>();
				
				Destroy (joint);
				
				rb.AddForce(Random.onUnitSphere * Random.Range(50f, 150f));
				rb.AddTorque(Random.onUnitSphere * Random.Range(-80f, 80f));
				Destroy (children[c].gameObject, Random.Range(minLifetime, maxLifetime));
			}	
			
			Instantiate(destructionLargeFX, transform.position + explosionPos, Quaternion.identity);
			//Debug.Log("Detonated explosion at time : " + Time.realtimeSinceStartup);
			yield return new WaitForSeconds(Random.Range(explosionIntervalMin, explosionIntervalMax));
		}

		yield return new WaitForSeconds (1f);
		
		AddForceAll(true);
		
		complete = true;
		//Debug.Log("Completed destruction at time : " + Time.realtimeSinceStartup);
		yield return null;	
	}
	
	public void AddForceAll(bool detach = false) {
		Collider[] remaining = Physics.OverlapSphere(transform.position, maxDistance * 2 + explosionSize * 2);
		if(destructionFX) Instantiate(destructionFX, transform.position, Quaternion.identity);
		//Debug.Log("Detonated explosion at time : " + Time.realtimeSinceStartup);
		PlaySfxSequence();
		
		for(int c = 0; c < remaining.Length; c++) {
			Rigidbody rb = remaining[c].GetComponent<Rigidbody>();
			if(rb == null) continue;
			
			if(!remaining[c].transform.IsChildOf(transform)) continue;
			
			if(detach) {
				rb.transform.SetParent(GameController.instance.transform, true);
				
				ConfigurableJoint joint = rb.GetComponent<ConfigurableJoint>();
				
				Destroy (joint);
			}
			
			rb.AddForce(Random.onUnitSphere * Random.Range(40f, 300.0f));
			rb.AddTorque(Random.onUnitSphere * Random.Range(-120f, 120f));
			Destroy (remaining[c].gameObject, Random.Range(minLifetime, maxLifetime));
		}		
	}
	
	public IEnumerator GroupDestructionSequence() {
		
		Instantiate(destructionLargeFX, transform.position, Quaternion.identity);
		PlaySfxSequence();
		yield return new WaitForSeconds( 0.75f);
		
		for(int n = 0; n < groups.Count; n++) {
			
			groups[n].GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * Random.Range(200f, 4000.0f));
			groups[n].GetComponent<Rigidbody>().AddTorque(Random.onUnitSphere * Random.Range(-490f, 490f));

			//Instantiate(destructionFX, groups[n].transform.position, Quaternion.identity);
			yield return new WaitForSeconds(Random.Range(0.15f, 0.3f));
		}		
		
		yield return new WaitForSeconds(0.75f);
		
		subGroups.Shuffle();
		for(int n = 0; n < subGroups.Count; n++) {
			
			StartCoroutine(SubGroupSequence(subGroups[n]));

			yield return new WaitForSeconds(Random.Range(explosionIntervalMin, explosionIntervalMax));
		}
		
		complete = true;
		
		yield return null;		
	
	}
	
	int explosionNum = 0;
	
	public IEnumerator SubGroupSequence(GameObject subgroup) {
		if(explosionNum < 3) {
			Instantiate(destructionLargeFX, subgroup.transform.position, Quaternion.identity);
			PlaySfxSequence();
			explosionNum++;
		}
		yield return new WaitForSeconds(0.5f);
		
		
		yield return new WaitForSeconds(Random.Range(explosionIntervalMin, explosionIntervalMax));
	
		Rigidbody[] children = subgroup.GetComponentsInChildren<Rigidbody>();
		
//		int randomIndex = Random.Range(0, children.Length);
		
		for(int c = 0; c < children.Length; c++) {

			children[c].transform.SetParent(GameController.instance.transform, true);
			
			ConfigurableJoint joint = children[c].GetComponent<ConfigurableJoint>();
	

			Destroy (joint);

			children[c].GetComponent<Rigidbody>().AddForce(Random.onUnitSphere * Random.Range(100f, 300.0f));
			children[c].GetComponent<Rigidbody>().AddTorque(Random.onUnitSphere * Random.Range(-120f, 120f));
			Destroy (children[c].gameObject, Random.Range(minLifetime, maxLifetime));
		}	
		
		//Instantiate(destructionFX, group.transform.position + Random.onUnitSphere * 2f, Quaternion.identity);
	}
	
	public void PlaySfxSequence() {
		StartCoroutine(SfxSequence());
	}
	
	public void PlaySFX() {
		if(audioClips.Length == 0) return;
		if(multiAudio) {
			multiAudio.PlayAudio(audioClips.PickRandom());	
		} else {
			//audioSource.PlayOneShot(audioClips.PickRandom());
		}	
		//Debug.Log("Playing destruction sound");
	}	
	
	public IEnumerator SfxSequence () {
		for(int n = 0; n < clipsPerPlay; n++) {
			PlaySFX ();
			yield return new WaitForSeconds(delayBetweenClips);
		}
	}

}

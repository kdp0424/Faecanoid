using UnityEngine;
using System.Collections;

public class RULECORE : MonoBehaviour {
		
	static public void _RotateYaw(Transform transform, float fTurnRate) {
		if(fTurnRate > 6.0f) fTurnRate = 6.0f;
		if(fTurnRate < -6.0f) fTurnRate = -6.0f;
		transform.Rotate(fTurnRate * Vector3.up);
	}
	
	static public void RotateTowards (Transform transform, Vector3 target, float RotationSpeed) {		
		
		Quaternion _lookRotation;
		Vector3 _direction;
		
		//find the vector pointing from the object's position to the target
		_direction = (target - transform.position).normalized;
		
		//create the rotation the object needs to be in to look at the target
		_lookRotation = Quaternion.LookRotation(_direction);
		
		//rotate the object over time according to speed until we are in the required rotation
		transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
		
	}
	
	static public void RotateTowards (Transform	transform, Vector3 target, float RotationSpeed, float deltaTime) {
		Quaternion _lookRotation;
		Vector3 _direction;
		
		//find the vector pointing from the object's position to the target
		_direction = (target - transform.position).normalized;
		
		//create the rotation the object needs to be in to look at the target
		_lookRotation = Quaternion.LookRotation(_direction);
		
		
		//rotate the object over time according to speed until we are in the required rotation
		transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, deltaTime * RotationSpeed);	
	}
	
	static public void RotateTowardsConstant (Transform transform, Vector3 target, Vector3 up, float RotationSpeed, float deltaTime) {
		Quaternion _lookRotation;
		Vector3 _direction;
		
		//find the vector pointing from the object's position to the target
		_direction = (target - transform.position).normalized;
		
		//create the rotation the object needs to be in to look at the target
		_lookRotation = Quaternion.LookRotation(_direction, up);
		
		//rotate the object over time according to speed until we are in the required rotation
		transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, RotationSpeed * deltaTime);
	}	
	
	static public void _MoveForward(Transform transform, float fVelocity) {
		if(fVelocity > 0.75f) fVelocity = 0.75f;
		if(fVelocity < -0.75f) fVelocity = -0.75f;
		transform.Translate(fVelocity * Vector3.forward, Space.Self);	
	}
	
	// _SeekTarget : Seeks out the indicated target and returns true when reached
	static public bool _SeekTarget(GameObject seeker, Vector3 target, float fMaxVelocity) {
		float fTargetDistance;
		float zIsTargetBehindMe, zIsTargetInFrontOfMe, zIsTargetToMyLeft, zIsTargetToMyRight;
		AICore._GetSpatialAwareness2D(seeker, target, out fTargetDistance, out zIsTargetBehindMe, out zIsTargetInFrontOfMe, out zIsTargetToMyLeft, out zIsTargetToMyRight);
		
		// Detect whether TARGET is sufficiently in front
		if(zIsTargetInFrontOfMe > 0.99) {
			// Satisfactally facing target	
			// No need to turn
		} else {
			// Should we turn right or left?
			if(zIsTargetToMyRight > zIsTargetToMyLeft) {
				// Turn right
				float fTurnRate;
				if(zIsTargetBehindMe > zIsTargetToMyRight) {
					fTurnRate = AICore.Defuzzify(zIsTargetBehindMe, 0.0f, 6.0f);					
				} else {
					fTurnRate = AICore.Defuzzify(zIsTargetToMyRight, 0.0f, 6.0f);
				}
				RULECORE._RotateYaw(seeker.transform, fTurnRate);
			} else {
				// Turn left
				float fTurnRate;
				if(zIsTargetBehindMe > zIsTargetToMyLeft) {
					fTurnRate = AICore.Defuzzify(zIsTargetBehindMe, 0.0f, 6.0f);					
				} else {
					fTurnRate = AICore.Defuzzify(zIsTargetToMyLeft, 0.0f, 6.0f);
				}
				RULECORE._RotateYaw(seeker.transform, -fTurnRate);
			}
		}
					
		if(fMaxVelocity > 0.0f) {
			// Only drive forward when facing nearly toward target	
			if(zIsTargetInFrontOfMe > 0.7) {
				// Only drive forward if we're far enough from target
				if(fTargetDistance >= 3.00f) {
					float fVelocity = AICore.Defuzzify(zIsTargetInFrontOfMe, 0.0f, fMaxVelocity);
					RULECORE._MoveForward(seeker.transform, fVelocity);
				}
			}
			
			// Return whether target is reached
			return fTargetDistance < 4.00f;
		} else {
			// Return whether we're facing the target
			// Also include whether target is reached because when
			// we're very close to the target we get weird look at information
			return zIsTargetInFrontOfMe > 0.9f || fTargetDistance < 5.00f;
		}
		
	}
}

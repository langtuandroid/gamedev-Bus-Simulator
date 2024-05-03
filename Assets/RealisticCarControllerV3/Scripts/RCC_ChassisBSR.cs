//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

// Script Will Simulate Chassis Movement Based On Vehicle Rigidbody Velocity.
[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/Chassis")]
public class RCC_ChassisBSR : MonoBehaviour {

	private Rigidbody mainRigidBSR;

	private float chassisVerticalLeanBSR = 4.0f;		// Chassis Vertical Lean Sensitivity.
	private float chassisHorizontalLeanBSR = 4.0f;		// Chassis Horizontal Lean Sensitivity.
	private float horizontalLeanBSR = 3f;
	private float verticalLeanBSR = 3f;

	private void Start () {

		mainRigidBSR = GetComponentInParent<RCC_CarControllerV3> ().GetComponent<Rigidbody> ();

		chassisVerticalLeanBSR = GetComponentInParent<RCC_CarControllerV3> ().chassisVerticalLean;
		chassisHorizontalLeanBSR = GetComponentInParent<RCC_CarControllerV3> ().chassisHorizontalLean;

		if (!RCC_SettingsBSR.InstanceBSR.dontUseChassisJointBSR)
			ChassisJointBSR ();

	}

	private void OnEnable(){

		if (!RCC_SettingsBSR.InstanceBSR.dontUseChassisJointBSR)
			StartCoroutine ("ReEnableBSR");

	}

	private IEnumerator ReEnableBSR(){

		if(!GetComponent<ConfigurableJoint>())
			yield return null;

		GameObject _joint = GetComponentInParent<ConfigurableJoint>().gameObject;

		_joint.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
		yield return new WaitForFixedUpdate();
		_joint.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;

	}

	private void ChassisJointBSR(){

		GameObject colliders = new GameObject("Colliders");
		colliders.transform.SetParent(GetComponentInParent<RCC_CarControllerV3> ().transform, false);

		GameObject chassisJoint;

		Transform[] childTransforms = GetComponentInParent<RCC_CarControllerV3> ().chassis.GetComponentsInChildren<Transform>();

		foreach(Transform t in childTransforms){

			if(t.gameObject.activeSelf && t.GetComponent<Collider>()){

				if (t.childCount >= 1) {
					Transform[] childObjects = t.GetComponentsInChildren<Transform> ();
					foreach (Transform c in childObjects) {
						if (c != t) {
							c.SetParent (transform);
						}
					}
				}

				GameObject newGO = (GameObject)Instantiate(t.gameObject, t.transform.position, t.transform.rotation);
				newGO.transform.SetParent(colliders.transform, true);
				newGO.transform.localScale = t.lossyScale;

				Component[] components = newGO.GetComponents(typeof(Component));

				foreach(Component comp  in components){
					if(!(comp is Transform) && !(comp is Collider)){
						Destroy(comp);
					}
				}

			}

		}

		chassisJoint = (GameObject)Instantiate((RCC_SettingsBSR.InstanceBSR.chassisJointBSR), Vector3.zero, Quaternion.identity);
		chassisJoint.transform.SetParent(mainRigidBSR.transform, false);
		chassisJoint.GetComponent<ConfigurableJoint> ().connectedBody = mainRigidBSR;

		transform.SetParent(chassisJoint.transform, false);

		Collider[] collidersInChassis = GetComponentsInChildren<Collider>();

		foreach(Collider c in collidersInChassis)
			Destroy(c);

		GetComponentInParent<Rigidbody> ().centerOfMass = new Vector3 (mainRigidBSR.centerOfMass.x, mainRigidBSR.centerOfMass.y + 1f, mainRigidBSR.centerOfMass.z);

	}

	private void FixedUpdate () {

		if (RCC_SettingsBSR.InstanceBSR.dontUseChassisJointBSR)
			LegacyChassisBSR ();

	}

	private void LegacyChassisBSR (){

		verticalLeanBSR = Mathf.Clamp(Mathf.Lerp (verticalLeanBSR, mainRigidBSR.angularVelocity.x * chassisVerticalLeanBSR, Time.fixedDeltaTime * 5f), -3f, 3f);
		horizontalLeanBSR = Mathf.Clamp(Mathf.Lerp (horizontalLeanBSR, (transform.InverseTransformDirection(mainRigidBSR.angularVelocity).y * (transform.InverseTransformDirection(mainRigidBSR.velocity).z >= 0 ? 1 : -1)) * chassisHorizontalLeanBSR, Time.fixedDeltaTime * 5f), -3f, 3f);

		if(float.IsNaN(verticalLeanBSR) || float.IsNaN(horizontalLeanBSR) || float.IsInfinity(verticalLeanBSR) || float.IsInfinity(horizontalLeanBSR) || Mathf.Approximately(verticalLeanBSR, 0f) || Mathf.Approximately(horizontalLeanBSR, 0f))
			return;

		Quaternion target = Quaternion.Euler(verticalLeanBSR, transform.localRotation.y + (mainRigidBSR.angularVelocity.z), horizontalLeanBSR);
		transform.localRotation = target;

	}

}

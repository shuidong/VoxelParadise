using UnityEngine;
using System.Collections;

public class GravitySphere : MonoBehaviour {
	public float gravity = -10f;

	public void Attract(Transform body)
	{
		Vector3 gravityUp = (body.position - transform.position).normalized;
		Vector3 localUp = body.up;
		Debug.Log(-(gravityUp * gravity) +"    "+gravityUp);
		body.GetComponent<Rigidbody>().AddForce(-(gravityUp * gravity));
		body.rotation = Quaternion.FromToRotation(localUp, gravityUp) * body.rotation;
		
	}
}

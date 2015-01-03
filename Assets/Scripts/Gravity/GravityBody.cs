using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{

	GravitySphere planet;

	void Awake()
	{
		planet = GameObject.FindGameObjectWithTag("Planet").GetComponent<GravitySphere>();
		GetComponent<Rigidbody>().useGravity = false;
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
	}
	void FixedUpdate()
	{
		planet.Attract(transform);
		//Debug.Log(transform.position);
	}
}
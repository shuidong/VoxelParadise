using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 15;
	private Vector3 moveDir;

	
	// Update is called once per frame
	void Update () {
		moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

	}

	void FixedUpdate()
	{
		GetComponent<Rigidbody>().transform.position=(GetComponent<Rigidbody>().transform.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
	}
}

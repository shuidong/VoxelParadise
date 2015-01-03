using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float moveSpeed = 15;
	public float mouseSensitivityX = 250f;
	public float mouseSensitivityY = 250f;
	public float walkSpeed = 8f;
	public float jumpForce = 250f;
	public LayerMask groundedMask;

	Transform cameraT;
	bool grounded;
	bool cheatModeActive = false;
	float verticalLookRotation;

	Vector3 moveAmount;
	Vector3 smoothMoveVelocity;
	Vector3 moveDir;

	void Start()
	{
		cameraT = Camera.main.transform;
	}
	// Update is called once per frame
	void Update () {
		//moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
		transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivityX);
		verticalLookRotation += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivityY;
		verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90, 90);
		cameraT.localEulerAngles = Vector3.left * verticalLookRotation;

		Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
		Vector3 targetMoveAmount = moveDir * walkSpeed;
		moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

		if (Input.GetKeyDown (KeyCode.C))
		{
			cheatModeActive = !cheatModeActive;
			Debug.Log("cheatModeActive: " + cheatModeActive);
		}

		if (cheatModeActive)
		{
			if (Input.GetButtonDown("Jump"))
			{
				GetComponent<Rigidbody>().AddForce(transform.up * jumpForce);
			}
		}
		else
		{
			if (Input.GetButtonDown("Jump"))
			{
				if (grounded)
				{
					GetComponent<Rigidbody>().AddForce(transform.up * jumpForce);
				}
			}
			grounded = false;
			Ray ray = new Ray(transform.position, -transform.up);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 1 + .1f, groundedMask))
			{
				grounded = true;
			}
		}
	}

	void FixedUpdate()
	{
		GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
		//GetComponent<Rigidbody>().transform.position=(GetComponent<Rigidbody>().transform.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
	}
}

using UnityEngine;

public class GravitySphere : MonoBehaviour
{
	public float gravity = -10f;
	private GenerateWorld world;
	public GameObject worldGO;

	public void Attract(Transform body)
	{
		world = worldGO.GetComponent("GenerateWorld") as GenerateWorld;
		Vector3 gravityUp = (body.position - CalculateCenterOfWorld(world.GetSizeWorld())).normalized;
		Vector3 localUp = body.up;
		body.GetComponent<Rigidbody>().AddForce(-gravityUp * gravity);
		body.rotation = Quaternion.FromToRotation(localUp, gravityUp) * body.rotation;
	}

	private Vector3 CalculateCenterOfWorld(Vector3 worldParameters)
	{
		return worldParameters / 2;
	}
}
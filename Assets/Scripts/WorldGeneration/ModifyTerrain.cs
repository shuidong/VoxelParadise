﻿using UnityEngine;

public class ModifyTerrain : MonoBehaviour
{
	private GenerateWorld world;
	private GameObject cameraGO;

	public int loadingDistance = 32;
	public int unloadingDistance = 64;
	private int multiplierLoading = 1;

	// Use this for initialization
	private void Start()
	{
		world = gameObject.GetComponent("GenerateWorld") as GenerateWorld;
		cameraGO = GameObject.FindGameObjectWithTag("MainCamera");
	}

	// Update is called once per frame
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			ReplaceBlockCenter(5, 0);
		}

		if (Input.GetMouseButtonDown(1))
		{
			AddBlockCenter(5, 255);
		}
		LoadChunks(GameObject.FindGameObjectWithTag("Player").transform.position, loadingDistance * multiplierLoading, unloadingDistance * multiplierLoading);

		if (Input.GetKeyDown(KeyCode.V))
		{
			if (multiplierLoading == 1)
			{
				multiplierLoading = 100;
				Debug.Log("ChunkLoadingDistance = High");
			}
			else
			{
				multiplierLoading = 1;
				Debug.Log("ChunkLoadingDistance = low");
			}
		}
	}

	public void ReplaceBlockCenter(float range, byte block)
	{
		//Replaces the block directly in front of the

		Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			if (hit.distance < range)
			{
				ReplaceBlockAt(hit, block);
			}
		}
	}

	public void AddBlockCenter(float range, byte block)
	{
		//Adds the block specified directly in front of the player

		Ray ray = new Ray(cameraGO.transform.position, cameraGO.transform.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			if (hit.distance < range)
			{
				AddBlockAt(hit, block);
			}
			Debug.DrawLine(ray.origin, ray.origin + (ray.direction * hit.distance), Color.green, 2);
		}
	}

	public void ReplaceBlockCursor(byte block)
	{
		//Replaces the block specified where the mouse cursor is pointing
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			ReplaceBlockAt(hit, block);
			Debug.DrawLine(ray.origin, ray.origin + (ray.direction * hit.distance),
			 Color.green, 2);
		}
	}

	public void AddBlockCursor(byte block)
	{
		//Adds the block specified where the mouse cursor is pointing
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			AddBlockAt(hit, block);
			Debug.DrawLine(ray.origin, ray.origin + (ray.direction * hit.distance),
			 Color.green, 2);
		}
	}

	public void ReplaceBlockAt(RaycastHit hit, byte block)
	{
		//removes a block at these impact coordinates, you can raycast against the terrain and call this with the hit.point
		Vector3 position = hit.point;
		position += (hit.normal * -0.5f);

		SetBlockAt(position, block);
	}

	public void AddBlockAt(RaycastHit hit, byte block)
	{
		//adds the specified block at these impact coordinates, you can raycast against the terrain and call this with the hit.point
		Vector3 position = hit.point;
		position += (hit.normal * 0.5f);

		SetBlockAt(position, block);
	}

	public void SetBlockAt(Vector3 position, byte block)
	{
		//sets the specified block at these coordinates
		int x = Mathf.RoundToInt(position.x);
		int y = Mathf.RoundToInt(position.y);
		int z = Mathf.RoundToInt(position.z);

		SetBlockAt(x, y, z, block);
	}

	public void SetBlockAt(int x, int y, int z, byte block)
	{
		//adds the specified block at these coordinates
		print("Adding: " + x + ", " + y + ", " + z);

		if (world.data[x + 1, y, z] == 254)
		{
			world.data[x + 1, y, z] = 255;
		}
		if (world.data[x - 1, y, z] == 254)
		{
			world.data[x - 1, y, z] = 255;
		}
		if (world.data[x, y, z + 1] == 254)
		{
			world.data[x, y, z + 1] = 255;
		}
		if (world.data[x, y, z - 1] == 254)
		{
			world.data[x, y, z - 1] = 255;
		}
		if (world.data[x, y + 1, z] == 254)
		{
			world.data[x, y + 1, z] = 255;
		}
		world.data[x, y, z] = block;
		UpdateChunkAt(x, y, z);
	}

	public void UpdateChunkAt(int x, int y, int z)
	{
		int updateX = Mathf.FloorToInt(x / world.chunkSize);
		int updateY = Mathf.FloorToInt(y / world.chunkSize);
		int updateZ = Mathf.FloorToInt(z / world.chunkSize);

		print("Updating: " + updateX + ", " + updateY + ", " + updateZ);

		world.chunks[updateX, updateY, updateZ].update = true;
		if (x - (world.chunkSize * updateX) == 0 && updateX != 0)
		{
			world.chunks[updateX - 1, updateY, updateZ].update = true;
		}

		if (x - (world.chunkSize * updateX) == 15 && updateX != world.chunks.GetLength(0) - 1)
		{
			world.chunks[updateX + 1, updateY, updateZ].update = true;
		}

		if (y - (world.chunkSize * updateY) == 0 && updateY != 0)
		{
			world.chunks[updateX, updateY - 1, updateZ].update = true;
		}

		if (y - (world.chunkSize * updateY) == 15 && updateY != world.chunks.GetLength(1) - 1)
		{
			world.chunks[updateX, updateY + 1, updateZ].update = true;
		}

		if (z - (world.chunkSize * updateZ) == 0 && updateZ != 0)
		{
			world.chunks[updateX, updateY, updateZ - 1].update = true;
		}

		if (z - (world.chunkSize * updateZ) == 15 && updateZ != world.chunks.GetLength(2) - 1)
		{
			world.chunks[updateX, updateY, updateZ + 1].update = true;
		}
	}

	public void LoadChunks(Vector3 playerPos, float distToLoad, float distToUnload)
	{
		for (int x = 0; x < world.chunks.GetLength(0); x++)
		{
			for (int y = 0; y < world.chunks.GetLength(1); y++)
			{
				for (int z = 0; z < world.chunks.GetLength(2); z++)
				{
					float BlockDistanceFromCenter = Mathf.Pow(playerPos.x-x*16, 2) + Mathf.Pow(playerPos.y-y*16, 2) + Mathf.Pow(playerPos.z-z*16, 2);
// 					float dist = Vector2.Distance(new Vector2(x * world.chunkSize,
// 					z * world.chunkSize), new Vector2(playerPos.x, playerPos.z));
					//Debug.Log("dist: "+BlockDistanceFromCenter);

					if (BlockDistanceFromCenter < Mathf.Pow(distToLoad, 2))
					{
						if (world.chunks[x, y, z] == null)
						{
							world.GenChunk(x,y, z);
						}
					}
					else if (BlockDistanceFromCenter > Mathf.Pow(distToUnload, 2))
					{
						if (world.chunks[x, y, z] != null)
						{
							world.UnloadChunk(x,y, z);
						}
					}
				}
			}
		}
	}
}
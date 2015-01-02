using UnityEngine;

public class GenerateWorld : MonoBehaviour
{
	public GameObject chunk;
	public GenerateChunks[, ,] chunks;
	public int chunkSize = 16;

	public byte[, ,] data;
	public int worldX = 128;
	public int worldY = 128;
	public int worldZ = 128;

	// Use this for initialization
	private void Start()
	{
		GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(worldX * 0.5f, worldY, worldZ * 0.5f);
		data = new byte[worldX, worldY, worldZ];
		int radiusPlanet = worldY * chunkSize - 40;
		int heightDif = 1500;

		for (int x = 0; x < worldX; x++)
		{
			for (int y = 0; y < worldY; y++)
			{
				for (int z = 0; z < worldZ; z++)
				{
					// 				if (Mathf.Pow(x - worldX / 2, 2) + Mathf.Pow(y - worldY / 2, 2) + Mathf.Pow(z - worldZ / 2, 2) < PerlinNoise(x,2,y,2,z,2)+600) //Cool effect    
					// 				{                                                                                                                                               
					// 					data[x, y, z] = 1;                                                                                                                          
					// 				}                                                                                                                                               

					//Debug.Log(PerlinNoise(x, y, z, 30, heightDif, 8));

					int BlockDistanceFromCenter = (int)(Mathf.Pow(x - worldX / 2, 2) + Mathf.Pow(y - worldY / 2, 2) + Mathf.Pow(z - worldZ / 2, 2));
					if (BlockDistanceFromCenter <= radiusPlanet + PerlinNoise(x, y, z, 15, heightDif, 1))
					{
						data[x, y, z] = 1;
					}
					if (y == 0)
					{
					 	data[x, y, z] = 1;
					}
				}
			}
		}

// 		for (int x = 0; x < worldX; x++)
// 		{
// 			for (int z = 0; z < worldZ; z++)
// 			{
// 				int stone = PerlinNoise(x, 0, z, 10, 3, 1.2f);
// 				stone += PerlinNoise(x, 300, z, 20, 4, 0) + 10;
// 				int dirt = PerlinNoise(x, 100, z, 50, 3, 0) + 1;
// 
// 				for (int y = 0; y < worldY; y++)
// 				{
// 					if (y <= stone)
// 					{
// 						data[x, y, z] = 1;
// 					}
// 					else if (y <= dirt + stone)
// 					{
// 						data[x, y, z] = 2;
// 					}
// 
// 				}
// 			}
// 		}

		chunks = new GenerateChunks[Mathf.FloorToInt(worldX / chunkSize),
		Mathf.FloorToInt(worldY / chunkSize), Mathf.FloorToInt(worldZ / chunkSize)];
	}

	// Update is called once per frame
	private void Update()
	{
	}

	public byte Block(int x, int y, int z)
	{
		if (x >= worldX || x < 0 || y >= worldY || y < 0 || z >= worldZ || z < 0)
		{
			return (byte)1;
		}

		return data[x, y, z];
	}

	private int PerlinNoise(int x, int y, int z, float scale, float height, float power)
	{
		float rValue;
		rValue = Noise.Noise.GetNoise(((double)x) / scale, ((double)y) / scale, ((double)z) / scale);
		rValue *= height;

		if (power != 0)
		{
			rValue = Mathf.Pow(rValue, power);
		}

		return (int)rValue;
	}

	public void GenColumn(int x, int z)
	{
		for (int y = 0; y < chunks.GetLength(1); y++)
		{

			GameObject newChunk = Instantiate(chunk,
 new Vector3(x * chunkSize - 0.5f, y * chunkSize + 0.5f, z * chunkSize - 0.5f),
 new Quaternion(0, 0, 0, 0)) as GameObject;

			chunks[x, y, z] = newChunk.GetComponent("GenerateChunks") as GenerateChunks;
			chunks[x, y, z].worldGO = gameObject;
			chunks[x, y, z].chunkSize = chunkSize;
			chunks[x, y, z].chunkX = x * chunkSize;
			chunks[x, y, z].chunkY = y * chunkSize;
			chunks[x, y, z].chunkZ = z * chunkSize;
		}
	}

	public void UnloadColumn(int x, int z)
	{
		for (int y = 0; y < chunks.GetLength(1); y++)
		{
			Object.Destroy(chunks[x, y, z].gameObject);

		}
	}
}
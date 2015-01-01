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
		data = new byte[worldX, worldY, worldZ];
		for (int x = 0; x < worldX; x++)
		{
			for (int y = 0; y < worldY; y++)
			{
				for (int z = 0; z < worldZ; z++)
				{
					// 					if (Mathf.Pow(x - worldX / 2, 2) + Mathf.Pow(y - worldY / 2, 2) + Mathf.Pow(z - worldZ / 2, 2) < PerlinNoise(x,2,y,2,z,2)+600) //Cool effect
					// 					{
					// 						data[x, y, z] = 1;
					// 					}

// 					if (Mathf.Pow(x - worldX / 2, 2) + Mathf.Pow(y - worldY / 2, 2) + Mathf.Pow(z - worldZ / 2, 2) < PerlinNoise(x, 7, z, 7, 1, 1) + 20)
// 					{
// 						data[x, y, z] = 1;
// 					}
					 					if (y <= 8)
					 					{
					 						data[x, y, z] = 1;
					 					}
				}
			}
		}

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
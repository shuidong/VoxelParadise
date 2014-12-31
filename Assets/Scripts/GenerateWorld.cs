using UnityEngine;

public class GenerateWorld : MonoBehaviour
{
	public GameObject chunk;
	public GameObject[, ,] chunks;
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

					if (Mathf.Pow(x - worldX / 2, 2) + Mathf.Pow(y - worldY / 2, 2) + Mathf.Pow(z - worldZ / 2, 2) < PerlinNoise(x,7,z,7,1,1)+600)
					{
						data[x, y, z] = 1;
					}
// 					if (y <= 8)
// 					{
// 						data[x, y, z] = 1;
// 					}
     
				}
			}
		}

		chunks = new GameObject[Mathf.FloorToInt(worldX / chunkSize), Mathf.FloorToInt(worldY / chunkSize), Mathf.FloorToInt(worldZ / chunkSize)];

		for (int x = 0; x < chunks.GetLength(0); x++)
		{
			for (int y = 0; y < chunks.GetLength(1); y++)
			{
				for (int z = 0; z < chunks.GetLength(2); z++)
				{
					chunks[x, y, z] = Instantiate(chunk,
					 new Vector3(x * chunkSize, y * chunkSize, z * chunkSize),
					 new Quaternion(0, 0, 0, 0)) as GameObject;

					GenerateChunks newChunkScript = chunks[x, y, z].GetComponent("GenerateChunks") as GenerateChunks;

					newChunkScript.worldGO = gameObject;
					newChunkScript.chunkSize = chunkSize;
					newChunkScript.chunkX = x * chunkSize;
					newChunkScript.chunkY = y * chunkSize;
					newChunkScript.chunkZ = z * chunkSize;
				}
			}
		}
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

	int PerlinNoise(int x, int y, int z, float scale, float height, float power)
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
}
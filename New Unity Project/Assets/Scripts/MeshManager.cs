using UnityEngine;
using System.Collections;

public class MeshManager : MonoBehaviour
{
	private MeshManager(){}

	private static MeshManager instance;

	public static MeshManager Instance
	{
		get
		{
			if (instance == null)
				instance = GameObject.FindGameObjectWithTag("MeshManager").GetComponent<MeshManager>();
			return instance;
		}
	}
	// Use this for initialization
	void Start () 
	{
//		meshFilter = gameObject.GetComponent<MeshFilter> ();
//		mesh = meshFilter.mesh;
//		GenerateMesh ();
//		StartCoroutine(SetHeights());
	}

	public void Regenerate(float tileW, MeshFilter meshFilter, MeshDetail meshDetail, float localScale)
	{
		GenerateMesh(tileW, meshFilter, meshDetail, localScale);
		StartCoroutine(SetHeights(meshFilter, localScale));
	}


	private IEnumerator SetHeights(MeshFilter meshFilter, float localScale)
	{
		float h;
		Mesh mesh = meshFilter.mesh;
		Vector3[] verts = mesh.vertices;
		for (int i = 0; i < verts.Length; i++)
		{
			verts[i] = mesh.vertices[i];
//			Random.seed = (int)((meshFilter.gameObject.transform.position.x + mesh.vertices[i].x * localScale)* 1000 + meshFilter.gameObject.transform.position.z + mesh.vertices[i].z * localScale);
//			h = Random.Range (0, 3);
			h = 1.0f * Mathf.PerlinNoise((meshFilter.gameObject.transform.position.x + mesh.vertices[i].x * localScale)* 1000, meshFilter.gameObject.transform.position.z + mesh.vertices[i].z * localScale);
			h -= 0.5f;
			verts[i].y += h;

			if ( i % 300 == 299)
			{
				mesh.vertices = verts;
				meshFilter.mesh = mesh;
				yield return 0;
			}
		}
		Debug.Log (Mathf.PerlinNoise(2.0f, 2.0f));
		mesh.vertices = verts;
		mesh.RecalculateNormals ();
		meshFilter.mesh = mesh;
		meshFilter.GetComponent<MeshCollider> ().sharedMesh = null;
		meshFilter.GetComponent<MeshCollider> ().sharedMesh = meshFilter.mesh;
//		yield return 0;
	}

	private void GenerateMesh(float tileW, MeshFilter meshFilter, MeshDetail meshDetail, float localScale)
	{
		int width;
		int height;
		float spacing;
		if (meshDetail == MeshDetail.HIGH)
		{
			width =	height = 32;
		}
		else if (meshDetail == MeshDetail.MED)
		{
			width = height = 16;
		}
		else
		{
			// low detail
			width = height = 8;
		}

		spacing = (tileW / (float)width) / localScale;

		Vector3[] verts = new Vector3[(width + 1) * (height + 1)];
		Vector3[] norms = new Vector3[(width + 1) * (height + 1)];
		Vector2[] uvs = new Vector2[(width + 1) * (height + 1)];
		int[] tris = new int[3 * 2 * (width * height)]; 			// 3 points per triangle, 2 triangles per quad, width * height quads

		int i = 0;
		for (float x = 0; x < width + 1; x++)
		{
			for (int z = 0; z < height + 1; z++)
			{
				verts[i].x = (x - width / 2.0f) * spacing;
				verts[i].y = 0;
				verts[i].z = (z - height / 2.0f) * spacing;
				norms[i] = new Vector3(0.0f,1.0f,0.0f);
				uvs[i].x = x / (float)width;
				uvs[i].y = z / (float)height;

				i++;
			}
		}
		int t = 0;
		for (int n = 0; n < verts.Length; n++)
		{
			if (n % (width + 1) == width)
				continue;
			if (n / (height + 1) == height)
				continue;
			tris[t  ] = n;
			tris[t+1] = n + 1;
			tris[t+2] = n + width + 1;
			tris[t+3] = n + 1;
			tris[t+4] = n + width + 2;
			tris[t+5] = n + width + 1;
			t += 6;
		}
		Mesh mesh = new Mesh ();
		mesh.vertices = verts;
		mesh.normals = norms;
		mesh.uv = uvs;
		mesh.triangles = tris;
		mesh.name = "not null";
		meshFilter.mesh = mesh;
	}
}

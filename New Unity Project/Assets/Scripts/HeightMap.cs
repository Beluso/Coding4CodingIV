using UnityEngine;
using System.Collections;

public class HeightMap : MonoBehaviour 
{
	public int width = 100;
	public int height = 100;

	public float spacing = 4.0f;

	public Texture2D heightmap;
	private MeshFilter meshFilter;
	private Mesh mesh;
	public float max_height = 10f;

	void Awake()
	{

	}

	// Use this for initialization
	void Start () 
	{
		meshFilter = gameObject.GetComponent<MeshFilter> ();
		mesh = meshFilter.mesh;
		GenerateMesh ();
		StartCoroutine(SetHeights());
	}

	IEnumerator SetHeights()
	{
		float h;
		Vector2 uv;
		Vector3[] verts = mesh.vertices;

		for (int i = 0; i < verts.Length; i++)
		{
			verts[i] = mesh.vertices[i];
			uv = mesh.uv[i];
			h = heightmap.GetPixelBilinear(uv.x, uv.y).r;

			verts[i].y += h * max_height;

			if ( i % 30 == 29)
			{
				mesh.vertices = verts;
				meshFilter.mesh = mesh;
				yield return 0;
			}
		}
		mesh.vertices = verts;
		mesh.RecalculateNormals ();
		meshFilter.mesh = mesh;
		gameObject.GetComponent<MeshCollider> ().sharedMesh = null;
		gameObject.GetComponent<MeshCollider> ().sharedMesh = mesh;
	}

	void GenerateMesh()
	{
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

		mesh = new Mesh ();
		mesh.vertices = verts;
		mesh.normals = norms;
		mesh.uv = uvs;
		mesh.triangles = tris;
		mesh.name = "not null";
		meshFilter.mesh = mesh;
	}
}

using UnityEngine;
using System.Collections;

public class MeshManager : MonoBehaviour
{
	private struct MeshDet
	{
		public MeshFilter meshFilter;
		public Vector3[] verts;
		public Vector3[] norms;
		public Vector2[] uvs;
		public int[] tris;
	}
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
			Random.seed = (int)((meshFilter.gameObject.transform.position.x + mesh.vertices[i].x * localScale) * 1000 + meshFilter.gameObject.transform.position.z + mesh.vertices[i].z * localScale);
			h = Mathf.PerlinNoise((meshFilter.gameObject.transform.position.x + mesh.vertices[i].x * localScale), meshFilter.gameObject.transform.position.z + mesh.vertices[i].z * localScale);
			h += Random.Range(0.0f, 2.0f);
			if (h < 2.0f)
				h = 2.0f;
			h -= 2.0f;
			verts[i].y += h;
			
			if ( i % 300 == 299)
			{
				mesh.vertices = verts;
				meshFilter.mesh = mesh;
				yield return 0;
			}
		}
		mesh.vertices = verts;
		mesh.RecalculateNormals ();
		meshFilter.mesh = mesh;
		meshFilter.GetComponent<MeshCollider> ().sharedMesh = null;
		meshFilter.GetComponent<MeshCollider> ().sharedMesh = meshFilter.mesh;
		//		yield return 0;
	}
	
	private void GenerateMesh(float tileW, MeshFilter meshFilter, MeshDetail meshDetail, float localScale)
	{
		/*
		 * we know that the skirts are:
		 * skirts[0] = top
		 * skirts[1] = right
		 * skirts[2] = down
		 * skirts[3] = left
		 */

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
		for (int x = 0; x < width + 1; x++)
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

		MakeSkirts(width, height, tileW, spacing, meshFilter, meshDetail, localScale);
	}

	private void MakeSkirts(int width, int height, float tileW, float spacing, MeshFilter meshFilter, MeshDetail meshDetail, float localScale)
	{
		// we know that height = width, so we can go down using width instead of using height
		MeshDet[] skirt = new MeshDet[4];
		int i = 0;
		for (i = 0; i < 4; i++)
		{
			skirt[i].meshFilter = meshFilter.GetComponent<Tile>().skirts[i].GetComponent<MeshFilter>();
			skirt[i].verts = new Vector3[(width + 1) * 2];
			skirt[i].norms = new Vector3[(width + 1) * 2];
			skirt[i].uvs = new Vector2[(width + 1) * 2];
			skirt[i].tris = new int[3 * 2 * width];
		}
		//top
		i = 0;
		for (int x = 0; x < width + 1; x++)
		{
			skirt[0].verts[i].x = (x - width / 2.0f) * spacing;
			skirt[0].verts[i].y = 0;
			skirt[0].verts[i].z = (0 - height / 2.0f) * spacing;
			skirt[0].norms[i] = new Vector3(0.0f,0.0f,1.0f);
			skirt[0].uvs[i].x = x / (float)width;
			skirt[0].uvs[i].y = 0 / (float)height;
			i++;
		}
		//right
		i = 0;
		for (int z = 0; z < height + 1; z++)
		{
			skirt[1].verts[i].x = (width - width / 2.0f) * spacing;
			skirt[1].verts[i].y = 0;
			skirt[1].verts[i].z = (z - height / 2.0f) * spacing;
			i++;
		}
		//down
		i = 0;
		for (int x = 0; x < width + 1; x++)
		{
			skirt[2].verts[i].x = (x - width / 2.0f) * spacing;
			skirt[2].verts[i].y = 0;
			skirt[2].verts[i].z = (height - height / 2.0f) * spacing;
			i++;
		}
		//left
		i = 0;
		for (int z = 0; z < height + 1; z++)
		{
			skirt[3].verts[i].x = (0 - width / 2.0f) * spacing;
			skirt[3].verts[i].y = 0;
			skirt[3].verts[i].z = (z - height / 2.0f) * spacing;
			i++;
		}


	}
}

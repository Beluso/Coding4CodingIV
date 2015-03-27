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
			Random.seed = (int)((meshFilter.gameObject.transform.position.x + mesh.vertices[i].x * localScale) * 1000 + meshFilter.gameObject.transform.position.z + mesh.vertices[i].z * localScale);
			h = Mathf.PerlinNoise((meshFilter.gameObject.transform.position.x + mesh.vertices[i].x * localScale), meshFilter.gameObject.transform.position.z + mesh.vertices[i].z * localScale);
			h += Random.Range(0.0f, 48.0f);
			if (h < 4.0f)
				h = 4.0f;
			h -= 4.0f;
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
		int width;
		int height;
		float spacing;
		if (meshDetail == MeshDetail.HIGH)
		{
			width =	height = 8;
		}
		else if (meshDetail == MeshDetail.MED)
		{
			width = height = 8;
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
				//default case
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

//		spacing = (tileW / (float)width) / localScale;
//		
//		Vector3[] verts = new Vector3[(width + 3) * (height + 3)];
//		Vector3[] norms = new Vector3[(width + 3) * (height + 3)];
//		Vector2[] uvs = new Vector2[(width + 3) * (height + 3)];
//		int[] tris = new int[3 * 2 * ((width + 1) * (height + 1))];		// 3 points per triangle, 2 triangles per quad, width * height quads
//		
//		int i = 0;
//		for (float x = 0; x < width + 3; x++)
//		{
//			for (int z = 0; z < height + 3; z++)
//			{
//				/*
//				 * mesh of size 32x32 has verts of 33x33
//				 * make mesh with verts 35x35
//				 * shift up and to the left one unit
//				 * if vert is in first row, move down one extra unit
//				 * if vert is in last row, move up one extra unit
//				 * if vert is first of the row, move right one unit
//				 * if vert is last of the row, move left one unit
//				 */
//				
//				//default case
//				verts[i].x = (x - width / 2.0f) * spacing - spacing;
//				verts[i].y = 0;
//				verts[i].z = (z - height / 2.0f) * spacing  + spacing;
//				
//				if (z == 0) //first row, move down one unit
//				{
//					verts[i].x = (x - width / 2.0f) * spacing - spacing;
//					verts[i].y = 0;
//					verts[i].z = (z - height / 2.0f) * spacing;
//				}
//				if (z == height + 2) //last row, move up one unit
//				{
//					verts[i].x = (x - width / 2.0f) * spacing - spacing;
//					verts[i].y = 0;
//					verts[i].z = (z - height / 2.0f) * spacing  + 2 * spacing;
//				}
//				if (x == 0) //first of the row, move right one unit
//				{
//					verts[i].x = (x - width / 2.0f) * spacing;
//					verts[i].y = 0;
//					verts[i].z = (z - height / 2.0f) * spacing  + spacing;
//				}
//				if (x == width + 2) //last of the row, move left one unit
//				{
//					verts[i].x = (x - width / 2.0f) * spacing - 2 * spacing;
//					verts[i].y = 0;
//					verts[i].z = (z - height / 2.0f) * spacing  + spacing;
//				}
//				norms[i] = new Vector3(0.0f,1.0f,0.0f);
//				uvs[i].x = x / (float)width;
//				uvs[i].y = z / (float)height;
//				
//				i++;
//			}
//		}
//		int t = 0;
//		for (int n = 0; n < verts.Length; n++)
//		{
//			if (n % (width + 3) == width + 2)
//				continue;
//			if (n / (height + 3) == height + 2)
//				continue;
//			tris[t  ] = n;
//			tris[t+1] = n + 1;
//			tris[t+2] = n + width + 1;
//			tris[t+3] = n + 1;
//			tris[t+4] = n + width + 2;
//			tris[t+5] = n + width + 1;
//			t += 6;
//		}
//		Mesh mesh = new Mesh ();
//		mesh.vertices = verts;
//		mesh.normals = norms;
//		mesh.uv = uvs;
//		mesh.triangles = tris;
//		mesh.name = "not null";
//		meshFilter.mesh = mesh;
	}
}

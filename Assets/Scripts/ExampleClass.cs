using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour {

	public Material mat;
	private Vector3 startVertex;
	private Vector3 mousePos;


	void Update() 
	{
		mousePos = Input.mousePosition;
		if (Input.GetKeyDown(KeyCode.Space))
			startVertex = new Vector3(mousePos.x / Screen.width, mousePos.y / Screen.height, 0);

	}
	void OnPostRender() 
	{
		if (!mat) {
			Debug.LogError("Please Assign a material on the inspector");
			return;
		}
			
		GL.PushMatrix();
		mat.SetPass(0);
		GL.MultMatrix(Matrix4x4.identity); // proietta con la matrice della camera di Unity

		GL.Begin(GL.LINES);
		Vector3[] verts = meshDeformer.myMesh.vertices;
		for(int i = 0; i < verts.Length; i++)
		{
			GL.Color(new Color(1,0,0));
			GL.Vertex3(meshDeformer._prefab.transform.position.x,meshDeformer._prefab.transform.position.y,meshDeformer._prefab.transform.position.z); // centro del prefab
			GL.Vertex3(verts[i].x, verts[i].y, verts[i].z); 

		}

//		GL.Color(Color.blue);
//		GL.Vertex(startVertex);
//		GL.Vertex(new Vector3(mousePos.x / Screen.width, mousePos.y / Screen.height, 0));

		GL.End();



		GL.PopMatrix();
	}

}

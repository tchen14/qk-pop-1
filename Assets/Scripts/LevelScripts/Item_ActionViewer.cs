using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Item_ActionViewer : MonoBehaviour {

	public Texture2D push_ability;
	public Camera cam;
	private List<GameObject> billboards = new List<GameObject>();

	// Use this for initialization
	void Start () {
		billboards.Add(createSquare(0.5f, push_ability));
	}
	
	// Update is called once per frame
	void Update () {
		foreach (GameObject b in billboards) {
			b.transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
		}

	}

	GameObject createSquare(float width, Texture2D tex){
		Mesh plane = new Mesh();
		plane.name = "billbard_mesh";
		plane.vertices = new Vector3[]{
			new Vector3(-width, -width, 0.01f),
			new Vector3(width, -width, 0.01f),
			new Vector3(width, width, 0.01f),
			new Vector3(-width, width, 0.01f)
		};

		plane.uv = new Vector2[] {
			new Vector2 (0, 0),
			new Vector2 (0, 1),
			new Vector2(1, 1),
			new Vector2 (1, 0)
		};

		plane.triangles = new int[] { 0, 1, 2, 0, 2, 3};
		plane.RecalculateNormals();

		GameObject obj = new GameObject("actionViewer_billboard");
		obj.AddComponent<MeshFilter>();
		obj.AddComponent<MeshRenderer>();
		obj.GetComponent<MeshFilter>().mesh = plane;
		obj.GetComponent<MeshRenderer>().material.mainTexture = tex;
		obj.transform.Translate(gameObject.transform.position);
		obj.transform.Translate(new Vector3(0.0f, 1.0f, 0.0f));

		return obj;
	}
}

using UnityEngine;
using System.Collections;

public class meshDeformer : MonoBehaviour 
{
	public static GameObject 					_prefab; // Instance


	public string 								prefabNameInEditor = "mesh1";
	public 	Transform 							reference;
	public static Mesh 							myMesh;
	public static Vector3[] 					originalPoints;
	Vector3[]									currentPoints;
	public static Vector3[] 					newVert;

	public bool 							    bOSC;
	[Header("Simple Setting")]
	[Range(1.00f, 1.50f)] public float 			mainScale = 0.0f;

	[Header("Destruction Settings")]
	[Range(0.00f,10.0f)] public float 			_noiseScale = 1.000f;
	[Range(0,10.0f)] public float 				heightScale = 1.0f;
	public float 								timeLerp = 1;			

	private float 								noiseAnimXY = 0.02f;
	float 										PnPos;
	float								        SinPos;

	[Header("Speed Settings")]
	 private float 									speed = 0.1f; // camera
	 private float 									smoothTime = 0.1f; // camera
	 public float 									_timeDumpener;

	[Header("Displacement Settings")]
	[Range(0.00f, 1.0f)]public  float 			    _displacementX;
	[Range(0.00f, 1.0f)]public  float 				_displacementY;
	[Range(0.00f, 1.0f)]public  float 				_displacementZ;
	[Range(0.00f, 1.0f)]public float 				displacementAmount;


	private Vector3 								startingPos;
	public static Vector3 							displacement;
	public int										changeMeshRunTime = 1;



	void Mover () 
	{
		Vector3 desirePos = startingPos + displacement * Mathf.Sin( Time.time * speed );
		reference.transform.position = Vector3.Lerp(reference.transform.position, desirePos, smoothTime);

		displacement.x = _displacementX;
		displacement.y = _displacementY;
		displacement.z = _displacementZ;
	}

	void Start () 
	{
		_prefab = GameObject.Find("mesh1");

		startingPos = reference.transform.position;
		_prefab = GameObject.Find(prefabNameInEditor);
		myMesh = _prefab.GetComponent<MeshFilter>().mesh;
		originalPoints = new Vector3[ myMesh.vertexCount ];
		currentPoints = new Vector3[myMesh.vertexCount];

		myMesh.MarkDynamic();

		for( int i = 0; i< myMesh.vertexCount; i++ )
		{
			originalPoints[i] = new Vector3 ( myMesh.vertices[i].x, myMesh.vertices[i].y, myMesh.vertices[i].z );
		}

	}

	void Update () 
	{
		
		Vector3 refPoints = reference.position;
		newVert = myMesh.vertices;

		for( int i = 0; i< myMesh.vertexCount; i++)
		{
			Vector3 pt = originalPoints[i];

			Vector3 direction = ( pt - refPoints );
			float timeMul = ( pt - refPoints).magnitude * _timeDumpener;
			direction.Normalize();

			PnPos = Mathf.PerlinNoise (Time.deltaTime + i * _noiseScale + noiseAnimXY, Time.deltaTime + i * _noiseScale + noiseAnimXY) * heightScale;
			SinPos = Mathf.Sin(Time.time * timeMul);

			currentPoints[i] = originalPoints [i] + (direction * SinPos * displacementAmount) + (direction * PnPos * heightScale);

			newVert[i] = Vector3.Lerp(newVert[i], currentPoints[i], Time.deltaTime * timeLerp);
		}
			

		myMesh.vertices =  newVert;
		myMesh.RecalculateNormals();

		Mover ();

		// Main Scale FBX
		_prefab.transform.localScale = new Vector3(mainScale, mainScale, mainScale);


		// OSC 
		if(bOSC)
		{
			displacementAmount = OSCReceiver.displacementAmount;
			displacement.x = OSCReceiver.displacementX;
			displacement.y = OSCReceiver.displacementY;
			displacement.z = OSCReceiver.displacementZ;

			mainScale = OSCReceiver.mainScale;
			heightScale = OSCReceiver.triangulation;

			//_timeDumpener = OSCReceiver.displacementAmount;
			_noiseScale = OSCReceiver.noiseScale;
		}



	}

	int cnt = 0;
	public int Counter(int cntEnd)
	{
		cnt ++;
		//print(cnt);
		if(cnt >= cntEnd)
		{
			//print("end");
			cnt = 0;
		}
		return cnt;
	}
		



}

using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour 
{
	private Vector3 				rndTargetCam;
	private Vector3 				curVel;
	[Range(0,1)] public float 		smoothTime = 0.1f;
	public bool 					rotate 				= true;
	[Range(0,1)] public float 		rotSpeed = 0.01f;
	public float 					rndDistZ  = 20;	


	void Start()
	{
		rndTargetCam = new Vector3(0,0,-20);
		curVel = Vector3.one;
	}

	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.T) || (OSCReceiver.bCamMov == 1.0f) )	
		{			
			rndTargetCam = new Vector3(Random.Range(-rndDistZ,rndDistZ), Random.Range(-rndDistZ,rndDistZ), Random.Range(3,rndDistZ));
			//Debug.LogError("OSCReceiver.bCamMov : " + OSCReceiver.bCamMov);
		}

		smoothTime = OSCReceiver.smoothTime;
		
		this.transform.position = Vector3.SmoothDamp(this.transform.position, rndTargetCam, ref curVel, smoothTime);
		transform.LookAt(meshDeformer._prefab.transform);

		if(rotate)
			meshDeformer._prefab.transform.Rotate(0, rotSpeed, 0);

	}




}

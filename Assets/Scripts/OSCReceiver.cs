using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class OSCReceiver : MonoBehaviour {
	
	public string			RemoteIP 				= "127.0.0.1";
	public int 				ListenerPort 			= 13001;
	public int 				SendToPort 				= 57131;

	private Osc 			handler;
	private UDPPacketIO udp;

	// MAIN SETTING
	public static float 	mainScale;

	// RANDOM ACTION
	public static float 	bCamMov;
	public static float 	rndTimeDumpener;
	// DESTRUCTION SETTING
	public static float		noiseScale; 
	public static float 	heightScale;
	public static bool		bDestruction;
	// SPEED SETTING
	public static float 	speed;
	public static float 	smoothTime;
	public static float 	timeDumpener;

	// DISPLACEMENT SETTING
	public static float 	displacementX, displacementY, displacementZ, displacementAmount;
	public static float 	triangulation;

	// OPEN GL
	private float 			wireframe = 0;
	private float 			clearColor = 0;
	public bool 			w = false;
	public bool				_clearColor = false;


	void Start () 
	{	
		udp = gameObject.AddComponent<UDPPacketIO>() as UDPPacketIO;
		udp.init(RemoteIP, SendToPort, ListenerPort);
		handler = gameObject.AddComponent<Osc>() as Osc;
		handler.init(udp);

		handler.SetAddressHandler("/unity/mainScale", ListenEvent);
		handler.SetAddressHandler("/unity/displacementAmount", ListenEvent);
		handler.SetAddressHandler("/unity/displacementX", ListenEvent);	
		handler.SetAddressHandler("/unity/displacementY", ListenEvent);	
		handler.SetAddressHandler("/unity/displacementZ", ListenEvent);	
		handler.SetAddressHandler("/unity/triangulation", ListenEvent);	
		handler.SetAddressHandler("/unity/noiseScale", ListenEvent);

		handler.SetAddressHandler("/unity/bCamMov", ListenEvent);
		handler.SetAddressHandler("/unity/smoothTimeCam", ListenEvent);	

		handler.SetAddressHandler("/unity/wireframe", ListenEvent);	
		handler.SetAddressHandler("/unity/clearColor", ListenEvent);
	}
	
	public void ListenEvent(OscMessage oscMessage)
	{	
		string address = oscMessage.Address;

		//  **********************************************************************   ANDREA  **********************************************************************
		if(address == "/unity/mainScale")
		{
			mainScale = (float)System.Convert.ToSingle(oscMessage.Values[0]);
			//print((float)System.Convert.ToSingle(oscMessage.Values[0]));
		}


		//  **********************************************************************   MAURIZIO  **********************************************************************
		if(address == "/unity/displacementAmount")
			displacementAmount = (float)System.Convert.ToSingle(oscMessage.Values[0]);
		
		if(address == "/unity/displacementX")
			displacementX = (float)System.Convert.ToSingle(oscMessage.Values[0]);

		if(address == "/unity/displacementY")
			displacementY = (float)System.Convert.ToSingle(oscMessage.Values[0]);

		if(address == "/unity/displacementZ")
			displacementZ = (float)System.Convert.ToSingle(oscMessage.Values[0]);
		


		// **********************************************************************   FRANCESCO  **********************************************************************
		if(address == "/unity/triangulation")
			triangulation = (float)System.Convert.ToSingle(oscMessage.Values[0]);
	
		if(address == "/unity/noiseScale")
			noiseScale = (float)System.Convert.ToSingle(oscMessage.Values[0]);
		
		if(address == "/unity/bCamMov")
			bCamMov = (float)System.Convert.ToSingle(oscMessage.Values[0]);
		
		if(address == "/unity/smoothTimeCam")
			smoothTime = (float)System.Convert.ToSingle(oscMessage.Values[0]);

		// OPENGL WIREFRAME
		if(address == "/unity/wireframe")
		{
			wireframe = (float)System.Convert.ToSingle(oscMessage.Values[0]);
			w = (bool)System.Convert.ToBoolean(wireframe);
			Debug.LogError("wireframe : " + w);
		}

		if(address == "/unity/clearColor")
		{
			clearColor = (float)System.Convert.ToSingle(oscMessage.Values[0]);
			_clearColor = (bool)System.Convert.ToBoolean(clearColor);
			Debug.LogError("clearColor : " + _clearColor);
		}

	} 

	void Update()
	{
		//w = (bool)System.Convert.ToBoolean(wireframe);
		//_clearColor = (bool)System.Convert.ToBoolean(clearColor);

	}

	void OnPreRender() 
	{
		GL.wireframe = w;
		GL.Clear(true, _clearColor, new Color(0,0,0));
		//GL.ClearWithSkybox(_clearColor, Camera.main);
		//Debug.LogError("wireframe : " + w + " _clearColor : " + _clearColor);
	}

	void OnPostRender() 
	{
		GL.wireframe = false;
	}
}
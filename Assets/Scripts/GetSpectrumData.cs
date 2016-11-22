using UnityEngine;
using System.Collections;

/**
 * Roberto Fazio Studio 07.2014
 * Raw Microphone Sound Analisys 
 *
 * http://answers.unity3d.com/questions/394158/real-time-microphone-line-input-fft-analysis.html
 * http://answers.unity3d.com/questions/157940/getoutputdata-and-getspectrumDatadata-they-represent-t.html
 * http://forum.unity3d.com/threads/119595-Using-device-microphone-to-interact-with-objects
 * http://www.kaappine.fi/tutorials/fundamental-frequencies-and-detecting-notes/
 * 
 */

public class GetSpectrumData : MonoBehaviour 

{
	public static int 			deviceNum				= 0; 			// Select your input device number starting from 0 // iOS is set to 0
	public static int		    SAMPLECOUNT				= 1024;
	public int 					samplerate 				= 11024;
	public string 				CurrentAudioInput 		= "none";	
	public static float			loudness				= 0.0f;
	public float        		frequency				= 200.0f;
	public float				sensitivity				= 500.0f;
	public static float[]		spectrumData;							// Array of freq
	
	void Start () 
	{	
		// all audio devices available 
		string[] inputDevices = new string[Microphone.devices.Length];

		for(int i = 0; i < Microphone.devices.Length; i++)
		{	// print all 
			inputDevices[i] = Microphone.devices[i].ToString();
			//Debug.Log ("Audio Device : " + inputDevices[i]);
		}
						
		CurrentAudioInput = inputDevices[deviceNum].ToString();
		spectrumData = new float[SAMPLECOUNT];

		// add current mic device to AudioSource component
		//GetComponent<AudioSource>().clip = Microphone.Start (Microphone.devices [deviceNum], true, 999, samplerate);

		//HACK  - Wait for microphone to be ready, you can use this to control latency
		//while (!(Microphone.GetPosition(Microphone.devices[deviceNum]) > 0))
		//GetComponent<AudioSource>().Play();

	}
	
	void Update() 
	{
		// Returns a block of the currently playing source's output data.
		GetComponent<AudioSource>().GetOutputData(spectrumData, 0);
		GetComponent<AudioSource>().GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);
		loudness = GetAveragedVolume () * sensitivity ;
		frequency = GetFundamentalFrequency ();

		if(!GetComponent<AudioSource>().isPlaying)
		GetComponent<AudioSource>().Play();
				
		
	//	this.transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, ( spectrumData[Mathf.Abs((int)freq)] * sc ), Time.time * decay), 
	//	                                        Mathf.Lerp(transform.localScale.y, ( spectrumData[Mathf.Abs((int)freq)] * sc ), Time.time * decay), 
	//	                                        Mathf.Lerp(transform.localScale.z, ( spectrumData[Mathf.Abs((int)freq)] * sc ), Time.time * decay ) );

		//Debug.Log (loudness);
	}

	public float GetAveragedVolume() 
	{
		float a = 0;
		foreach (float s in spectrumData) 
		{
			a += Mathf.Abs (s);
		}
		return a / 256;
	}

	float GetFundamentalFrequency() 
	{
		float fundamentalFrequency = 0.0f;
		float s = 0.0f;
		int i = 0;
		for (int j = 1; j < SAMPLECOUNT; j++) 
		{
			if (s < spectrumData [j]) 
			{
				s = spectrumData [j];
				i = j;
			}
		}
		fundamentalFrequency = i * samplerate / SAMPLECOUNT;
		return fundamentalFrequency;
	}
}

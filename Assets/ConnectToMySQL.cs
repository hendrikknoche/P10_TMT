﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System;

public class ConnectToMySQL : MonoBehaviour {
	
	[SerializeField]
	private LoggingManager loggingManager;

	private string url;

	[SerializeField]
	private myURL serverUrl;
	private string securityCode;
	public static string response = "";
	public static bool dataReceived = false;
	public static ConnectToMySQL instance = null;

	private static bool isConnected = false;
	private int retries = 0;
	//private Dictionary<string, string> wwwHeader = new Dictionary<string, string> ();
	private string hash;

	void Awake() {
		// Setting the headers in order to get through the security of the server
		//wwwHeader["Accept"] = "*/*";
		//wwwHeader["Accept-Encoding"] = "gzip, deflate";
		//wwwHeader["User-Agent"] = "runscope/0.1";

		securityCode = serverUrl.GetSecurityCode ();
		//string[] splitArray = serverUrl.text.Split(char.Parse(","));
		//url = "http://" + splitArray [0] + "/" + splitArray [1] + "/" + splitArray [2];
		url = serverUrl.GetServerUrl();
		WWWForm testForm = new WWWForm ();
		testForm.AddField ("purposePost", "connectionTest");
		hash = Utils.Md5Sum (securityCode);
		testForm.AddField ("hashPost", hash);

		if (instance == null) {
			instance = this;
			if(!isConnected) {
				StartCoroutine(ConnectToServer (testForm));
			}
		}
	}

	IEnumerator ConnectToServer(WWWForm form) {

		if (url != "") {
			WWW www = new WWW (url, form);

			yield return www;

			if(www.error == null) {
				print ("Connected to Server");
				isConnected = true;
			} else {
				Debug.LogError(www.error);
				yield return new WaitForSeconds(2.0f);
				retries++;
				if (retries < 3) {
					StartCoroutine (ConnectToServer (form));
				}
			}
		} else {
			Debug.LogError("No URL was assigned");
		}
	}

	public void UploadLog(List<string> input) {
		WWWForm form = new WWWForm ();

		form.AddField ("purposePost", "submitLogs");
		form.AddField ("hashPost", hash);

		// Create a string with the data
		string data = "";
		for(int i = 0; i < input.Count; i++) {
			if(i != 0) {
				data += ";";
			}
			data += input[i];
		}
		Debug.Log ("data to submit: " + data);
		form.AddField ("dataPost", data);

		StartCoroutine (SubmitLogs (form));
	}

	IEnumerator SubmitLogs(WWWForm form) {
		Debug.Log ("Submitting logs..");
		WWW www = new WWW (url, form);

		yield return www;

		if (www.error == null) {
			Debug.Log ("Posted successfully");
		} else {
			Debug.Log ("log submission error: " + www.error);
			Debug.LogError ("Dumping Log To Disk For Later Uploading");
			loggingManager.DumpCurrentLog ();
		}
		loggingManager.ClearLogEntries ();
	}
		
	//public void UploadData(string playerID, string playerOrGuest, string date, string currentHitX) {

		/*for(int i = 0; i < input.Count; i++) {
			if(i != 0) {
				data += ";";
			}
			data += input[i].uid + "," + input[i].data + "," + input[i].intensity + "," + input[i].modality;
		}*/

	//	StartCoroutine (SubmitLogs (playerID, playerOrGuest, date, currentHitX));
	//}
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour {


	[SerializeField]
	private MainMenuScreen mainMenuScreen;

	[SerializeField]
	private GameObject mainMenuCanvas;

	[SerializeField]
	private GameObject firstTimeCanvas;

	private string currentProfileID = "Gæst";
	private string currentName;
	private string currentEmail;
	private bool shouldUpload;
	private bool shouldProtectSettings;
	private List<string> profiles = new List<string> ();
	private string sep = ";";


	public void Awake() {
		string profileString = PlayerPrefs.GetString ("Settings:ProfileIDs", "-1");

		if (profileString == "-1")
		{
			mainMenuCanvas.SetActive(false);
			firstTimeCanvas.SetActive(true);
			profileString = "Gæst";
		}

		if (profileString.Contains (sep)) {
			string[] profilesArray = profileString.Split (char.Parse (sep));
			for (int i = 0; i < profilesArray.Length; i++) {
				profiles.Add (profilesArray [i]);
			}
		} else {
			profiles.Add (profileString);
		}

		currentProfileID = PlayerPrefs.GetString ("Settings:CurrentProfileID", "Gæst");
		SetCurrentProfile (currentProfileID);

	}

	public void AddNewProfile(string name, string email, bool uploadPolicy, bool protectSettings) {
		
		if (name != "") {
			currentName = name;
		} else {
			currentName = "Gæst";
		}

		if (email != "") {
			currentEmail = email;
		} else {
			currentEmail = "No Email";
		}

		string newProfileID = Utils.Md5Sum(currentName + currentEmail);
		currentProfileID = newProfileID;
		shouldUpload = uploadPolicy;
		shouldProtectSettings = protectSettings;
		profiles.Add (newProfileID);
		Debug.Log ("Adding " + newProfileID + "to profiles list with shouldUpload " + uploadPolicy);
		SaveProfiles ();
	}

	public string GetCurrentName()
	{
		return currentName;
	}

	public string GetCurrentEmail()
	{
		return currentEmail;
	}

	public bool GetUploadPolicy()
	{
		return shouldUpload;
	}

	public bool GetProtectSettings()
	{
		return shouldProtectSettings;
	}

	public string GetCurrentProfileID()
	{
		return currentProfileID;
	}

	public int GetProfileAmount()
	{
		int profileAmount = profiles.Count;
		return profileAmount;
	}

	public List<string> GetProfileList()
	{
		return profiles;
	}

	public void SetCurrentProfile(string newProfileID)
	{
		currentProfileID = newProfileID;
		currentName = PlayerPrefs.GetString("Settings:" + newProfileID + ":Name", "Gæst");
		currentEmail = PlayerPrefs.GetString ("Settings:" + newProfileID + ":Email", "No Email");
		shouldUpload = (PlayerPrefs.GetInt("Settings:" + newProfileID + ":UploadData", 0) == 1);
		shouldProtectSettings = (PlayerPrefs.GetInt("Settings:" + newProfileID + ":ProtectSettings", 0) == 1);
		Debug.Log ("current profile set as id " + currentProfileID + " with name " + currentName + " and shouldUpload " + shouldUpload);

		//mainMenuScreen.setWelcomeText (currentName);
		// Call SettingsScreen.UpdateSettings(profileID); or just make it read the CurrentProfileID.
		SaveProfiles ();
	}

	public void RemoveProfile(string idToRemove) {
		List<string> newProfileList = new List<string>();
		foreach (var id in profiles) {

			if (id == idToRemove)
			{
				Debug.Log("id " + id + "found and removed");
			}
			if (id != idToRemove) {
				newProfileList.Add(id);
			}
		}
		profiles = newProfileList;
	}

	public void SaveProfiles()
	{
		string[] profileArray = profiles.ToArray ();
		string profileString = string.Join (sep, profileArray);
		PlayerPrefs.SetString ("Settings:ProfileIDs", profileString);
		PlayerPrefs.SetString ("Settings:CurrentProfileID", currentProfileID);
		PlayerPrefs.SetString ("Settings:" + currentProfileID + ":Name", currentName);
		PlayerPrefs.SetString ("Settings:" + currentProfileID + ":Email", currentEmail);
		PlayerPrefs.SetInt("Settings:" + currentProfileID + ":UploadData", int.Parse((Utils.BoolToNumberString(shouldUpload))));
		PlayerPrefs.SetInt("Settings:" + currentProfileID + ":ProtectSettings", int.Parse((Utils.BoolToNumberString(shouldProtectSettings))));
		PlayerPrefs.SetString ("Settings:Name", currentName);
		PlayerPrefs.SetString ("Settings:Email", currentEmail);
		PlayerPrefs.SetInt ("Settings:UploadData", int.Parse((Utils.BoolToNumberString(shouldUpload))));
		PlayerPrefs.SetInt ("Settings:ProtectSettings", int.Parse((Utils.BoolToNumberString(shouldProtectSettings))));
	}
}

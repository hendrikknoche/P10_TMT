﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using UnityEngine.UI;

public class SettingsScreen : MonoBehaviour
{
    [SerializeField]
    private InputField passwordInput;
    private const string password = "hello";
    [SerializeField]
    private Sprite fra;
    [SerializeField]
    private Sprite til;

	[SerializeField]
	private GameObject settingsPanel;

	[SerializeField]
	private GameObject passwordPanel;

	[SerializeField]
	private GameObject wrongPassword;

	[SerializeField]
	private ProfileManager profileManager;

    [SerializeField]
    private Image landingsbaneToggle;
    [SerializeField]
    private Image pulseToggle;
    [SerializeField]
    private Image stemmeToggle;

	[SerializeField]
	private GameObject gentagStemmeToggle;

	[SerializeField]
	private Image introToggle;

	[SerializeField]
	private Text trainingMinutes;
	[SerializeField]
	private UnityEngine.UI.Slider trainingSlider;
	[SerializeField]
	private Text circleAmountNumber;
	[SerializeField]
	private UnityEngine.UI.Slider circleAmountSlider;

	[SerializeField]
	private Image gameAButton;
	[SerializeField]
	private Image gameBButton;

	[SerializeField]
	private Sprite selectedButtonSprite;

	[SerializeField]
	private Sprite regularButtonSprite;

    private bool landingsbane;
    private bool pulse;
    private bool stemme;
	private bool gentagStemme;
	private bool intro;
	private int trainingTime = 8;
	private int difficultyLevel = 3;
	private int minimumLevel = 7;
	private int maximumLevel = 13;
	private int circleAmountMin = 1;
	private int circleAmountMax = 2;
	private string circleAmountNumberTemplate;
	private string trainingMinutesTemplate;
	private string gameType;

    private void OnEnable()
    {

        LoadSettings();
        landingsbaneToggle.sprite = landingsbane ? til : fra;
        pulseToggle.sprite = pulse ? til : fra;
        stemmeToggle.sprite = stemme ? til : fra;
		introToggle.sprite = intro ? til : fra;
		gentagStemmeToggle.SetActive (stemme);
		gentagStemmeToggle.GetComponent<Toggle> ().isOn = gentagStemme;
		if (gameType == "gameA") {
			gameAButton.sprite = selectedButtonSprite;
			gameBButton.sprite = regularButtonSprite;
		} else {
			gameBButton.sprite = selectedButtonSprite;
			gameAButton.sprite = regularButtonSprite;
		}
		trainingSlider.value = (float) trainingTime;
		circleAmountSlider.value = (float) difficultyLevel;
		if (trainingMinutesTemplate == null) {
			
			trainingMinutesTemplate = trainingMinutes.text;
		}
		if (circleAmountNumberTemplate == null) {
			circleAmountNumberTemplate = circleAmountNumber.text;
		}
		trainingMinutes.text = string.Format (trainingMinutesTemplate, trainingTime.ToString());
		circleAmountNumber.text = string.Format (circleAmountNumberTemplate, circleAmountMin.ToString(),circleAmountMax.ToString());
    }

	private void DetermineAmountOfCircles () {
		switch(difficultyLevel) {
		case 1:
			circleAmountMin = 10;
			circleAmountMax = 15;
			break;
		case 2:
			circleAmountMin = 20;
			circleAmountMax = 30;
			break;
		case 3:
			circleAmountMin = 35;
			circleAmountMax = 40;
			break;
		default:
			circleAmountMin = 1;
			circleAmountMax = 2;
			break;
		}			
	}

	private void DetermineMinMaxLevel () {
		switch(difficultyLevel) {
		case 1:
			minimumLevel = 1;
			maximumLevel = 4;
			break;
		case 2:
			minimumLevel = 7;
			maximumLevel = 13;
			break;
		case 3:
			minimumLevel = 18;
			maximumLevel = 27;
			break;
		default:
			minimumLevel = 1;
			maximumLevel = 2;
			break;
		}		
	}

    public void Landingsbane_Click()
    {
        landingsbane = !landingsbane;
        landingsbaneToggle.sprite = landingsbane ? til : fra;
        SaveSettings();
    }

    public void Animation_Click()
    {
        pulse = !pulse;
        pulseToggle.sprite = pulse ? til : fra;
        SaveSettings();
    }

    public void Stemme_Click()
    {
        stemme = !stemme;
        stemmeToggle.sprite = stemme ? til : fra;
		gentagStemmeToggle.SetActive (stemme ? true : false);
        SaveSettings();
    }

	public void GentagStemme_Click()
	{
		gentagStemme = !gentagStemme;
		SaveSettings();
	}

	public void Intro_Click()
	{
		intro = !intro;
		introToggle.sprite = intro ? til : fra;
		SaveSettings();
	}

	public void TrainingTimeSlider_OnValueChanged()
	{
		int val = (int) trainingSlider.value;
		if (trainingMinutesTemplate != null) {
			trainingTime = val;
			trainingMinutes.text = string.Format (trainingMinutesTemplate, val.ToString ());
			SaveSettings ();
		}
	}

	public void CircleSlider_OnValueChanged()
	{
		int val = (int) circleAmountSlider.value;
		if (circleAmountNumberTemplate != null) {
			difficultyLevel = val;
			DetermineAmountOfCircles ();
			circleAmountNumber.text = string.Format (circleAmountNumberTemplate, circleAmountMin.ToString (), circleAmountMax.ToString ());
			DetermineMinMaxLevel ();
			SaveSettings ();
		}
	}

	public void switchToGameA() {
		gameAButton.sprite = selectedButtonSprite;
		gameBButton.sprite = regularButtonSprite;
		gameType = "gameA";
		SaveSettings ();
	}

	public void switchToGameB() {
		gameBButton.sprite = selectedButtonSprite;
		gameAButton.sprite = regularButtonSprite;
		gameType = "gameB";
		SaveSettings ();
	}

    /// <summary>
    /// Load the settings from PlayerPrefs
    /// </summary>
    public void LoadSettings()
    {
		string currentProfileID = profileManager.GetCurrentProfileID();

        landingsbane = PlayerPrefs.GetInt("Settings:" + currentProfileID + ":Landingsbane", 0) == 1;
        pulse = PlayerPrefs.GetInt("Settings:" + currentProfileID + ":Pulse", 0) == 1;
        stemme = PlayerPrefs.GetInt("Settings:"+ currentProfileID + ":Stemme", 0) == 1;
		gentagStemme = PlayerPrefs.GetInt("Settings:"+ currentProfileID + ":GentagStemme", 0) == 1;
		intro = PlayerPrefs.GetInt("Settings:" + currentProfileID + ":Intro", 1) == 1;
		trainingTime = PlayerPrefs.GetInt("Settings:"+ currentProfileID +":Time", 2);
		gameType = PlayerPrefs.GetString ("Settings:" + currentProfileID + ":GameType", "gameA");
		difficultyLevel = PlayerPrefs.GetInt("Settings:"+ currentProfileID + ":DifficultyLevel", 1);
		DetermineMinMaxLevel();
		DetermineAmountOfCircles ();
    }

    /// <summary>
    /// Save the current settings to PlayerPrefs
    /// </summary>
    public void SaveSettings()
    {
		string currentProfileID = profileManager.GetCurrentProfileID();

        PlayerPrefs.SetInt("Settings:"+currentProfileID +":Landingsbane", landingsbane ? 1 : 0);
		PlayerPrefs.SetInt("Settings:"+currentProfileID +":Pulse", pulse ? 1 : 0);
		PlayerPrefs.SetInt("Settings:"+currentProfileID +":Stemme", stemme ? 1 : 0);
		PlayerPrefs.SetInt("Settings:"+currentProfileID +":GentagStemme", gentagStemme ? 1 : 0);
		PlayerPrefs.SetInt("Settings:"+currentProfileID +":Intro", intro ? 1 : 0);
		PlayerPrefs.SetInt ("Settings:"+currentProfileID +":Time", trainingTime);
		PlayerPrefs.SetInt ("Settings:"+currentProfileID +":DifficultyLevel", difficultyLevel);
		PlayerPrefs.SetInt ("Settings:"+currentProfileID +":MinLevel", minimumLevel);
		PlayerPrefs.SetInt ("Settings:"+currentProfileID +":MaxLevel", maximumLevel);
		PlayerPrefs.SetString ("Settings:" + currentProfileID + ":GameType", gameType);
    }

	public void CheckPassword() {
		if (!profileManager.GetProtectSettings () || passwordInput.text == password) {
			settingsPanel.SetActive (true);
			passwordPanel.SetActive (false);
			passwordInput.text = "";
		} else if (passwordInput.text != "") {
			wrongPassword.SetActive (true);
			passwordPanel.SetActive (true);
			settingsPanel.SetActive (false);
		} else {
			passwordPanel.SetActive (true);
			settingsPanel.SetActive (false);
		}

	}
}

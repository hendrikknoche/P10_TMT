﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fadein : MonoBehaviour {

	public float duration;
	public float startDelay;
	[SerializeField]
	private bool startNow = true;
	private float f = 0f;
	private bool fadeIn = false;
	private bool startOnAwake = false;
	private float startTime;
	private Image[] fadeImages;
	private Text[] fadeTexts;

	public bool fadeInChildren = false;

	// Use this for initialization
	void OnEnable()
	{
		this.ResetFade();
		if (startOnAwake)
		{
			startNow = true;
		}
	}

	void Start () {

		if (fadeInChildren)
		{
			fadeImages = this.GetComponentsInChildren<Image>();
			fadeTexts = this.GetComponentsInChildren<Text>();
		}
		else
		{
			fadeImages = new Image[1];
			fadeImages[0] = this.GetComponent<Image>();
			fadeTexts = new Text[1];
			fadeTexts[0] = this.GetComponent<Text>();
		}
		this.ResetFade ();
		if (startNow)
		{
			startOnAwake = true;
		}
		//Debug.Log (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (!startNow) {
			startTime = Time.fixedTime;
			return;
		}

		if (startDelay < (Time.fixedTime - startTime)) {
			fadeIn = true;
		}
		//Debug.Log(Time.fixedTime - startTime + " seconds passed");

		if (fadeIn) {
			if (fadeImages != null)
			{
				foreach (Image img in fadeImages)
				{
					if (img != null)
					{
						img.color = new Color(img.color.r, img.color.g, img.color.b, f);
					}
				}
			}

			if (fadeTexts != null)
			{
				foreach (Text text in fadeTexts)
				{
					if (text != null)
					{
						text.color = new Color(text.color.r, text.color.g, text.color.b, f);
					}
				}
			}

			if (f >= 1)
			{
				fadeIn = false;
				startNow = false;
				f = 0;
				//Debug.Log("fading stopped!: " + f);
			}

			f += Time.deltaTime / duration;
			//Debug.Log("alpha is: " + f);
		}
	}

	public void StartFade()
	{
		startNow = true;
	}

	public void ResetFade() {
		f = 0f;
		fadeIn = false;

		if (fadeImages != null) {
			foreach (Image img in fadeImages) {
				if (img != null) {
					img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
				}
			}
		}

		if (fadeTexts != null) {
			foreach (Text text in fadeTexts) {
				if (text != null) {
					text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
				}
			}
		}

		startTime = Time.fixedTime;
	}

}

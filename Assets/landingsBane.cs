﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class landingsBane : MonoBehaviour {

	public bool directionRight = true;
	public float arrowWidth = 136f;
	public float arrowOffset = 0;
	public float speed = 4f;
	private new RectTransform transform;
	private Image sprite;
	private Vector2 resetOffsetMin;
	private Vector2 resetOffsetMax;
	private Vector2 anchoredPositionMin;
	private float maxMove;
	private float minMove;


	// Use this for initialization
	void Start () {
		Canvas.ForceUpdateCanvases ();
		transform = this.GetComponent<RectTransform> ();
		sprite = this.GetComponent<Image> ();

		resetOffsetMin = transform.offsetMin;
		resetOffsetMax = transform.offsetMax;
		anchoredPositionMin = transform.anchoredPosition;
		transform.anchoredPosition = new Vector2();

		if (directionRight) {
			transform.offsetMin = new Vector2 (transform.offsetMin.x + (arrowWidth * arrowOffset), transform.offsetMin.y);
		} else {
			transform.offsetMax = new Vector2 (transform.offsetMax.x - (arrowWidth * arrowOffset), transform.offsetMax.y);
			transform.anchoredPosition = new Vector2(transform.anchoredPosition.x - (arrowWidth * arrowOffset), transform.anchoredPosition.y);
		}
		sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
	}
	
	// Update is called once per frame
	void Update () {
		DrawArrow ();
	}

	void DrawArrow() {

		if (directionRight) {
			transform.offsetMin += new Vector2 (speed, 0f);

			//Debug.Log ("current: " + transform.offsetMin.x + " width: " + transform.rect.width);

			if (transform.rect.width <= 0) {
				transform.offsetMin = resetOffsetMin;
			}
		} else {
			transform.offsetMax -= new Vector2 (speed, 0f);
			transform.anchoredPosition -= new Vector2(speed, 0f);

			//Debug.Log ("current: " + transform.offsetMax.x + " width: " + transform.rect.width);

			if (transform.rect.width <= 0) {
				transform.offsetMax += new Vector2(resetOffsetMax.x, 0f);	
				transform.anchoredPosition = anchoredPositionMin;
			}

		}
			
		//transform.rect = new Rect(transform.rect.xMin, transform.rect.yMin, transform.rect.xMax - (transform.rect.xMin + 0.1f), transform.rect.yMax - transform.rect.yMin);
	}
}

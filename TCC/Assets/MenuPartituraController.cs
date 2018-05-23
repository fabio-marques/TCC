﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPartituraController : MonoBehaviour {

	Transform partiturasGroup;
	GameObject[] partituras;
	bool menuAberto = false;
	bool apertouDirecional = false;
	int currIndex = 0;

	bool canPressLTrigger = true;

	// Use this for initialization
	void Start () {
		partiturasGroup = transform.GetChild (0);
		partituras = new GameObject[partiturasGroup.childCount];

		for (int i = 0; i < partituras.Length; i++) {
			partituras [i] = partiturasGroup.GetChild (i).gameObject;
			partituras [i].SetActive (false);
		}

		partiturasGroup.gameObject.SetActive (menuAberto);
	}
	
	// Update is called once per frame
	void Update () {
		bool oldState = menuAberto;

		if ((Input.GetAxis ("L_Trigger") != 0 && canPressLTrigger) || Input.GetKeyDown (KeyCode.Tab)) {
			menuAberto = !menuAberto;
			partiturasGroup.gameObject.SetActive (menuAberto);
			canPressLTrigger = false;
		} else if (Input.GetAxis ("L_Trigger") == 0) {
			canPressLTrigger = true;
		}

//		if(!menuAberto){
//			currIndex = 0;
//		} 
		if (menuAberto) {
			int oldIndex = currIndex;

			#region DirectionInput
			if(Input.GetAxis("DPad_X") <= -0.1f || Input.GetKeyDown(KeyCode.E)){
				if (!apertouDirecional) {
					apertouDirecional = true;
					currIndex++;
				}
			} else if (Input.GetAxis("DPad_X") >= 0.1f || Input.GetKeyDown(KeyCode.Q)) {
				if (!apertouDirecional) {
					apertouDirecional = true;
					currIndex--;
				}
			} else {
				apertouDirecional = false;
			}
			#endregion

			if (currIndex >= partituras.Length)
				currIndex = 0;
			else if (currIndex < 0)
				currIndex = partituras.Length - 1;

			if(oldIndex != currIndex || oldState == false){
				for (int i = 0; i < partituras.Length; i++) {
					partituras [i].SetActive (false);
				}
			}

			partituras [currIndex].SetActive (true);
		}
	}
}

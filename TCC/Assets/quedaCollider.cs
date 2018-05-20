﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quedaCollider : MonoBehaviour {
	private Vector3 posicaoInicial;
	private Vector3 posicaoQueda;
	public GameObject player;
	public WayPointQueda wayPoint;
	public TranslateObject movePai;
	// Use this for initialization
	void Start () {
		posicaoInicial = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.H)){
			player.transform.position = posicaoInicial;
			movePai.startMove = false;
		}
	}

	void OnTriggerEnter(Collider colisor){
		if(colisor.name == "PlayerCollider"){
			posicaoQueda = colisor.transform.position;
			wayPoint.GetPlayerPosition (posicaoQueda);
			movePai.StartMove();
			print ("colisaoPlayer");

		}
	}

}
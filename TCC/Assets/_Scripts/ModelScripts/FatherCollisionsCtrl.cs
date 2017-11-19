﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatherCollisionsCtrl : MonoBehaviour {
	
	private AgentLinkMover linkMover;
	private FatherPath fatherPath;
	private FatherHeightCtrl fatherHeight;
	private FatherSingCtrl fatherSing;

	void Awake(){
		linkMover = GetComponent<AgentLinkMover> ();
		fatherPath = GetComponent<FatherPath> ();
		fatherHeight = GetComponent<FatherHeightCtrl> ();
		fatherSing = GetComponent<FatherSingCtrl> ();
	}

	IEnumerator WaitForChangeHeight(float strength){
		yield return new WaitForSeconds (0.25f);
		fatherHeight.UpdateHeight (strength);
	}

	void OnTriggerEnter(Collider col){
		if(col.CompareTag("Player")){
			fatherPath.state = FatherPath.FSMStates.Path;
			fatherPath.esperaFilho = false;
			//fatherPath.ChangeWaypoint (false);
		}

		if (col.CompareTag("NpcPath"))
		{
			if(col.GetComponent<FatherWPBehaviour> ().behaviour != FatherBehaviour.None)
				col.GetComponent<FatherWPBehaviour> ().StartBehaviour (fatherPath, fatherHeight, fatherSing);
//			else {
//				fatherPath.ChangeWaypoint (false);
//				int num = int.Parse(col.name.TrimStart ("WP ".ToCharArray ()));
//				fatherPath.currentWayPoint = num;
//			}
		}

		if (col.CompareTag("Pai_Sing"))
		{
			GetComponent<AudioSource> ().Play ();
		}

		if(col.CompareTag("Pai_Esticar")){
			linkMover.m_Method = OffMeshLinkMoveMethod.NormalSpeed;
			StartCoroutine ("WaitForChangeHeight", 0.9f);
		}
		if(col.CompareTag("Pai_Abaixar")){
			linkMover.m_Method = OffMeshLinkMoveMethod.NormalSpeed;
			StartCoroutine ("WaitForChangeHeight", -0.9f);
		}
		if(col.CompareTag("Pai_Fly")){
			linkMover.parabolaHeight = linkMover.parabolaHeight * 10f;
		}
	}
		
	void OnTriggerStay(Collider col){
		if(col.CompareTag("Player")){
			fatherPath.state = FatherPath.FSMStates.Path;
			fatherPath.esperaFilho = false;
			//fatherPath.ChangeWaypoint (false);
		}
	}

	void OnTriggerExit(Collider col){
//		if(col.CompareTag("Player")){
//			fatherPath.state = FatherPath.FSMStates.Path;
//			fatherPath.esperaFilho = true;
//		}

		if(col.CompareTag("Pai_Esticar")){
			linkMover.m_Method = OffMeshLinkMoveMethod.Parabola;
			StartCoroutine ("WaitForChangeHeight", 0f);
		}
		if(col.CompareTag("Pai_Abaixar")){
			linkMover.m_Method = OffMeshLinkMoveMethod.Parabola;
			StartCoroutine ("WaitForChangeHeight", 0f);
		}
		if(col.CompareTag("Pai_Fly")){
			linkMover.parabolaHeight = linkMover.parabolaHeight * 0.1f;
		}
	}
}

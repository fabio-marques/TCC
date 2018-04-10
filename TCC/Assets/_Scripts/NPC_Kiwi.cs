﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Kiwi : NPCBehaviour {

	float distToPlayer;
	float timer_StartPatrulha = 0;
	float timer_Distraido = 0;
	float timer_PegarObjeto = 0;
	Vector3 patrulhaStartPos;
	bool fugindo, patrulhando, podePegarObj;

	List<Transform> collObjects = new List<Transform> ();
	Transform objetoCarregado;

	protected override void Update ()
	{
		distToPlayer = Vector3.Distance (player.position, npcTransform.position);

		base.Update ();

		if (currentState != NPC_CurrentState.DefaultState)
			ResetDefaultBehaviour ();

		if(timer_PegarObjeto > 0f){
			timer_PegarObjeto -= Time.deltaTime;
			podePegarObj = false;
		} else {
			timer_PegarObjeto = 0f;
			podePegarObj = true;
		}
	}

	protected override void DefaultState ()
	{
		if(currentState == NPC_CurrentState.Distraido){
			Distrair ();
			return;
		}

		base.DefaultState ();

		//Normalmente, Foge do player quando este se aproxima.
		if(distToPlayer < 7f){
			patrulhando = false;
			timer_StartPatrulha = 0;
			currentSong = PlayerSongs.Empty;

			if (objetoCarregado != null)
				SoltarObjeto ();

			if(!fugindo){
				fugindo = true;
				FleeFromPlayer ();
			}
			else {
				if (!nmAgent.pathPending && !nmAgent.isStopped){
					if (nmAgent.remainingDistance <= nmAgent.stoppingDistance){
						if (!nmAgent.hasPath || nmAgent.velocity.sqrMagnitude == 0f){
							FleeFromPlayer ();
						}
					}
				}
			}

		} else {
			fugindo = false;
		}

		//Se ficar 10s parado, anda pra um lugar aleatorio perto de onde está
		if (timer_StartPatrulha >= 10f) {

			if(!patrulhando){
				patrulhando = true;
				patrulhaStartPos = npcTransform.position;
			}

			if (!nmAgent.pathPending && !nmAgent.isStopped){
				if (nmAgent.remainingDistance <= nmAgent.stoppingDistance){
					if (!nmAgent.hasPath || nmAgent.velocity.sqrMagnitude == 0f){
						if (timer_StartPatrulha < 20f) {
							Vector2 circleRand = new Vector2 (patrulhaStartPos.x, patrulhaStartPos.z) + (20f * Random.insideUnitCircle);
							Vector3 dest = new Vector3 (circleRand.x, npcTransform.position.y, circleRand.y);
							nmAgent.SetDestination (dest);
						} else {
							nmAgent.SetDestination (patrulhaStartPos);
							if (!nmAgent.pathPending && !nmAgent.isStopped) {
								if (nmAgent.remainingDistance <= nmAgent.stoppingDistance) {
									if (!nmAgent.hasPath || nmAgent.velocity.sqrMagnitude == 0f) {
										timer_StartPatrulha = 0f;
									}
								}
							}
						}
					}
				}
			}
		}

		timer_StartPatrulha += Time.deltaTime;
	}

	void FleeFromPlayer (){
		Vector3 fleeDirection = npcTransform.position + (npcTransform.position - player.position).normalized * 10f;
		Vector2 circleRand = new Vector2 (fleeDirection.x, fleeDirection.z) + (5f * Random.insideUnitCircle);
		Vector3 dest = new Vector3 (circleRand.x, npcTransform.position.y, circleRand.y);
		nmAgent.SetDestination (dest);
	}

	void ResetDefaultBehaviour (){
		patrulhando = fugindo = false;
		timer_StartPatrulha = 0;
	}

	protected override void Seguir ()
	{
		if(currentState != NPC_CurrentState.Distraido && currentState != NPC_CurrentState.Seguindo){
			DefaultState ();
			return;
		}

		base.Seguir ();
	}

	protected override void Distrair ()
	{
		//base.Distrair faz a função retornar se já estiver distraído.
		//base.Distrair ();

		if (currentState != NPC_CurrentState.Distraido){
			currentState = NPC_CurrentState.Distraido;
			timer_Distraido = 15f;
		}

		if(timer_Distraido > 0f){
			timer_Distraido -= Time.deltaTime;
		} else {
			timer_Distraido = 0f;
			currentState = NPC_CurrentState.DefaultState;
		}

	}

	protected override void Irritar ()
	{
		//TODO: O que ficar "Agitado" significa?
		//Agitado -> 10s -> Normal

		if (objetoCarregado != null)
			SoltarObjeto ();

		base.Irritar ();
	}

	protected override void Acalmar ()
	{
		if(currentState != NPC_CurrentState.Distraido && currentState != NPC_CurrentState.Calmo){
			DefaultState ();
			return;
		}

		base.Acalmar ();
	}


	IEnumerator PegarObjeto (){
		yield return new WaitForSeconds (0.25f);

		float dist = 1000f;
		int index = 0;
		for (int i = 0; i < collObjects.Count; i++) {
			float temp = Vector3.Distance (npcTransform.position, collObjects [i].position);
			if(temp < dist){
				dist = temp;
				index = i;
			}
		}

		CarregarObjeto (collObjects[index]);
	}

	void CarregarObjeto (Transform obj){
		objetoCarregado = obj;

		objetoCarregado.SetParent (npcTransform);
		objetoCarregado.localPosition = new Vector3 (0, 2, 0);
	}

	void SoltarObjeto (){
		objetoCarregado.SetParent (null);
		objetoCarregado = null;
		timer_PegarObjeto = 2f;
	}

	void OnTriggerEnter (Collider col){
		if(col.CompareTag("Fruta") || col.CompareTag("Semente") || col.CompareTag("PaiDebilitado")){
			if (objetoCarregado == null && podePegarObj) {
				if(!collObjects.Contains(col.transform)){
					collObjects.Add (col.transform);
					StopCoroutine ("PegarObjeto");
					StartCoroutine ("PegarObjeto");
				}
			}
		}
	}
	void OnTriggerExit (Collider col){
		if(col.CompareTag("Fruta") || col.CompareTag("Semente") || col.CompareTag("PaiDebilitado")){
			if(collObjects.Contains(col.transform)){
				collObjects.Remove (col.transform);
			}
		}
	}
}
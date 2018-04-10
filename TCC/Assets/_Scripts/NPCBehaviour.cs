﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class NPCBehaviour : MonoBehaviour, ISongListener {

	protected NavMeshAgent nmAgent;
	protected Transform npcTransform;
	protected Transform player;
	protected Transform father;

	[BitMaskAttribute(typeof(PlayerSongs))]
	public PlayerSongs acceptedSongs; //Definir pelo inspector com quais melodias o NPC poderá interagir.
	protected List<int> selectedSongs; //Armazena separadamente cada uma das melodias escolhidas em acceptedSongs. Ou seja, ela me permite saber quais melodias um NPC pode interagir.


		// (0) Amizade	-	-	(Seguir)	-	UPDATE
		// (1) Estorvo	-	-	(Irritar)	-	Start
		// (2) Serenidade	-	(Acalmar)	-	Start
		// (3) Ninar	-	-	(Dormir)	-	Start
		// (4) Crescimento	-	(Crescer)	-	UPDATE
		// (5) Encolhimento	-	(Encolher)	-	UPDATE
		// (6) Alegria	-	-	(Distrair)	-	Start

	[SerializeField]
	protected PlayerSongs currentSong;

	[SerializeField]
	protected NPC_CurrentState currentState;

	protected Transform currentInteractionAgent; //Player ou Pai

	float timer = 0f;

	protected virtual void Start () {
		selectedSongs = ReturnSelectedElements ();
		nmAgent = GetComponent<NavMeshAgent> ();
		npcTransform = GetComponent<Transform> ();
		player = GameObject.FindObjectOfType<WalkingController> ().transform;
		father = GameObject.FindObjectOfType<FatherFSM> ().transform;

		currentSong = PlayerSongs.Empty;
		currentState = NPC_CurrentState.DefaultState;
		currentInteractionAgent = player;

		GetComponent<Rigidbody> ().isKinematic = true;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		if(timer >= 1f){
			timer = 1f;
			currentSong = PlayerSongs.Empty;
			if (currentState == NPC_CurrentState.Seguindo)
				currentState = NPC_CurrentState.DefaultState;
		} else {
			timer += Time.deltaTime;
		}

		switch (currentSong) {
		case PlayerSongs.Amizade:
			if (selectedSongs.Contains (0))
				Seguir ();
			break;
		case PlayerSongs.Estorvo:
			if (selectedSongs.Contains (1))
				Irritar ();
			break;
		case PlayerSongs.Serenidade:
			if (!selectedSongs.Contains (2))
				Acalmar ();
			break;
		case PlayerSongs.Ninar:
			if (selectedSongs.Contains (3))
				Dormir ();
			break;
		case PlayerSongs.Crescimento:
			if (selectedSongs.Contains (4))
				Crescer ();
			break;
		case PlayerSongs.Encolhimento:
			if (selectedSongs.Contains (5))
				Encolher ();
			break;
		case PlayerSongs.Alegria:
			if (selectedSongs.Contains (6))
				Distrair ();
			break;
		default: //PlayerSongs.Empty
			if (currentState == NPC_CurrentState.Seguindo) {
				Seguir ();
			} else {
				DefaultState ();
			}
			break;
		}
	}

	public void DetectSong (PlayerSongs song, bool isFather = false){
		timer = 0f;
		currentSong = song;

		if (!isFather)
			currentInteractionAgent = player;
		else
			currentInteractionAgent = father;
	}

	//======================================================================================================================
	//=================------------------------- FUNÇÕES DE COMPORTAMENTO -------------------------=========================
	//======================================================================================================================

	protected virtual void DefaultState (){
		currentState = NPC_CurrentState.DefaultState;

	}

	protected virtual void Seguir (){
		//TODO: Se pa colocar aqui condições que se aplicam a todos, como "Só Seguir se NÃO estiver Dormindo".
		//TODO: No 'filho', adicionar condições especificas, como "Só Seguir se NÃO estiver Irritado".

		currentState = NPC_CurrentState.Seguindo;

		nmAgent.SetDestination (currentInteractionAgent.position);
	}
	protected virtual void PararDeSeguir (){
		if (currentState == NPC_CurrentState.DefaultState)
			return; //Esta função só deve roda uma vez.
		
		currentState = NPC_CurrentState.DefaultState;

		nmAgent.SetDestination (transform.position);
	}

	protected virtual void Irritar (){
		if (currentState == NPC_CurrentState.Irritado)
			return; //Esta função só deve roda uma vez.
		
		currentState = NPC_CurrentState.Irritado;
		//Faz Irritar
	}
	protected virtual void Acalmar (){
		if (currentState == NPC_CurrentState.Calmo)
			return; //Esta função só deve roda uma vez.

		currentState = NPC_CurrentState.Calmo;
		//Faz Acalmar
	}

	protected virtual void Dormir (){
		if (currentState == NPC_CurrentState.Dormindo)
			return; //Esta função só deve roda uma vez.
		
		currentState = NPC_CurrentState.Dormindo;
		//Faz Dormir
	}
	protected virtual void Acordar (){
		if (currentState == NPC_CurrentState.DefaultState)
			return; //Esta função só deve roda uma vez.
		
		currentState = NPC_CurrentState.DefaultState;
	}

	protected virtual void Crescer (){
		currentState = NPC_CurrentState.Crescendo;
		//Faz Crescer
	}
	protected virtual void Encolher (){
		currentState = NPC_CurrentState.Encolhendo;
		//Faz Encolher
	}

	protected virtual void ChamarAtencao (){
		if (currentState == NPC_CurrentState.Atento)
			return; //Esta função só deve roda uma vez.
		
		currentState = NPC_CurrentState.Atento;
	}
	protected virtual void Distrair (){
		if (currentState == NPC_CurrentState.Distraido)
			return; //Esta função só deve roda uma vez.
		
		currentState = NPC_CurrentState.Distraido;
		//Faz Distrair
	}

	//======================================================================================================================
	//======================================================================================================================
	//======================================================================================================================


	//Esta função me retorna todos os indices selecionados do Enum acceptedSongs
	List<int> ReturnSelectedElements () {
		List<int> selectedElements = new List<int>();
		for (int i = 0; i < System.Enum.GetValues(typeof(PlayerSongs)).Length; i++)
		{
			int layer = 1 << i;
			if (((int) acceptedSongs & layer) != 0)
			{
				selectedElements.Add(i);
			}
		}

		return selectedElements;
	}

	//======================================================================================================================

	public enum NPC_CurrentState
	{
		DefaultState,
		Seguindo,
		Irritado,
		Calmo,
		Dormindo,
		Crescendo,
		Encolhendo,
		Atento,
		Distraido
	}
}
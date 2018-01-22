using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Player3D))]
public class PlayerWalkInput : MonoBehaviour {

	public Player3D player;

	float jumpPressTime;
	float floreioHoldTime;
	float staccatoHoldTime;

	[HideInInspector]
	public bool playerInputStartGame;

	bool holdingJump;

	void Start() {
		player = GetComponent<Player3D> ();
		holdingJump = false;
	}

	void Update () {
		#region Check Movement
		Vector3 directionalInput = Vector3.zero;

		//Set vertical movement
		directionalInput += player.orientation.forward * Input.GetAxisRaw("L_Joystick_Y");
		directionalInput += player.orientation.forward * Input.GetAxisRaw("Vertical");

		//Set horizontal movement
		directionalInput += player.orientation.right * Input.GetAxisRaw("L_Joystick_X");
		directionalInput += player.orientation.right * Input.GetAxisRaw("Horizontal");

		player.SetDirectionalInput (directionalInput);
		#endregion

		#region Check Sanfona
		//Set Sanfona
		float sanfonaStrength = Input.GetAxisRaw("R_Joystick_Y") + Input.GetAxisRaw("SanfonaAxis");
		sanfonaStrength = Mathf.Clamp(sanfonaStrength, -1f, 1f);
		player.SetSanfonaStrength(sanfonaStrength);
		#endregion

		#region check Jump
		//Set Jump
		bool pressedJump = false;
		if(Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space)){
			player.OnJumpInputDown ();
			pressedJump = true;
		}

		if(pressedJump && !holdingJump){
			holdingJump = true;
		}
		if(!pressedJump && holdingJump) {
			player.OnJumpInputHold ();
		}

		if(Input.GetKeyUp(KeyCode.JoystickButton0) || Input.GetKeyUp(KeyCode.Space)){
			player.OnJumpInputUp ();
			pressedJump = holdingJump = false;
		}
		#endregion

		#region Check Sing
		//Check Floreio
		if(Input.GetAxis("R_Trigger") != 0 || Input.GetKeyDown(KeyCode.E)){
			if(floreioHoldTime == 0f){
				player.OnFloreioInputDown ();
			} else {
				//player.OnFloreioInputHold ();
			}
			floreioHoldTime += Time.deltaTime;
		} else {
			//player.OnFloreioInputUp ();
			floreioHoldTime = 0f;
		}

		//Chech Staccato
		if(Input.GetKeyDown(KeyCode.JoystickButton5) || Input.GetKeyDown(KeyCode.F)){
			if (staccatoHoldTime == 0f) {
				player.OnStaccatoInputDown ();
			}
			staccatoHoldTime += Time.deltaTime;
			if(staccatoHoldTime > 2f){ //Se estiver segurando o botao por 2s, pode tocar de novo.
				staccatoHoldTime = 0f;
			}
		} else {
			//player.OnStacattoInputUp ();
			staccatoHoldTime = 0f;
		}
		#endregion

		#region Check Cheat
		if(Input.GetKeyDown(KeyCode.PageUp)){
			player.SetCheatState(true);
		}
		if(Input.GetKeyDown(KeyCode.PageDown)){
			player.SetCheatState(false);
		}
		#endregion
	}
}
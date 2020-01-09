using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvasManager : MonoBehaviour {

	// Main System
	private MainScript Main;

	// Canvas遷移用ボタン
	private Button ChangeToSettingsButton;
	private Button ChangeToMyConsoleButton;

	// UI
	private Button CallService_Move_Button;
	private Button CallService_Grasp_Button;
	private Button CallService_MoveAndGrasp_Button;

	// Startが終わったかどうか
	private bool finish_start = false;
	public bool IsFinishStart() { return finish_start; }

	// Service Calling Manager
	private ServiceCallingManager ServiceCallingManager;
	private bool calling_move = false;
	private bool calling_grasp = false;
	private bool calling_moveandgrasp = false;


	/**************************************************
	 * Start()
	 **************************************************/
	void Start() {
		// Main Systemを取得
		Main = GameObject.Find("Main System").GetComponent<MainScript>();

		// Canvas遷移用ボタンを取得・設定
		ChangeToSettingsButton = GameObject.Find("Main System/Main Canvas/Vertical/Horizontal/Change to Settings Button").GetComponent<Button>();
		ChangeToMyConsoleButton = GameObject.Find("Main System/Main Canvas/Vertical/Horizontal/Change to MyConsole Button").GetComponent<Button>();
		ChangeToSettingsButton.onClick.AddListener(Main.ChangeToSettings);
		ChangeToMyConsoleButton.onClick.AddListener(Main.ChangeToMyConsole);

		// UIを取得・設定
		CallService_Move_Button = GameObject.Find("Main System/Main Canvas/Vertical/Vertical/Move/Button").GetComponent<Button>();
		CallService_Grasp_Button = GameObject.Find("Main System/Main Canvas/Vertical/Vertical/Grasp/Button").GetComponent<Button>();
		CallService_MoveAndGrasp_Button = GameObject.Find("Main System/Main Canvas/Vertical/Vertical/Move and Grasp/Button").GetComponent<Button>();
		CallService_Move_Button.onClick.AddListener(StartCallServiceMove);
		CallService_Grasp_Button.onClick.AddListener(StartCallServiceGrasp);
		CallService_MoveAndGrasp_Button.onClick.AddListener(StartCallServiceMoveAndGrasp);

		ServiceCallingManager = GameObject.Find("Ros Socket Client").GetComponent<ServiceCallingManager>();

		finish_start = true;
	}

	/**************************************************
	 * Update
	 **************************************************/
	void Update() {
		if (calling_move) {
			if(ServiceCallingManager.IsConnected() && !ServiceCallingManager.CheckWaitAnything()) {
				IEnumerator coroutine = ServiceCallingManager.CallServiceToMove();
				StartCoroutine(coroutine);
				Main.MyConsole_Add("Move");
			}

			if (ServiceCallingManager.CheckWaitAnything()) {
				if (ServiceCallingManager.CheckAbort()) {
					Main.MyConsole_Add("Abort");
					ServiceCallingManager.FinishAccess();
					calling_move = false;
				}
				if (ServiceCallingManager.CheckSuccess()) {
					Main.MyConsole_Add(ServiceCallingManager.GetResponceJson());
					ServiceCallingManager.FinishAccess();
					calling_move = false;
				}
			}
		}

		if (calling_grasp) {
			if (ServiceCallingManager.IsConnected() && !ServiceCallingManager.CheckWaitAnything()) {
				IEnumerator coroutine = ServiceCallingManager.CallServiceToGrasp();
				StartCoroutine(coroutine);
				Main.MyConsole_Add("Grasp");
			}

			if (ServiceCallingManager.CheckWaitAnything()) {
				if (ServiceCallingManager.CheckAbort()) {
					Main.MyConsole_Add("Abort");
					ServiceCallingManager.FinishAccess();
					calling_grasp = false;
				}
				if (ServiceCallingManager.CheckSuccess()) {
					Main.MyConsole_Add(ServiceCallingManager.GetResponceJson());
					ServiceCallingManager.FinishAccess();
					calling_grasp = false;
				}
			}
		}

		if (calling_moveandgrasp) {
			if (ServiceCallingManager.IsConnected() && !ServiceCallingManager.CheckWaitAnything()) {
				IEnumerator coroutine = ServiceCallingManager.CallServiceToMoveAndGrasp();
				StartCoroutine(coroutine);
				Main.MyConsole_Add("Move and Grasp");
			}

			if (ServiceCallingManager.CheckWaitAnything()) {
				if (ServiceCallingManager.CheckAbort()) {
					Main.MyConsole_Add("Abort");
					ServiceCallingManager.FinishAccess();
					calling_grasp = false;
				}
				if (ServiceCallingManager.CheckSuccess()) {
					Main.MyConsole_Add(ServiceCallingManager.GetResponceJson());
					ServiceCallingManager.FinishAccess();
					calling_grasp = false;
				}
			}
		}
	}

	void StartCallServiceMove() {
		if(!calling_move && !calling_grasp && !calling_moveandgrasp) {
			calling_move = true;
		}
	}

	void StartCallServiceGrasp() {
		if (!calling_move && !calling_grasp && !calling_moveandgrasp) {
			calling_grasp = true;
		}
	}

	void StartCallServiceMoveAndGrasp() {
		if (!calling_move && !calling_grasp && !calling_moveandgrasp) {
			calling_moveandgrasp = true;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartPreviewedRealityConfig {
	public string ros_ip = "ws://192.168.4.170:9090";
	public bool screen_not_sleep = true;
	public Vector3 vicon_offset_pos = new Vector3();
	public Vector3 calibration_offset_pos = new Vector3();
	public float calibration_offset_yaw = 0.0f;
	public Vector3 robot_offset_pos = new Vector3();
	public float robot_offset_yaw = 0.0f;
	public float safety_distance = 1.0f;
	public float room_alpha = 1.0f;
	public float robot_alpha = 1.0f;
}

public enum CanvasName {
	Error = -1,
	MainCanvas = 0,
	SettingCanvas = 1,
	MyConsoleCanvas = 2
}

public class MainScript : MonoBehaviour {
	
	//画面が消えないようにする
	private bool screen_not_sleep = true;

	//Startの処理がすべて終わったかどうか
	private bool finish_start_all = false;
	public bool FinishStartAll() { return finish_start_all; }

	//コンフィグデータ
	private SmartPreviewedRealityConfig config_data;
	public SmartPreviewedRealityConfig GetConfig() { return config_data; }
	private bool finish_read_config = false;
	public bool FinishReadConfig() { return finish_read_config; }

	//Canvasたち

	//どのキャンバスを使用中か示す変数と対応する辞書


	/**************************************************
	 * Start()
	 **************************************************/
	void Start() {
		// 画面が消えないようにする
		if (screen_not_sleep) {
			Screen.sleepTimeout = SleepTimeout.NeverSleep;
		}
		else {
			Screen.sleepTimeout = SleepTimeout.SystemSetting;
		}
	}


	/**************************************************
	 * Update
	 **************************************************/
	void Update() {
		// 戻るボタンでアプリ終了
		if (Input.GetKey(KeyCode.Escape)) {
			Application.Quit();
		}

	}
}

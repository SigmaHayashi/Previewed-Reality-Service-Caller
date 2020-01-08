using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceCallerConfig {
	public string ros_ip = "ws://192.168.4.170:9090";
	public bool screen_not_sleep = true;
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
	public bool IsFinishStartAll() { return finish_start_all; }

	//コンフィグデータ
	private ServiceCallerConfig config_data;
	public ServiceCallerConfig GetConfig() { return config_data; }
	private bool finish_read_config = false;
	public bool IsFinishReadConfig() { return finish_read_config; }

	//Canvasたち
	private MainCanvasManager MainCanvas;
	private SettingsCanvasManager SettingsCanvas;
	private MyConsoleCanvasManager MyConsoleCanvas;

	//どのキャンバスを使用中か示す変数と対応する辞書
	private CanvasName active_canvas = CanvasName.Error;
	private Dictionary<CanvasName, GameObject> CanvasDictionary = new Dictionary<CanvasName, GameObject>();

	//MyConsole Canvasのバッファ
	private List<object> MyConsole_Message_Buffer = new List<object>();
	//private bool myconsole_delete_buffer;


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

		// Canvasを取得
		MainCanvas = GameObject.Find("Main System/Main Canvas").GetComponent<MainCanvasManager>();
		SettingsCanvas = GameObject.Find("Main System/Settings Canvas").GetComponent<SettingsCanvasManager>();
		MyConsoleCanvas = GameObject.Find("Main System/MyConsole Canvas").GetComponent<MyConsoleCanvasManager>();

		// CanvasをDictionaryに追加
		CanvasDictionary.Add(CanvasName.MainCanvas, MainCanvas.gameObject);
		CanvasDictionary.Add(CanvasName.SettingCanvas, SettingsCanvas.gameObject);
		CanvasDictionary.Add(CanvasName.MyConsoleCanvas, MyConsoleCanvas.gameObject);
	}


	/**************************************************
	 * Update
	 **************************************************/
	void Update() {
		// 戻るボタンでアプリ終了
		if (Input.GetKey(KeyCode.Escape)) {
			Application.Quit();
		}

		if (!finish_read_config && SettingsCanvas.IsFinishStart()) {
			config_data = SettingsCanvas.GetConfig();
			screen_not_sleep = config_data.screen_not_sleep;
			finish_read_config = true;
		}

		if (!finish_start_all &&
			MainCanvas.IsFinishStart() &&
			SettingsCanvas.IsFinishStart() && 
			MyConsoleCanvas.IsFinishStart()) {
			foreach (KeyValuePair<CanvasName, GameObject> canvas in CanvasDictionary) {
				if (canvas.Key != CanvasName.MainCanvas) {
					canvas.Value.SetActive(false);
				}
			}
			active_canvas = CanvasName.MainCanvas;

			finish_start_all = true;
		}
	}

	/**************************************************
	 * どのCanvasを使用中か返す
	 **************************************************/
	public CanvasName WhichCanvasActive() {
		return active_canvas;
	}

	/**************************************************
	 * 画面の切り替え：Main Canvas
	 **************************************************/
	public void ChangeToMain() {
		CanvasDictionary[active_canvas].SetActive(false);
		active_canvas = CanvasName.MainCanvas;
		CanvasDictionary[active_canvas].SetActive(true);
	}

	/**************************************************
	 * 画面の切り替え：Settings Canvas
	 **************************************************/
	public void ChangeToSettings() {
		CanvasDictionary[active_canvas].SetActive(false);
		active_canvas = CanvasName.SettingCanvas;
		CanvasDictionary[active_canvas].SetActive(true);
	}

	/**************************************************
	 * 画面の切り替え：MyConsole Canvas
	 **************************************************/
	public void ChangeToMyConsole() {
		CanvasDictionary[active_canvas].SetActive(false);
		active_canvas = CanvasName.MyConsoleCanvas;
		CanvasDictionary[active_canvas].SetActive(true);

		MyConsoleCanvas.Add(MyConsole_Message_Buffer);
		MyConsole_Message_Buffer = new List<object>();
	}

	/**************************************************
	 * MyConsole Canvas : Add
	 **************************************************/
	public void MyConsole_Add(object message) {
		if (WhichCanvasActive() == CanvasName.MyConsoleCanvas) {
			MyConsoleCanvas.Add(message);
		}
		else {
			MyConsole_Message_Buffer.Add(message);
		}
	}
}

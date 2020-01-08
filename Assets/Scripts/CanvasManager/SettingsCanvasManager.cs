using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class SettingsCanvasManager : MonoBehaviour {

	//Main System
	private MainScript Main;

	//Canvas遷移用ボタン
	private Button BackToMainButton;
	private Button RestartAppButton;

	//コンフィグ周り
	private string config_filepath;
	private ServiceCallerConfig config_data = new ServiceCallerConfig();
	public ServiceCallerConfig GetConfig() { return config_data; }

	//UI
	private InputField RosIpInput;
	private Toggle ScreenNotSleepToggle;

	//Startが終わったかどうか
	private bool finish_start = false;
	public bool IsFinishStart() { return finish_start; }


	/**************************************************
	 * Start()
	 **************************************************/
	void Start() {
		//Main Systemを取得
		Main = GameObject.Find("Main System").GetComponent<MainScript>();

		//Canvas遷移ボタンを取得・設定
		BackToMainButton = GameObject.Find("Main System/Settings Canvas/Vertical/Back to Main Button").GetComponent<Button>();
		RestartAppButton = GameObject.Find("Main System/Settings Canvas/Vertical/Restart App Button").GetComponent<Button>();
		BackToMainButton.onClick.AddListener(Main.ChangeToMain);
		RestartAppButton.onClick.AddListener(RestartApp);

		//UIを取得・設定
		RosIpInput = GameObject.Find("Main System/Settings Canvas/Vertical/Vertical/ROS IP/Input").GetComponent<InputField>();
		ScreenNotSleepToggle = GameObject.Find("Main System/Settings Canvas/Vertical/Vertical/Screen Not Sleep/Toggle").GetComponent<Toggle>();

		RosIpInput.onValueChanged.AddListener(ActivateRestartButton);
		ScreenNotSleepToggle.onValueChanged.AddListener(ActivateRestartButton);

		//コンフィグファイル読み込み
		config_filepath = Application.persistentDataPath + "/Previewed Reality Service Caller Config.JSON";
		if (!File.Exists(config_filepath)) {
			using (File.Create(config_filepath)) { }
			string config_json = JsonUtility.ToJson(config_data);
			using (FileStream file = new FileStream(config_filepath, FileMode.Create, FileAccess.Write)) {
				using (StreamWriter writer = new StreamWriter(file)) {
					writer.Write(config_json);
				}
			}
		}
		using (FileStream file = new FileStream(config_filepath, FileMode.Open, FileAccess.Read)) {
			using (StreamReader reader = new StreamReader(file)) {
				string config_read = reader.ReadToEnd();
				Debug.Log(config_read);

				config_data = JsonUtility.FromJson<ServiceCallerConfig>(config_read);

				RosIpInput.text = config_data.ros_ip;
				ScreenNotSleepToggle.isOn = config_data.screen_not_sleep;
			}
		}

		BackToMainButton.gameObject.SetActive(true);
		RestartAppButton.gameObject.SetActive(false);

		finish_start = true;
	}

	/**************************************************
	 * Update
	 **************************************************/
	void Update() {

	}


	void ActivateRestartButton(string s) {
		BackToMainButton.gameObject.SetActive(false);
		RestartAppButton.gameObject.SetActive(true);
	}

	void ActivateRestartButton(bool b) {
		BackToMainButton.gameObject.SetActive(false);
		RestartAppButton.gameObject.SetActive(true);
	}

	void RestartApp() {
		config_data.ros_ip = RosIpInput.text;
		config_data.screen_not_sleep = ScreenNotSleepToggle.isOn;
		
		string config_json = JsonUtility.ToJson(config_data);

		using (FileStream file = new FileStream(config_filepath, FileMode.Create, FileAccess.Write)) {
			using (StreamWriter writer = new StreamWriter(file)) {
				writer.Write(config_json);
			}
		}

		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
}

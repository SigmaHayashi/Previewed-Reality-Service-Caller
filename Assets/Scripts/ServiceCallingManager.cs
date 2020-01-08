using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class TmsTsMasterRequest {
	public int rostime;
	public int task_id;
	public int robot_id;
	public int object_id;
	public int user_id;
	public int place_id;
	public int priority;
}

[Serializable]
public class TmsTsMasterResponse {
	public int result;
	public float val;
}

public class ServiceCallingManager : MonoBehaviour {

	private MainScript Main;

	//Ros Socket Client関連
	private RosSocketClient RosSocketClient;
	private readonly string service_name = "tms_ts_master";
	private string response_json;
	private TmsTsMasterResponse response_value;

	private float time_access = 0.0f;

	private bool wait_anything = false;
	private bool access_db = false;
	private bool success_access = false;
	private bool abort_access = false;

	//private bool wait_service = false;
	//public bool IsWaitService() { return wait_service; }

	// Start is called before the first frame update
	void Start() {
		Main = GameObject.Find("Main System").GetComponent<MainScript>();

		//ROSTMSに接続
		RosSocketClient = GameObject.Find("Ros Socket Client").GetComponent<RosSocketClient>();
	}


	// Update is called once per frame
	void Update() {
		if (!Main.IsFinishStartAll()) {
			return;
		}

		if (RosSocketClient.GetConnectionState() == ConnectionState.Disconnected) { //切断時
			time_access += Time.deltaTime;
			if (time_access > 10.0f) {
				time_access = 0.0f;
				RosSocketClient.Connect();
			}
		}


		if (RosSocketClient.GetConnectionState() == ConnectionState.Connected) {
			if (wait_anything) {
				if (!success_access || !abort_access) {
					WaitResponce(5.0f);
				}
			}
		}
	}

	/**************************************************
	 * 接続状態確認
	 **************************************************/
	public bool IsConnected() {
		if (RosSocketClient.GetConnectionState() == ConnectionState.Connected) {
			return true;
		}
		return false;
	}

	/**************************************************
	 * ROSからの返答待ち
	 **************************************************/
	void WaitResponce(float timeout) {
		time_access += Time.deltaTime;
		if (time_access > timeout) {
			time_access = 0.0f;
			abort_access = true;
			access_db = false;
		}

		KeyValuePair<bool, string> response = RosSocketClient.GetServiceResponseMessage(service_name);
		if (response.Key) {
			response_json = response.Value;
			string response_value_json = RosSocketClient.GetJsonArg(response_json, nameof(ServiceResponse.values));
			response_value = JsonUtility.FromJson<TmsTsMasterResponse>(response_value_json);

			success_access = true;
			access_db = false;
		}
	}

	/**************************************************
	 * データ取得時のAPI
	 **************************************************/
	public bool CheckWaitAnything() { return wait_anything; }

	public bool CheckSuccess() { return success_access; }

	public bool CheckAbort() { return abort_access; }

	public string GetResponceJson() { return response_json; }
	public TmsTsMasterResponse GetResponceValue() { return response_value; }

	public void FinishAccess() {
		success_access = abort_access = false;
	}
	
	/**************************************************
	 * Call Service : Move
	 **************************************************/
	public IEnumerator CallServiceToMove() {
		wait_anything = access_db = true;
		time_access = 0.0f;

		TmsTsMasterRequest request = new TmsTsMasterRequest() {
			task_id = 8007,
			robot_id = 2003,
			place_id = 6017
		};
		RosSocketClient.ServiceCaller(service_name, request);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything  = false;
	}

	/**************************************************
	 * Call Service : Grasp
	 **************************************************/
	public IEnumerator CallServiceToGrasp() {
		wait_anything = access_db = true;
		time_access = 0.0f;

		TmsTsMasterRequest request = new TmsTsMasterRequest() {
			task_id = 8005,
			robot_id = 2003,
			object_id = 7001,
		};
		RosSocketClient.ServiceCaller(service_name, request);

		while (access_db) {
			yield return null;
		}

		while (success_access || abort_access) {
			yield return null;
		}

		wait_anything = false;
	}
}

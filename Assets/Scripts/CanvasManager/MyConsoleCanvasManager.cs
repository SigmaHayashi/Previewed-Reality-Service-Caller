using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyConsoleCanvasManager : MonoBehaviour {

	// Main System
	private MainScript Main;

	// Canvas遷移用ボタン
	private Button BackToMainButton;

	// スクロールコンテンツの要素
	private RectTransform ScrollContentsRect;
	private GameObject ContentsPrefab;
	private GameObject ContentsParent;
	private RectTransform ScrollViewRect;

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
		BackToMainButton = GameObject.Find("Main System/MyConsole Canvas/Vertical/Horizontal_0/Back to Main Button").GetComponent<Button>();
		BackToMainButton.onClick.AddListener(Main.ChangeToMain);

		//スクロールコンテンツの要素を取得
		ScrollContentsRect = GameObject.Find("Main System/MyConsole Canvas/Vertical/Horizontal_1/Scroll View/Scroll Contents").GetComponent<RectTransform>();
		ContentsPrefab = (GameObject)Resources.Load("Contents Text");
		ContentsParent = GameObject.Find("Main System/MyConsole Canvas/Vertical/Horizontal_1/Scroll View/Scroll Contents");
		ScrollViewRect = GameObject.Find("Main System/MyConsole Canvas/Vertical/Horizontal_1/Scroll View").GetComponent<RectTransform>();

		finish_start = true;
	}

	/**************************************************
	 * Update
	 **************************************************/
	void Update() {
		//スクロールコンテンツのサイズが画面いっぱいになたら下側を表示させる
		if (ScrollContentsRect.rect.size.y > ScrollViewRect.rect.size.y) {
			ScrollContentsRect.pivot = new Vector2(0, 0);
		}
		else {
			ScrollContentsRect.pivot = new Vector2(0, 1);
		}
	}

	/**************************************************
	 * テキストを追加
	 **************************************************/
	public void Add(object message) {
		GameObject NewObject = Instantiate(ContentsPrefab);
		NewObject.transform.SetParent(ContentsParent.transform, false);
		NewObject.transform.localScale = new Vector3(1, 1, 1);
		NewObject.GetComponent<Text>().text = message.ToString();
	}

	/**************************************************
	 * テキストを一括で追加
	 **************************************************/
	public void Add(List<object> messages) {
		foreach (object message in messages) {
			Add(message);
		}
	}
}

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Nanolink;

public class UIManager : MonoBehaviour {
	GameObject ui;

	private GobangClient myClient;

    //联机成功加载棋盘
    public Board m_bboard;

	void Start() {
		ui = GameObject.Find ("UI");

		myClient = new GobangClient();

		// 天梯实时对战服务 初始化
		// init()，第一个参数为 appKey。
		// appKey 可以直接在后台创建。
		GobangClient.init("acd17643930c7363", 2);

		// 配置
		GobangClient.config ("debug-level", "7");

		// 可以隐藏 玩家输入名字+登录 界面，直接匹配
		// GobangClient.connect(8);

		// 如果存储的名字不存在，弹出输入昵称UI
		// 有昵称时，直接匹配
		string nickname = PlayerPrefs.GetString ("nickname");
		if (nickname == "") {
			GameObject mainObj = ui.transform.Find ("Main").gameObject;
			mainObj.SetActive (true);
		} else {
			// 赋值 GameManager 昵称
			GameManager.nickname = nickname;

			// 有昵称时，直接匹配
			GobangClient.connect(8);
		}
	}

    bool bChessPos;
    void FixedUpdate () {
		if(myClient != null)
			myClient.doUpdate ();
        //联机成功布置棋盘一次
        if (NanoClient.isConnected()&&!bChessPos)
        {
            bChessPos = true;
            m_bboard.chessposition();
        }

	}

	void OnGUI () {
		//if(myClient != null)
			//myClient.drawGUI ();
	}

	public void onButtonClick(string name) {
		if(name == "")
			return;

        GameObject mainObj = ui.transform.Find("Main").gameObject;
        switch (name) {
		case "level":
			// 昵称不能为空
			GameObject textObj = mainObj.transform.Find ("InputField/Text").gameObject;
			GameObject errorTipObj = mainObj.transform.Find ("ErrorTip").gameObject;
			string nickname = textObj.GetComponent<Text> ().text;
			if (nickname == "") {
				errorTipObj.SetActive (true);
				return;
			}

			GameManager.nickname = nickname;

			// 记录 nickname
			PlayerPrefs.SetString ("nickname", nickname);

			errorTipObj.SetActive (false);
			mainObj.SetActive (false);

			// 等级匹配
			GobangClient.connect (8);
			break;

		case "retry":
                //GobangClient.disconnect ();
                GameObject start = GameObject.Find("Start") as GameObject;
                ReGame RG = start.GetComponent<ReGame>();
                RG.ChessPostion();
                // 结果通知给对方
                Hashtable values = new Hashtable();
                values.Add("name", "piece");
                values.Add("FromX", -1);
                values.Add("FromY", -1);
                values.Add("ToX", -1);
                values.Add("ToY", -1);
                values.Add("Move", "红方走");
                values.Add("YidongOrChizi", "Yidong");
                values.Add("Regame", 1);
                GobangClient.send(GameSerialize.toBytes(values));
                GameObject resultObj = ui.transform.Find("Result").gameObject;

                resultObj.SetActive(false);
                break;

		}
	}
}
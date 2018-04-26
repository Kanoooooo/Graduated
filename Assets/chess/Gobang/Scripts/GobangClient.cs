using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Nanolink;

public class GobangClient : NanoClient {
	// GobangClient 主要重写 Nanolink实时对战服务已封装的 onMessage, onResync, onStatusChanged, onConnected, onDisconnected 方法。

	// 重写接受到数据的响应函数，在GameManager.recvMessage()中统一处理接收到数据
	protected override void onMessage (byte[] data, byte fromIndex) {
		// Hashtable values = GameSerialize.fromBytes (data);

		GameObject gameObj = GameObject.Find ("Game");
		if (gameObj == null)
			return;

		GameManager gameManager = gameObj.GetComponent<GameManager> ();
		if (gameManager == null)
			return;

		// 同意处理 接收到的数据
		gameManager.recvMessage(GameSerialize.fromBytes (data), fromIndex);
	}

	// 匹配成功时，自动调用 onResync 方法
	protected override void onResync(byte fromIndex) {
		// 同步玩家的 名字
		{
			Debug.Log(GameManager.nickname);

			// 交换名字，握手
			{
				Hashtable values = new Hashtable ();
				values.Add ("name", "handshake");
				values.Add ("v", GameManager.nickname);

				GobangClient.send (GameSerialize.toBytes (values));
			}

			// 先进入房间的玩家 为红方
			string chess = "";
			if (getInt ("client-index") == 0)
				chess = "红方";
			else
				chess = "黑方";

			GameObject ui = GameObject.Find ("UI");
			GameObject userInfoObj = ui.transform.Find ("UserInfo").gameObject;
			GameObject selfObj = userInfoObj.transform.Find ("Self").gameObject;
			selfObj.GetComponent<Text> ().text = "本人: " + GameManager.nickname + " (" + chess + ")";

			userInfoObj.SetActive (true);
		}
	}

    protected override void onStatusChanged(string newStatus, string oldStatus)
    {
        Debug.Log("状态由 " + oldStatus + " 改为 " + newStatus);
    }

    protected override void onConnected() {
		Debug.Log ("连接成功， playerIndex:" + getInt("client-index") + "; serverId:" + getString("server-id"));

		// 先进入房间的玩家 为红方
		if (getInt ("client-index") == 0)
			GameManager.userColor = "Red";
		else
			GameManager.userColor = "Black";
	}

	protected override void onDisconnected(int error) {
		if (error == 0) {
			if (disconnectedBySelf)
				Debug.Log ("主动断开");
			else
				Debug.Log ("对方断开");
		} else {
			// 错误代码具体参考 "Nanolink SDK 接口说明" 中 lastError 定义
			if (error == 501) {
				if (getInt ("last-time", -2) < 2000)
					Debug.Log ("超时断开, 可能是对方原因");
				else
					Debug.Log ("超时断开, 可能是己方原因");
			}
		}

		// 断开连接 重新reload 当前关卡
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	// 输出调试信息
	public void drawGUI () {
		// 回放不支持 stats 数据, 忽略
		if (getInt ("mode") == 0)
			return;

		GUIStyle guiStyle = new GUIStyle ();
		guiStyle.normal.textColor = Color.black;
		guiStyle.fontSize = 32;

		// 延迟 us->ms
		float latency = getInt ("latency") / 1000.0f;

		// 当前的状态
		string s = getStatus();
		if (s == "connected") {
			if (getInt ("is-master") == 1)
				s += ", 主场";
			else
				s += ", 客场";
		}

		// 对方 ID (或房间号)
		string targetId = getString("target-id");
		if (targetId.Length > 8)
			targetId = targetId.Substring (targetId.Length - 8);

		// 当前玩家 ID
		string clientId = getString("client-id");
		if (clientId.Length > 8)
			clientId = clientId.Substring (clientId.Length - 8);

		// Stats 数据
		string stats = getString("stats");

		// Stats 数据映射字典
		Dictionary<string, string> dictionary = new Dictionary<string, string>();

		string[] strs = stats.Split('\n');
		foreach (var str in strs) {
			string[] keyValue = str.Split('=');
			if (keyValue.Length > 1) {
				dictionary.Add(keyValue[0], keyValue[1]);
			}
		}

		string sendTaskIndex = dictionary ["send.task.index"];
		string sendTaskBytes = dictionary ["send.task.bytes"];
		string sendTotalBytes = dictionary ["send.total.bytes"];
		string recvTaskIndex = "-";
		string recvTaskBytes = "-";
		string recvTotalBytes = "-";

		string targetIdStr = "";

		// 根据连接模式区分输出信息
		if(getInt ("mode") == 3) {
			targetIdStr = "房间: " + targetId;
		} else {
			targetIdStr = "对方: " + targetId;

			// 1 VS 1 延迟／2
			latency = latency / 2;

			recvTaskIndex = dictionary ["recv.task.index"];
			recvTaskBytes = dictionary ["recv.task.bytes"];
			recvTotalBytes = dictionary ["recv.total.bytes"];
		}

		GUI.Label (new Rect(40, 140, 200, 200), "状态: " + s, guiStyle);

		GUI.Label (new Rect(40, 180, 200, 200), targetIdStr + ", 设备: " + clientId, guiStyle);

		// 延迟
		if (latency >= 0)
			GUI.Label (new Rect(40, 220, 200, 200), "时延: " + latency + " ms", guiStyle);
		else
			GUI.Label (new Rect(40, 220, 200, 200), "时延: ", guiStyle);

		GUI.Label (new Rect (40, 260, 200, 200), "发送:  " + sendTaskIndex + "次/ " + sendTaskBytes + "字节/ " + sendTotalBytes + "字节, 接收: " + recvTaskIndex + "次/ " + recvTaskBytes + "字节/ " + recvTotalBytes + "字节", guiStyle);

	}
}

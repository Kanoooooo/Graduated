using System.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    //public UILabel uil;

    //申请悔棋按钮
    public GameObject GoBackButton;
    public GameObject GoBackText;
    //悔棋脚本
    public ChessChongzhi CC;
    //计时器
    TimeManager TM;

    public static string nickname;

    // 当前是谁的回合
    public static string curTurn;

    // 用户的颜色
    // 先进入房间的玩家 为红方，即，getInt("client-index") == 0
    // getInt("client-index") == 1 为黑方
    public static string userColor;

    bool bRegame = false;
    private void Update()
    {
        //双方再开一局时重新计时
        if(bRegame)
        {
            // 如果没有重开UI
            GameObject ui = GameObject.Find("UI");
            GameObject resultObj = ui.transform.Find("Result").gameObject;
            if (resultObj.activeSelf == false)
            {
                //开始计时
                TM.bStartCul = true;
                TM.Timer.value = 1;
                TM.bGameOver = false;
                bRegame = false;
            }
            return;
        }
    }


    void Awake() {
        curTurn = "Red";
    }

	void Start() {

        GameObject obj = GameObject.Find("Timer") as GameObject;
        TM = obj.GetComponent<TimeManager>();
    }

	// 
	public void updateChess() {

        // 胜利
        if (blackclick.str == "红色方胜利"|| blackclick.str == "黑色方胜利")
        {
            // 结果通知给对方
            Hashtable values = new Hashtable();
            values.Add("name", "lose");
            values.Add("v", (byte)1);
            GobangClient.send(GameSerialize.toBytes(values));
            // UI
            GameObject ui = GameObject.Find("UI");
            GameObject resultObj = ui.transform.Find("Result").gameObject;
            GameObject gameoverObj = resultObj.transform.Find("GameOver").gameObject;
            gameoverObj.GetComponentInChildren<Text>().text = "完胜";

            TM.bStartCul = false;
            TM.Timer.value = 1;

            resultObj.SetActive(true);
        }
        // 超时失败
        if (TM.bGameOver)
        {
            // 结果通知给对方
            Hashtable values = new Hashtable();
            values.Add("name", "lose");
            values.Add("v", (byte)2);
            GobangClient.send(GameSerialize.toBytes(values));
            // UI
            GameObject ui = GameObject.Find("UI");
            GameObject resultObj = ui.transform.Find("Result").gameObject;
            GameObject gameoverObj = resultObj.transform.Find("GameOver").gameObject;
            gameoverObj.GetComponentInChildren<Text>().text = "超时失败";

            TM.bStartCul = false;
            TM.Timer.value = 1;

            resultObj.SetActive(true);
        }
    }

	// 接收到消息
	public void recvMessage(Hashtable values, byte fromIndex) {
		string name = (string) values["name"];
		switch(name) {
		// 握手，交换名片
		case "handshake":
			{
				// 先进入房间的玩家 为红方
				string theChess = "";
				if (fromIndex == 0)
					theChess = "红方";
				else
					theChess = "黑方";

                //设置名称
                GameObject Enemy = GameObject.Find("EnemyName");
                UILabel label = Enemy.GetComponent<UILabel>();
                label.text = values["v"] + " (" + theChess + ")";
                //设置头像
                GameObject EnemyPicture = GameObject.Find("EnemyPic");
                //string EnemyPic = (string)values["ImgStr"];
                //if(EnemyPic!="wu")
                //{
                //        uil.text = "联机成功";
                //        //设置头像
                //        SpriteRenderer m_srSR = EnemyPicture.GetComponent<SpriteRenderer>();
                //    Texture2D img = new Texture2D(1024,1024);
                //    byte[] data = System.Convert.FromBase64String(EnemyPic);
                //    img.LoadImage(data);
                //    Sprite sp = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.0f, 0.0f));
                //    m_srSR.sprite = sp;
                //}
			}
			break;

		// 落子
		case "piece":
			{
                int Regame = (int)values["Regame"];
				int FromX = (int) values["FromX"];
				int FromY = (int) values["FromY"];
                int ToX = (int)values["ToX"];
                int ToY = (int)values["ToY"];
                string Move = (string) values["Move"];
                string YidongOrChizi = (string)values["YidongOrChizi"];
                int GoBack = (int)values["GoBack"];
                    //再来一局
                    if(Regame == 1)
                    {
                        GameObject start = GameObject.Find("Start") as GameObject;
                        ReGame RG = start.GetComponent<ReGame>();
                        RG.ChessPostion();
                        bRegame = true;
                        return;
                    }
                    //拒绝悔棋
                    if(GoBack == 0)
                    {
                        GoBackText.SetActive(false);
                    }
                    //接收到申请悔棋
                    if (GoBack == 1)
                    {
                        //接受到悔棋申请暂停计时
                        TimeManager TM;
                        GameObject obj = GameObject.Find("Timer") as GameObject;
                        TM = obj.GetComponent<TimeManager>();
                        TM.bStartCul = false;
                        GoBackButton.SetActive(true);
                        return;
                    }
                    if (GoBack == 2)
                    {
                        // 悔棋后，切换回合
                        if (GameManager.curTurn == "Red")
                        {
                            GameManager.curTurn = "Black";
                        }
                        else
                        {
                            GameManager.curTurn = "Red";
                        }
                        GoBackText.SetActive(false);
                        CC.IloveHUIQI();
                        return;
                    }
                    // 渲染接收到的数据，落子
                    if (YidongOrChizi == "Yidong")
                    {
                        GameObject item1 = ItemOne(FromX, FromY);//得到第一个框名字
                        GameObject item2 = ItemTwo(ToX, ToY);//得到第二个框名字
                        GameObject firstChess = chessOne(item1);//得到第一个旗子名字
                        int a = Board.chess[FromY, FromX];
                        int b = Board.chess[ToY, ToX];
                        chzh.AddChess(ChessChongzhi.Count, FromX, FromY, ToX, ToY, true, a, b);
                        IsMove(firstChess.name, item2, FromX, FromY, ToX, ToY);
                        blackclick.ChessMove = !blackclick.ChessMove;
                        blackclick.str = Move;
                        // 落子后，切换回合
                        if (GameManager.curTurn == "Red")
                        {
                            GameManager.curTurn = "Black";
                        }
                        else
                        {
                            GameManager.curTurn = "Red";
                        }
                    }
                    if(YidongOrChizi == "Chizi")
                    {
                        GameObject item1 = ItemOne(FromX, FromY);//得到第一个框名字
                        GameObject item2 = ItemTwo(ToX, ToY);//得到第二个框名字
                        GameObject firstChess = chessOne(item1);//得到第一个旗子名字
                        GameObject secondChess = ChessTwo(item2);//得到第一个旗子名字
                        int a = Board.chess[FromY, FromX];
                        int b = Board.chess[ToY, ToX];
                        chzh.AddChess(ChessChongzhi.Count, FromX, FromY, ToX, ToY, true, a, b);
                        IsEat(firstChess.name, secondChess.name, FromX, FromY, ToX, ToY);
                        blackclick.ChessMove = !blackclick.ChessMove;
                        blackclick.str = Move;
                        // 落子后，切换回合
                        if (GameManager.curTurn == "Red")
                        {
                            GameManager.curTurn = "Black";
                        }
                        else
                        {
                            GameManager.curTurn = "Red";
                        }
                    }

			}
			break;

		// gameover
		case "lose":
			{
                byte V = (Byte)values["v"];
                // gameover
                blackclick.TrueOrFalse = false;

                //停止计时
                TM.bStartCul = false;
                TM.Timer.value = 1;

                // UI
                GameObject ui = GameObject.Find("UI");

				GameObject resultObj = ui.transform.Find ("Result").gameObject;
				GameObject gameoverObj = resultObj.transform.Find ("GameOver").gameObject;
                if((int)V == 1)
                {
                    gameoverObj.GetComponentInChildren<Text>().text = "惜败";
                }
                if ((int)V == 2)
                {
                    gameoverObj.GetComponentInChildren<Text>().text = "对方超时，胜利";
                }
                    resultObj.SetActive(true);
            }
			break;

		default:
			Debug.Log ("无效的指令");
			break;
		}

	}

    ChessChongzhi chzh = new ChessChongzhi();
    //移动类
    public void IsMove(string One, GameObject game, int x1, int y1, int x2, int y2)
    {
        GameObject parent1 = GameObject.Find(One);
        parent1.transform.parent = game.transform;
        parent1.transform.localPosition = Vector3.zero;
        Board.chess[y2, x2] = Board.chess[y1, x1];
        Board.chess[y1, x1] = 0;

    }
    //吃子类
    public void IsEat(string Frist, string sconde, int x1, int y1, int x2, int y2)
    {
        GameObject Onename = GameObject.Find(Frist);//得到第一个
        GameObject Twoname = GameObject.Find(sconde);//得到第二个名字
        GameObject Twofather = Twoname.gameObject.transform.parent.gameObject;//得到第二个的父亲
        Onename.gameObject.transform.parent = Twofather.transform;
        Onename.transform.localPosition = Vector3.zero;
        //	Destroy (Twoname);
        //Twoname.transform.localPosition = new Vector3 (1000, 10000, 0);
        Board.chess[y2, x2] = Board.chess[y1, x1];
        Board.chess[y1, x1] = 0;
        GameObject a = GameObject.Find("xiaoshi");
        Twoname.transform.parent = a.transform;
        Twoname.transform.localPosition = new Vector3(5000, 5000, 0);

    }
    //得到第一个旗子名字
    public GameObject chessOne(GameObject obj)
    {
        string s = "";
        GameObject game = null;
        foreach (Transform child in obj.transform)
            s = child.name;//第一个象棋名字
        game = GameObject.Find(s);
        return game;
    }
    //得到第二个旗子名字
    public GameObject ChessTwo(GameObject obj)
    {
        string s = "";
        GameObject game = null;
        foreach (Transform child in obj.transform)
            s = child.name;//第二个象棋名字
        game = GameObject.Find(s);
        return game;
    }
    //得到第一个对象名字
    public GameObject ItemOne(int fromx, int fromy)
    {//得到开始位置gameobject的对象名字
        GameObject obj;
        string s3 = "";
        for (int i = 1; i <= 90; i++)
        {
            obj = GameObject.Find("item" + i.ToString());
            int x = System.Convert.ToInt32((obj.transform.localPosition.x + 263) / 195);
            int y = System.Convert.ToInt32(Mathf.Abs((obj.transform.localPosition.y - 302) / 192));
            if (x == fromx && y == fromy)
                s3 = obj.name;
        }
        obj = GameObject.Find(s3);
        return obj;
    }
    //得到第二个对象名字
    public GameObject ItemTwo(int tox, int toy)
    {//得到开始位置gameobject的对象名字
        GameObject obj;
        string s3 = "";
        for (int i = 1; i <= 90; i++)
        {
            obj = GameObject.Find("item" + i.ToString());
            int x = System.Convert.ToInt32((obj.transform.localPosition.x + 263) / 195);
            int y = System.Convert.ToInt32(Mathf.Abs((obj.transform.localPosition.y - 302) / 192));
            if (x == tox && y == toy)
                s3 = obj.name;
        }
        obj = GameObject.Find(s3);
        return obj;
    }

}
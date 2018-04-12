using UnityEngine;
using System.Collections;
using System.Threading;
public class blackclick : MonoBehaviour {
	public static int FromX=-1, ToX=-1, ToY=-1, FromY=-1;
	public static GameObject ObjBlack=null,ObjRed=null;//红色对象，和黑色对象
	public UILabel Lab;
	public static bool bdfdd = true;//测试
	public static string str="红方走";
	public static bool ChessMove =true;//true   redMove   false BlackMove
	public static bool TrueOrFalse=true;//判断这个时候输赢状态能否走棋  //重新开始记得该true
	public static string RedName=null,BlackName=null,ItemName;//blackchessname  and   redchessname
	rules re = new rules ();

	public UIToggle tog;
	public Blackmove.CHESSMOVE chere;
	Canmovetishi can = new Canmovetishi();
// Use this for initialization
	//控制窗口，不让窗口乱动

	void Start () {
		GameObject obj = GameObject.Find("tishi");
		Lab= obj.GetComponent<UILabel> ();
		}
	
	// Update is called once per frame
	void Update () {
		Lab.text = str;

	}
	//得到点击的象棋名字
	//判断点击到的是什么？
	//0是空   1 是黑色   2 是红色
	public int IsBlackOrRed(int x,int y){
		int Count = Board.chess [y, x];
		if (Count == 0)
			return 0;
		else if (Count > 0 && Count < 8)//是黑色
			return 1;
		else  //是红色 
			return 2;
	}
	public void BlackNameOrRedName(GameObject obj){//得到棋子的名字
	if (obj.name.Substring (0, 1) == "r")
			RedName = obj.name;//得到red名字
		else if (obj.name.Substring (0, 1) == "b")
			BlackName = obj.name;//得到black名字
		else 
			ItemName = obj.name;//得到item名字
	}
	//移动类
	public void IsMove(string One,GameObject game ,int x1,int y1,int x2,int y2){
	    GameObject parent1 = GameObject.Find (One);
		parent1.transform.parent = game.transform;
		parent1.transform.localPosition = Vector3.zero;
        Board.chess [y2, x2] = Board.chess[y1,x1];
        Board.chess [y1, x1] = 0;
	}
	//吃子类
	public void IsEat(string Frist,string sconde,int x1,int y1,int x2,int y2){
	GameObject Onename = GameObject.Find (Frist);//得到第一个
		GameObject Twoname = GameObject.Find (sconde);//得到第二个名字
		GameObject Twofather = Twoname.gameObject.transform.parent.gameObject;//得到第二个的父亲
		Onename.gameObject.transform.parent = Twofather.transform;
		Onename.transform.localPosition = Vector3.zero;
        //	Destroy (Twoname);
        //Twoname.transform.localPosition = new Vector3 (1000, 10000, 0);
        Board.chess [y2, x2] = Board.chess [y1, x1];
        Board.chess [y1, x1] = 0;
		GameObject a = GameObject.Find ("xiaoshi");
		Twoname.transform.parent = a.transform;
		Twoname.transform.localPosition = new Vector3(5000,5000,0);
	}
	//用来悔棋功能
	//点击事件
	//播放音乐
	 ChessChongzhi chzh = new ChessChongzhi();
	public void IsClickCheck(){	
		if (TrueOrFalse == false)
			return;
		GameObject obj = UICamera.hoveredObject;
		BlackNameOrRedName (obj);//是否点击到棋子  如果是  就得到棋子
		if (obj.name.Substring (0, 1) != "i")
			obj = obj.gameObject.transform.parent.gameObject;//得到他的父容器
		int x=System.Convert.ToInt32((obj.transform.localPosition.x)/130);
		int y = System.Convert.ToInt32(Mathf.Abs((obj.transform.localPosition.y)/128));
		int Result = IsBlackOrRed (x, y);//判断点击到了什么
		switch (Result) {
		case 0://点击到了空  是否要走棋
            //如果点击到了空格  就把对象清空
            for (int i = 1; i <= 90; i++)
            {
                GameObject Clear = GameObject.Find("prefabs" + i.ToString());
                Destroy(Clear);
            }
            ToY = y;
			ToX = x;
			if(ChessMove){//红色走
				if(RedName == null)
					return;
				string sssRed = RedName;//记录红色棋子的名字
			bool ba = rules.IsValidMove(Board.chess,FromX,FromY,ToX,ToY);
			if(!ba)
					return;

				int a = Board.chess[FromY,FromX];
				int b = Board.chess[ToY,ToX];
				chzh.AddChess(ChessChongzhi.Count,FromX,FromY,ToX,ToY,true,a,b);
				IsMove(RedName,obj,FromX,FromY,ToX,ToY);//走了
				str = "黑方走";
				KingPosition.JiangJunCheck();
				ChessMove = false;
				//getString();
				if(str=="红色棋子胜利")
					return ;//因为没有携程关系  每次进入黑色走棋的时候都判断 棋局是否结束
				BlackName = null;
				RedName = null;
				return;
			//执行走棋
			}
			else{//黑色走
				if(BlackName==null)
					return;
				bool ba = rules.IsValidMove(Board.chess,FromX,FromY,ToX,ToY);
				if(!ba)
					return;
				//ChessChongzhi chzh = new ChessChongzhi();
				int a = Board.chess[FromY,FromX];
				int b = Board.chess[ToY,ToX];
				chzh.AddChess(ChessChongzhi.Count,FromX,FromY,ToX,ToY,true,a,b);
				//看看是否能播放音乐
				IsMove(BlackName,obj,FromX,FromY,ToX,ToY);
				//黑色走棋
				ChessMove = true;
				str="红方走";
				KingPosition.JiangJunCheck();
			}
			break;
		case 1://点击到了黑色  是否选中  还是  红色要吃子
			if(!ChessMove){
				FromX = x;
				FromY = y;
                for (int i = 1; i <= 90; i++)
                {
                    GameObject Clear = GameObject.Find("prefabs" + i.ToString());
                    Destroy(Clear);
                }
                can.ClickChess(FromX,FromY);
			}
			else{
                for (int i = 1; i <= 90; i++)
                {
                    GameObject Clear = GameObject.Find("prefabs" + i.ToString());
                    Destroy(Clear);
                }
                if (RedName ==null)
					return;
				ToX = x;
				ToY = y;
				bool ba = rules.IsValidMove(Board.chess,FromX,FromY,ToX,ToY);
				if(!ba)
					return;
				int a = Board.chess[FromY,FromX];
				int b = Board.chess[ToY,ToX];
				chzh.AddChess(ChessChongzhi.Count,FromX,FromY,ToX,ToY,true,a,b);
				//看看是否能播放音乐
				IsEat(RedName,BlackName,FromX,FromY,ToX,ToY);
				ChessMove = false;
				//红色吃子  变黑色走
				str="黑方走";
				KingPosition.JiangJunCheck();
				if(str=="红色棋子胜利")
					return ;//因为没有携程关系  每次进入黑色走棋的时候都判断 棋局是否结束
                RedName=null;
				BlackName=null;
				return;
			}
			break;
		case 2://点击到了红色   是否选中  还是黑色要吃子
			if(ChessMove){
				FromX=x;
				FromY = y;
                for (int i = 1; i <= 90; i++)
                {
                    GameObject Clear = GameObject.Find("prefabs" + i.ToString());
                    Destroy(Clear);
                }
                can.ClickChess(FromX,FromY);
			}
			else{
                for (int i = 1; i <= 90; i++)
                {
                    GameObject Clear = GameObject.Find("prefabs" + i.ToString());
                    Destroy(Clear);
                }
                if (BlackName==null)
					return;
				ToX = x;
				ToY = y;
				bool ba = rules.IsValidMove(Board.chess,FromX,FromY,ToX,ToY);
				if(!ba)
					return;
				//ChessChongzhi chzh = new ChessChongzhi();
				int a = Board.chess[FromY,FromX];
				int b = Board.chess[ToY,ToX];
				chzh.AddChess(ChessChongzhi.Count,FromX,FromY,ToX,ToY,true,a,b);
				//看看是否能播放音乐
				IsEat(BlackName,RedName,FromX,FromY,ToX,ToY);
				RedName = null;
				BlackName = null;
				ChessMove = true;
				str="红方走";
				KingPosition.JiangJunCheck();
			}
			break;
	
		}
	
	}
}

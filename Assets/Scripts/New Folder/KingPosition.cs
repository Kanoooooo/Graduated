using UnityEngine;
using System.Collections;

public class KingPosition : MonoBehaviour {
	public static int Jiang_x,Jiang_y,Shuai_x,Shuai_y;

	public static  void IsPosition(){//得到将和帅的坐标
		for (int i=0; i<3; i++)
			for (int j=3; j<6; j++)
				if (Board.chess [i, j] == 1) {
				Jiang_x=j;
				Jiang_y = i;
			}
		for (int i=3; i<6; i++)
			for (int j=7; j<10; j++)
				if (Board.chess [j, i] == 8) {

				Shuai_x = i;
				Shuai_y = j;
			}
	}
	public static void JiangJunCheck(){//判断将和帅是否被将军了
		IsPosition ();
		if (Board.chess [Jiang_y, Jiang_x] != 1) {
//			blackclick.CanMove = false;
			blackclick.str = "红色方胜利";
			blackclick.TrueOrFalse = false;
            //联机模式下发送信息
            if(blackclick.bIsOnline)
            {
                GameObject game = GameObject.Find("Game") as GameObject;
                GameManager GM = game.GetComponent<GameManager>();
                GM.updateChess();
            }
			return;
		} else if (Board.chess [Shuai_y, Shuai_x] != 8) {
	//		blackclick.CanMove = false;
			blackclick.str="黑色方胜利";
			blackclick.TrueOrFalse= false;
            //联机模式下发送信息
            if (blackclick.bIsOnline)
            {
                GameObject game = GameObject.Find("Game") as GameObject;
                GameManager GM = game.GetComponent<GameManager>();
                GM.updateChess();
            }
            return ;
		}
		bool BOL;//bool 值
		for (int i=0; i<9; i++) {
			for(int j=0;j<10;j++){
				switch(Board.chess[j,i]){
                    //没有考虑同时被多个棋子将军
                    //case 2:
                    //	BOL= rules.IsValidMove(Board.chess,i,j,Shuai_x,Shuai_y);
                    //	if(BOL)
                    //		blackclick.str ="帅被車将军了";
                    //	break;
                    //case 3:
                    //	BOL= rules.IsValidMove(Board.chess,i,j,Shuai_x,Shuai_y);
                    //	if(BOL)
                    //		blackclick.str ="帅被马将军了";
                    //	break;
                    //case 4:
                    //	BOL= rules.IsValidMove(Board.chess,i,j,Shuai_x,Shuai_y);
                    //	if(BOL)
                    //		blackclick.str ="帅被炮将军了";
                    //	break;

                    //case 7:
                    //	BOL= rules.IsValidMove(Board.chess,i,j,Shuai_x,Shuai_y);
                    //	if(BOL)
                    //		blackclick.str ="帅被兵将军了";
                    //	break;
                    //case 9:
                    //	BOL= rules.IsValidMove(Board.chess,i,j,Jiang_x,Jiang_y);
                    //	if(BOL)
                    //		blackclick.str ="将被車将军了";
                    //	break;
                    //case 10:
                    //	BOL= rules.IsValidMove(Board.chess,i,j,Jiang_x,Jiang_y);
                    //	if(BOL)
                    //		blackclick.str ="将被马将军了";
                    //	break;
                    //case 11:
                    //	BOL= rules.IsValidMove(Board.chess,i,j,Jiang_x,Jiang_y);
                    //	if(BOL)
                    //		blackclick.str ="将被炮将军了";
                    //	break;
                    //case 14:
                    //	BOL= rules.IsValidMove(Board.chess,i,j,Jiang_x,Jiang_y);
                    //	if(BOL)
                    //		blackclick.str ="将被兵将军了";
                    //	break;

                    case 2:
                    case 3:
                    case 4:
                    case 7:
                    case 9:
                    case 10:
                    case 11:
                    case 14:
                        BOL = rules.IsValidMove(Board.chess, i, j, Jiang_x, Jiang_y);
                        if (BOL)
                            blackclick.str = "将军";
                        break;
                }
        }
		}

	}

}

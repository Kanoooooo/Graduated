using UnityEngine;
using System.Collections;

public class Blackmove : MonoBehaviour {

	//判断一个棋子是不是黑色
	public bool IsBlack(int x){
		if (x > 0 && x < 8)
			return true;
		else
			return false;
	}

	//判断一个棋子是不是红色
	public bool IsRed(int x){
		if (x >= 8 && x < 15)
			return true;
		else
			return false;
	}

	//判断两个棋子是不是同颜色
	public bool IsSameSide(int x,int y){
		if (IsBlack (x) && IsBlack (y) || IsRed (x) && IsRed (y))
			return true;
		else
			return false;
	}

}

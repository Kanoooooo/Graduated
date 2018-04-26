using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineGoBack : MonoBehaviour {

    public UILabel Label;

    //悔棋脚本
    public ChessChongzhi CC;

    public void RequestGoBack()
    {
        //发送请求
        Hashtable values = new Hashtable();
        values.Add("name", "piece");
        values.Add("FromX", -1);
        values.Add("FromY", -1);
        values.Add("ToX", -1);
        values.Add("ToY", -1);
        values.Add("Move", "str");
        values.Add("YidongOrChizi", "YidongOrChizi");
        values.Add("Regame", 0);
        values.Add("GoBack", 1);//申请悔棋，0不申请不同意，1申请，2同意

        Label.gameObject.SetActive(true);
        Label.text = "对方考虑中...";

        GobangClient.send(GameSerialize.toBytes(values));
    }

    //同意悔棋
    public void Agree()
    {
        //发送请求
        Hashtable values = new Hashtable();
        values.Add("name", "piece");
        values.Add("FromX", -1);
        values.Add("FromY", -1);
        values.Add("ToX", -1);
        values.Add("ToY", -1);
        values.Add("Move", "str");
        values.Add("YidongOrChizi", "YidongOrChizi");
        values.Add("Regame", 0);
        values.Add("GoBack", 2);//申请悔棋，0不申请不同意，1申请，2同意
        // 悔棋后，切换回合
        if (GameManager.curTurn == "Red")
        {
            GameManager.curTurn = "Black";
        }
        else
        {
            GameManager.curTurn = "Red";
        }
        GobangClient.send(GameSerialize.toBytes(values));
        CC.IloveHUIQI();
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    //拒绝悔棋
    public void DisAgree()
    {     
        //发送请求
        Hashtable values = new Hashtable();
        values.Add("name", "piece");
        values.Add("FromX", -1);
        values.Add("FromY", -1);
        values.Add("ToX", -1);
        values.Add("ToY", -1);
        values.Add("Move", "str");
        values.Add("YidongOrChizi", "YidongOrChizi");
        values.Add("Regame", 0);
        values.Add("GoBack", 0);//申请悔棋，0不申请不同意，1申请，2同意

        GobangClient.send(GameSerialize.toBytes(values));
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}

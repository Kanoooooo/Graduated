using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    //创建棋盘数组
    public static int[,] chess = new int[10, 9]{

        {2,3,6,5,1,5,6,3,2},
        {0,0,0,0,0,0,0,0,0},
        {0,4,0,0,0,0,0,4,0},
        {7,0,7,0,7,0,7,0,7},
        {0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0},
        {14,0,14,0,14,0,14,0,14},
        {0,11,0,0,0,0,0,11,0},
        {0,0,0,0,0,0,0,0,0},
        {9,10,13,12,8,12,13,10,9}

    };

    public static bool start = true;
    
    public void text()
    {                                  //动态添加了90个sprite 并且给它们位置等信息
        //UIAtlas atlas;
        int xx = 0, yy = 0;
        GameObject a = GameObject.Find("Chess");
        //atlas = Resources.Load("") as UIAtlas;   //让他不能找到图片集合
        for (int i = 1; i <= 90; i++)
        {
            GameObject ite = (GameObject)Instantiate(Resources.Load("item"));//找到预设体
            ite.transform.parent = a.transform;           //给预设体指定添加到什么地方
            GameObject b = GameObject.Find(ite.name);    //找到这个预设体的名字，给他做一些操作
            b.transform.localScale = new Vector3(1, 1, 1);
            b.name = "item" + i.ToString();                                           //suoyou所有的深度 都是5
            b.transform.localPosition = new Vector3(xx-263, yy+302, 0);
            xx += 195;
            if (xx >= 1755)
            {
                yy -= 192;
                xx = 0;
            }
        }
    }
    public static void xiangqizi(string sql, GameObject game, string name, int count)
    {
        /// P/	/// </summary>引用prefab 生成象棋的棋子
        /// 
        /// 
        GameObject a = (GameObject)Instantiate(Resources.Load(sql));
        a.transform.parent = game.transform;
        //GameObject b = GameObject.Find(a.name);
        a.name = name + count.ToString();
        a.transform.localPosition = new Vector3(0, 0, 0);
        a.transform.localScale = new Vector3(1, 1, 1);
    }

    public void chessposition()
    {
        Board.chess = new int[10, 9]{  //此注释要取消
			{2,3,6,5,1,5,6,3,2},
            {0,0,0,0,0,0,0,0,0},
            {0,4,0,0,0,0,0,4,0},
            {7,0,7,0,7,0,7,0,7},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {14,0,14,0,14,0,14,0,14},
            {0,11,0,0,0,0,0,11,0},
            {0,0,0,0,0,0,0,0,0},
            {9,10,13,12,8,12,13,10,9}
        };
        if (start == false)
        {
            return;
        }
        start = false;
        text();
        int count = 1;
        for (int i = 1; i <= 90; i++)
        {
            GameObject obj = GameObject.Find("item" + i.ToString());
            //	All.Add(obj);
            int x = System.Convert.ToInt32((obj.transform.localPosition.x+263) / 195);
            int y = System.Convert.ToInt32(Mathf.Abs((obj.transform.localPosition.y-302) / 192));
            switch (chess[y, x])
            {
                case 1:
                    count++;
                    xiangqizi("black_jiang", obj, "b_jiang", count);
                    break;
                case 2:
                    count++;
                    xiangqizi("black_ju", obj, "b_ju", count);
                    break;
                case 3:
                    count++;
                    xiangqizi("black_ma", obj, "b_ma", count);
                    break;
                case 4:
                    count++;
                    xiangqizi("black_pao", obj, "b_pao", count);
                    break;
                case 5:
                    count++;
                    xiangqizi("black_shi", obj, "b_shi", count);
                    break;
                case 6:
                    count++;
                    xiangqizi("black_xiang", obj, "b_xiang", count);
                    break;
                case 7:
                    count++;
                    xiangqizi("black_bing", obj, "b_bing", count);
                    break;
                case 8:
                    count++;
                    xiangqizi("red_shuai", obj, "r_shuai", count);
                    break;
                case 9:
                    count++;
                    xiangqizi("red_ju", obj, "r_ju", count);
                    break;
                case 10:
                    count++;
                    xiangqizi("red_ma", obj, "r_ma", count);
                    break;
                case 11:
                    count++;
                    xiangqizi("red_pao", obj, "r_pao", count);
                    break;
                case 12:
                    count++;
                    xiangqizi("red_shi", obj, "r_shi", count);
                    break;
                case 13:
                    count++;
                    xiangqizi("red_xiang", obj, "r_xiang", count);
                    break;
                case 14:
                    count++;
                    xiangqizi("red_bing", obj, "r_bing", count);
                    break;
            }
        }
    }
}

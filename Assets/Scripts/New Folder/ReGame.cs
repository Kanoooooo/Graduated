using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReGame : MonoBehaviour {

    ChessChongzhi chess = new ChessChongzhi();
    public void ChessPostion()
    {
        for (int i = 1; i <= 90; i++)
        {
            GameObject Clear = GameObject.Find("prefabs" + i.ToString());
            Destroy(Clear);
        }
        for (int i = ChessChongzhi.Count; i > 0; i--)
            chess.IloveHUIQI();

    }
}

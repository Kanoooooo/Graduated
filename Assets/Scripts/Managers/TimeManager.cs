using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public UISlider Timer;
    public bool bStartCul = true;
    public bool bGameOver = false;

    // Update is called once per frame
    void Update()
    {
        if(GameManager.curTurn == GameManager.userColor&&bStartCul)
        {
            StartCoroutine(ClucateTime());
        }

    }

    IEnumerator ClucateTime()
    {
        yield return new WaitForSeconds(0.1f);
        Timer.value -= 1 / 900f;
        if (Timer.value == 0)
        {
            bGameOver = true;
            //计时结束游戏失败
            GameObject game = GameObject.Find("Game") as GameObject;
            GameManager GM = game.GetComponent<GameManager>();
            GM.updateChess();
            Timer.value = 1;
            bStartCul = false;
        }
    }

    public void StopTime()
    {
        Timer.value = 1;
    }
}

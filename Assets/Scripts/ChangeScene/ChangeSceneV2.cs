using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class ChangeSceneV2 : MonoBehaviour {

    //获得按钮游戏对象
    public GameObject m_oStart;
    public GameObject m_oRestart;
    public GameObject m_oGoBack;
    public GameObject m_oExit;

    // Update is called once per frame
    void Update()
    {
        //点击对应按钮触发对应事件
        UIEventListener.Get(m_oStart).onClick = GameStart;
        UIEventListener.Get(m_oRestart).onClick = ReStart;
        UIEventListener.Get(m_oGoBack).onClick = GoBack;
        UIEventListener.Get(m_oExit).onClick = Exit;
    }

    //点击开始游戏按钮
    void GameStart(GameObject button)
    {

    }
    //点击重新开始按钮
    void ReStart(GameObject button)
    {

    }
    //点击悔棋按钮
    void GoBack(GameObject button)
    {
        
    }
    //点击退出游戏按钮
    void Exit(GameObject button)
    {
        Board.start = true;
        SceneManager.LoadScene("Start");
    }
}

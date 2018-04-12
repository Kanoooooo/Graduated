using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    //获得按钮游戏对象
    public GameObject m_oSign;
    public GameObject m_oLogin;
    public GameObject m_oStandAlone;
    public GameObject m_oExit;

    //头像
    GameObject DontDes;

    // Update is called once per frame
    void Update () {
        //点击对应按钮触发对应事件
        UIEventListener.Get(m_oSign).onClick = Sign;
        UIEventListener.Get(m_oLogin).onClick = Login;
        UIEventListener.Get(m_oStandAlone).onClick = StandAlone;
        UIEventListener.Get(m_oExit).onClick = Exit;

    }

    //点击注册按钮
    void Sign(GameObject button)
    {
        SceneManager.LoadScene("Test");
    }
    //点击登录按钮
    void Login(GameObject button)
    {
        //联机对战
        SceneManager.LoadScene("ChessDouble");
    }
    //点击单人游戏按钮
    void StandAlone(GameObject button)
    {
        SceneManager.LoadScene("ChessAlone");
    }
    //点击退出游戏按钮
    void Exit(GameObject button)
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Control : MonoBehaviour {

    //获得UIRoot和图片圈
    public GameObject m_oUIRoot;
    public GameObject m_oRect;
    //自定义图片按键
    public GameObject m_oPicture;
    //控制按钮开关
    bool bIsUIRootActive = true;
    //展示图
    public SpriteRenderer m_srSpriteRenderer;
    //物体不销毁
    public GameObject DontDes;
    static bool bIsDestoryAchieve = false;
    //返回键
    public GameObject m_oExit;

    //用户名
    public UILabel m_lLabel;
    string Name;

    public GUITexture mRectPortrait;
    Rect r1 = new Rect(100, 100, 300, 200);
    Rect r2 = new Rect(100, 300, 300, 200);
    Rect r3 = new Rect(100, 600, 300, 200);
    Rect r5 = new Rect(100, 300, 700, 100);

    string potolResult = "";

    //转场图片
    SpriteRenderer sr;

    // Use this for initialization
    void Start () {
        //设置默认图片
        Texture2D img = Resources.Load("1521968928188") as Texture2D;
        Sprite sp = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.0f, 0.0f));
        m_srSpriteRenderer.sprite = sp;
        //转场不销毁图片
        //第一次进入场景，设置物体不销毁
        if (!bIsDestoryAchieve)
        {
            bIsDestoryAchieve = true;
            DontDestroyOnLoad(DontDes);
        }
        //第二次进入场景，获得不销毁物体
        else
        {
            GameObject.Find("DontDestroyOnLoad").GetComponentInChildren<SpriteRenderer>().gameObject.SetActive(true);
            if (GameObject.Find("DontDestroyOnLoad").GetComponentInChildren<SpriteRenderer>()!=null)
            {
                m_srSpriteRenderer = GameObject.Find("DontDestroyOnLoad").GetComponentInChildren<SpriteRenderer>();
            }
        }
        //获得不销毁物体下的图形载体
        if (DontDes != null)
        {
            sr = DontDes.GetComponentInChildren<SpriteRenderer>();
        }

        //m_oCircle.SetActive(true);
        m_oUIRoot.SetActive(true);
        m_srSpriteRenderer.gameObject.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
        //实时更改用户名
        Name = m_lLabel.text;
        //点击对应按钮触发对应事件
        UIEventListener.Get(m_oPicture).onClick = Picture;
        UIEventListener.Get(m_oExit).onClick = Exit;
    }

    //点击完成设置按钮
    void Exit(GameObject button)
    {
        //更改图形载体层级，修改名字用来传递信息
        m_srSpriteRenderer.name = Name;
        SceneManager.LoadScene("Start");
    }

    //点击自定义图片按钮
    void Picture(GameObject button)
    {
        bIsUIRootActive = false;
        m_oUIRoot.SetActive(false);
    }

    void OnGUI()
    {
        if(bIsUIRootActive == false)
        {
            if (GUI.Button(r1, "拍照"))
            {
                InvokeCamera.instance.TakeRectClipPhoto(gameObject, "TakeRectClipPhoto");
            }
            if (GUI.Button(r2, "相册"))
            {
                InvokeCamera.instance.TakeRectClipPotolFromAlbum(gameObject, "TakeRectClipPotolFromAlbum");
            }
            if (GUI.Button(r3, "确定"))
            {
                bIsUIRootActive = true;
                m_oUIRoot.SetActive(true);
                //修改贴图
                Texture2D img = mRectPortrait.texture as Texture2D;
                Sprite sp = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0f, 0f));
                m_srSpriteRenderer.sprite = sp;
            }

            if (potolResult != "")
            {
                GUI.Label(r5, potolResult);
            }
        }

    }

    void TakeRectClipPhoto(string imageUrl)
    {
        StartCoroutine(_RectTexture(imageUrl));
    }

    void TakeRectClipPotolFromAlbum(string imageUrl)
    {
        StartCoroutine(_RectTexture(imageUrl));
    }

    IEnumerator _RectTexture(string imageName)
    {
        string path = "file://" + imageName;

        potolResult = path;

        WWW www = new WWW(path);
        while (!www.isDone)
        {

        }
        yield return www;
        //为贴图赋值

        mRectPortrait.texture = www.texture;
    }
}

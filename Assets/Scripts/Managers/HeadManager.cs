using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class HeadManager : MonoBehaviour {

    public UILabel uil;

    GameObject DontDes;

    private byte[] fssize;

    public static string ImgStr = "wu";

    // Use this for initialization
    void Start() {
        //设置默认头像
        SpriteRenderer m_srSR = gameObject.GetComponent<SpriteRenderer>();
        Texture2D img = Resources.Load("1521968928188") as Texture2D;
        Sprite sp = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.0f, 0.0f));
        m_srSR.sprite = sp;

        //设置自定义头像
        DontDes = GameObject.Find("DontDestroyOnLoad");
        if (DontDes != null)
        {
            SpriteRenderer SR = DontDes.GetComponentInChildren<SpriteRenderer>();
            if (!blackclick.bIsOnline)
            {
                GameObject name = GameObject.Find("Name");
                UILabel uil = name.GetComponent<UILabel>();
                uil.text = SR.gameObject.name;
            }
            if (SR != null)
            {
                m_srSR.sprite = SR.sprite;

            }

            //联机模式下发送图片信息
            if (blackclick.bIsOnline)
            {
                if (Control.Lujing!=null)
                {
                    FileStream fs = new FileStream(Control.Lujing, FileMode.Open);
                    uil.text = fs.ToString();
                    byte[] byt = new byte[fs.Length];
                    fs.Read(byt, 0,(int) byt.Length);
                    fs.Close();
                    fs.Dispose();
                    fs = null;
                    ImgStr = System.Convert.ToBase64String(byt);

                }
            }
        }
    }

}

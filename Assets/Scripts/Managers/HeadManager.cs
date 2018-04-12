using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadManager : MonoBehaviour {

    GameObject DontDes;

    // Use this for initialization
    void Start () {
        //设置默认头像
        SpriteRenderer m_srSR = gameObject.GetComponent<SpriteRenderer>();
        Texture2D img = Resources.Load("1521968928188") as Texture2D;
        Sprite sp = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.0f, 0.0f));
        m_srSR.sprite = sp;

        //设置自定义头像
        DontDes = GameObject.Find("DontDestroyOnLoad");
        if(DontDes!=null)
        {
            GameObject name = GameObject.Find("Name");
            UILabel uil = name.GetComponent<UILabel>();
            SpriteRenderer SR = DontDes.GetComponentInChildren<SpriteRenderer>();
            uil.text = SR.gameObject.name;
            if (SR != null)
            {
                m_srSR.sprite = SR.sprite;
            }
        }

    }
	
}

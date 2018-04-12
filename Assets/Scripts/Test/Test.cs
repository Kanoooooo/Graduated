using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    //展示图
    public SpriteRenderer m_oUiSpriteRenderer;

    public GUITexture mRectPortrait;
	public MeshRenderer mCirclePortrait;

	Rect r1 = new Rect(100,100,100,100);
	Rect r2 = new Rect(100,200,100,100);
	Rect r3 = new Rect(250,100,100,100);
	Rect r4 = new Rect(250,200,100,100);
	Rect r5 = new Rect(100,300,700,100);
	Rect r6 = new Rect(100,400,100,100);

    Rect r7 = new Rect(100, 600, 300, 200);

    string potolResult = "";

	void OnGUI(){
		if(GUI.Button(r1, "圆形_拍照")){
			InvokeCamera.instance.TakeCircleClipPhoto (gameObject, "TakeCircleClipPhoto");
		}
		if(GUI.Button(r2, "圆形_相册")){
			InvokeCamera.instance.TakeCircleClipPotolFromAlbum(gameObject, "TakeCircleClipPotolFromAlbum");
		}

		if(GUI.Button(r3, "矩形_拍照")){
			InvokeCamera.instance.TakeRectClipPhoto(gameObject, "TakeRectClipPhoto");
		}
		if(GUI.Button(r4, "矩形_相册")){
			InvokeCamera.instance.TakeRectClipPotolFromAlbum(gameObject, "TakeRectClipPotolFromAlbum");
		}

        if (GUI.Button(r7, "确定"))
        {
            //修改贴图
            Texture2D img = mCirclePortrait.material.mainTexture as Texture2D;
            Sprite sp = Sprite.Create(img, new Rect(0, 0, img.width, img.height), new Vector2(0.0f, 0.0f));
            m_oUiSpriteRenderer.sprite = sp;
        }
        if (potolResult != ""){
			GUI.Label (r5, potolResult);
		}
	}

	void TakeCircleClipPhoto(string imageUrl){
		StartCoroutine (_CircleTexture(imageUrl));
	}

	void TakeCircleClipPotolFromAlbum(string imageUrl){
		StartCoroutine (_CircleTexture(imageUrl));
	}

	void TakeRectClipPhoto(string imageUrl){
		StartCoroutine (_RectTexture(imageUrl));
	}

	void TakeRectClipPotolFromAlbum(string imageUrl){
		StartCoroutine (_RectTexture(imageUrl));
	}

	IEnumerator _RectTexture(string imageName){
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

	IEnumerator _CircleTexture(string imageName){
		string path = "file://" + imageName;

		potolResult = path;

		WWW www = new WWW(path);
		while (!www.isDone)
		{

		}
		yield return www;
		//为贴图赋值

		mCirclePortrait.material.mainTexture = www.texture;
	}

}

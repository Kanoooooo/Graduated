using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TestLabel : MonoBehaviour {


    public UILabel m_lLabel;

    string Name;
	
	// Update is called once per frame
	void Update () {
        Name = m_lLabel.text;
    }
}

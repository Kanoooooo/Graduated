using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shuzuTest : MonoBehaviour {

    int[,] test = new int[2,3]
    {
        {3,1,1},
        {2,2,5}
    };
    private void Start()
    {
       // Debug.Log(test[1, 2]);
       for(int i=0; i<3; i++)
        {
            Debug.Log(i);
        }
    }

}

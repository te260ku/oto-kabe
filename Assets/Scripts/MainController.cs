using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public enum STEP {
        NONE = -1, 
        WAIT,
        PLAY,  
        NUM, 
    }

    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) {
            
        }
    }
}

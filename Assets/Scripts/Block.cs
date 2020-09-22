using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public enum STEP {
        NONE = -1, 
        IDLE = 0, 
        FLASH, 
        NUM, 
    }
    public STEP step;
    void Start()
    {
        step = STEP.IDLE;
    }

    
    void Update()
    {
        
    }

    
}

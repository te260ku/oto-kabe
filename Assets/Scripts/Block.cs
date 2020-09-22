using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public enum STEP {
        NONE = -1, 
        IDLE = 0, 
        ACTIVE, 
        NUM, 
    }
    public STEP step;
    private float timer = 0.0f;
    void Start()
    {
        step = STEP.IDLE;
    }

    
    void Update()
    {
        switch (step)
        {
            case STEP.ACTIVE:
                timer += Time.deltaTime;
                break;
            default:
                break;
        }

        if (timer > 3f) {
            DeactivateBlock();
            timer = 0.0f;
        }
    }


    public void ActivateBlock() {
        step = STEP.ACTIVE;
        GetComponent<Renderer>().material.color = Color.blue;
    }
    public void DeactivateBlock() {
        step = STEP.IDLE;
        GetComponent<Renderer>().material.color = Color.white;
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public enum STATE {
        NONE = -1, 
        IDLE = 0, 
        ACTIVE, 
        NUM, 
    }
    public STATE state;
    private float timer = 0.0f;
    void Start()
    {
        state = STATE.IDLE;
    }

    
    void Update()
    {
        switch (state)
        {
            case STATE.ACTIVE:
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
        state = STATE.ACTIVE;
        GetComponent<Renderer>().material.color = Color.blue;
    }
    public void DeactivateBlock() {
        state = STATE.IDLE;
        GetComponent<Renderer>().material.color = Color.white;
    }

    
}

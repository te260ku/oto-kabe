using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;

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
    int blockID;
    public int BlockID
    {
        get { return blockID; }
        set { blockID = value; }
    }
    [SerializeField] TextMeshProUGUI idText;



    void Start()
    {
        
    }

    public void Initialize(int id) {
        state = STATE.IDLE;
        BlockID = id;
        idText.text = blockID.ToString();
    }

    
    void Update()
    {
        // switch (state)
        // {
        //     case STATE.ACTIVE:
        //         timer += Time.deltaTime;
        //         break;
        //     default:
        //         break;
        // }

        // if (timer > 0.5f) {
        //     DeactivateBlock();
        //     timer = 0.0f;
        // }
    }


    public void ActivateBlock() {
        if (state == STATE.ACTIVE) return;
        state = STATE.ACTIVE;
        GetComponent<Renderer>().material.color = Color.blue;
    }
    public void DeactivateBlock() {
        if (state == STATE.IDLE) return;
        state = STATE.IDLE;
        GetComponent<Renderer>().material.color = Color.white;
    }

  
    void OnTriggerEnter(Collider other)
    {
        OnHitHand();
    }

    public void OnHitHand() 
    {
        DeactivateBlock();
    }

    

    
}

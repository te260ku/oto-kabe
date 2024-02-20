using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using TMPro;

public class Block : MonoBehaviour
{
    public enum STATE {
        IDLE, 
        ACTIVE, 
        READY, 
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
    [SerializeField] Color readyColor;
    float readyEffectDuration;
    Renderer blockRenderer;



    void Start()
    {
        blockRenderer = GetComponent<Renderer>();
    }

    public void Initialize(int id) {
        state = STATE.IDLE;
        BlockID = id;
        idText.text = blockID.ToString();
    }

    
    void Update()
    {
        switch (state)
        {
            case STATE.ACTIVE:
                break;
            case STATE.READY:
                blockRenderer.material.color = Color.Lerp(blockRenderer.material.color, readyColor, readyEffectDuration * Time.deltaTime);
                break;
            default:
                break;
        }
        
    }


    public void ActivateBlock() {
        if (state == STATE.ACTIVE) return;
        state = STATE.ACTIVE;
        GetComponent<Renderer>().material.color = readyColor;
    }
    public void DeactivateBlock() {
        if (state == STATE.IDLE) return;
        state = STATE.IDLE;
        GetComponent<Renderer>().material.color = Color.white;
    }
    public void ReadyBlock(float duration) {
        if (state == STATE.READY) return;
        state = STATE.READY;
        PlayReadyBlockEffect(duration);
    }

    void PlayReadyBlockEffect(float duration) {
        readyEffectDuration = duration;
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

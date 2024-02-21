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
    [SerializeField] ParticleSystem onHitHandParticle;
    [SerializeField] GameObject bodyObj;
    public Vector3 BodyObjScale
    {
        get { return bodyObj.transform.localScale; }
    }



    void Start()
    {
        blockRenderer = bodyObj.GetComponent<Renderer>();
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
        blockRenderer.material.color = readyColor;
    }
    public void DeactivateBlock() {
        if (state == STATE.IDLE) return;
        state = STATE.IDLE;
        blockRenderer.material.color = Color.white;
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
        if (state != STATE.ACTIVE) return;
        DeactivateBlock();
        onHitHandParticle.Play();
    }

    

    
}

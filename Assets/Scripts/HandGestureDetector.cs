using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandGestureDetector : MonoBehaviour
{
    [SerializeField] OVRHand hand;
    [SerializeField] TextMeshProUGUI text;
    
    void Start()
    {
        
    }

    void Update()
    {
        text.text = Guu().ToString();
    }

    public bool Guu()
    {
        return hand.GetFingerPinchStrength(OVRHand.HandFinger.Index) >= 0.4f && hand.GetFingerPinchStrength(OVRHand.HandFinger.Middle) >= 0.4f;   
    }
}

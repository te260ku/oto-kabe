using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum HandGesture {
    None, ThumbsUp
}

public class HandGestureDetector : MonoBehaviour
{
    [SerializeField] OVRHand hand;
    [SerializeField] TextMeshProUGUI text;
    HandGesture currentHandGesture;
    public HandGesture CurrentHandGesture {
        get {
            return currentHandGesture;
        }
        set {
            currentHandGesture = value;
        }
    }
    
    
    void Start()
    {
        
    }

    void Update()
    {
        text.text = IsThumbsUp().ToString();

        if (IsThumbsUp()) {
            CurrentHandGesture = HandGesture.ThumbsUp;
        } else {
            CurrentHandGesture = HandGesture.None;
        }
        
    }

    bool IsThumbsUp() {
        return hand.GetFingerPinchStrength(OVRHand.HandFinger.Pinky) >=0.05f && hand.GetFingerPinchStrength(OVRHand.HandFinger.Ring) >= 0.2f;
    }
}

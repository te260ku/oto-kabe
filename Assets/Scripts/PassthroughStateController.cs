using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassthroughStateController : MonoBehaviour
{
    [SerializeField] OVRPassthroughLayer OVRPassthroughLayer;
    bool passthroughIsON;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SwitchPassthroughState() {
        if (passthroughIsON) {
            OVRPassthroughLayer.textureOpacity = 1.0f;
        } else {
            OVRPassthroughLayer.textureOpacity = 0.0f;
        }
        passthroughIsON = !passthroughIsON;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PassthroughStateController : MonoBehaviour
{
    [SerializeField] OVRPassthroughLayer OVRPassthroughLayer;
    [SerializeField] Color ONColor;
    [SerializeField] Color OFFColor;
    [SerializeField] TextMeshPro buttonText;
    bool passthroughIsON;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            SwitchPassthroughState();
        }
    }

    public void SwitchPassthroughState() {
        passthroughIsON = !passthroughIsON;
        string text = "Passthrough: ";
        if (passthroughIsON) {
            // オン
            OVRPassthroughLayer.textureOpacity = 1.0f;
            text += "ON";
        } else {
            // オフ
            OVRPassthroughLayer.textureOpacity = 0.0f;
            text += "OFF";
        }
        buttonText.text = text;
    }
}

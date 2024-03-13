using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PassthroughStateController : MonoBehaviour
{
    [SerializeField] OVRPassthroughLayer OVRPassthroughLayer;
    [SerializeField] Color ONColor;
    [SerializeField] Color OFFColor;
    [SerializeField] TextMeshPro buttonText;
    [SerializeField] Slider passthroughOpacitySlider;
    bool passthroughIsON;

    void Start()
    {
        passthroughOpacitySlider.onValueChanged.AddListener(ChangePassthroughOpacity);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            SwitchPassthroughState();
        }
    }

    void ChangePassthroughOpacity(float value) {
        OVRPassthroughLayer.textureOpacity = value;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HandUIController : MonoBehaviour
{
    [SerializeField] GameObject uiObj;
    [SerializeField] TextMeshProUGUI visibilitySwitcherButtonText;
    [SerializeField] string visibilitySwitcherButtonTextValueWithON;
    [SerializeField] string visibilitySwitcherButtonTextValueWithOFF;
    bool visibility;

    void Start()
    {
        visibility = true;
        visibilitySwitcherButtonText.text = visibilitySwitcherButtonTextValueWithOFF;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            SwitchVisibility();
        }
    }

    public void SwitchVisibility() {
        visibility = !visibility;
        uiObj.SetActive(visibility);
        visibilitySwitcherButtonText.text = visibility ? visibilitySwitcherButtonTextValueWithOFF : visibilitySwitcherButtonTextValueWithON;
    }
}

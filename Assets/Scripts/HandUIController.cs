using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandUIController : MonoBehaviour
{
    [SerializeField] GameObject uiObj;
    bool visibility;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SwitchVisibility() {
        visibility = !visibility;
        uiObj.SetActive(visibility);
    }
}

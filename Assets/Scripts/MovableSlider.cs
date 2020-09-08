using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovableSlider : MonoBehaviour {

    
    [SerializeField] private GameObject sliderParent;
    public float value;

    [SerializeField] public Text valueText;
    private void Start() {

    }
    private void Update() {
            this.transform.localPosition = new Vector3(
            Mathf.Clamp(transform.localPosition.x,-0.2f, 0.2f),
            transform.position.y,
            transform.position.z);
            
            this.transform.position = new Vector3(
                Mathf.Clamp(transform.position.x,-0.2f, 0.2f), 
                sliderParent.transform.position.y, 
                sliderParent.transform.position.z);

            value = this.transform.localPosition.x;

            valueText.text = "value: " + value.ToString();
    }
    
}


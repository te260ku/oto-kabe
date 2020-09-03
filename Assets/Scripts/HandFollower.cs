using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandFollower : MonoBehaviour
{
    [SerializeField] private GameObject rightHand;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        transform.position = rightHand.transform.position;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("collide");
        GameObject target = other.gameObject;
        if (target.tag == "Target") {
            target.GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.tag == "Target") {
            target.GetComponent<Renderer>().material.color = Color.white;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleFollower : MonoBehaviour
{
    [SerializeField] private GameObject rightHand;

    // 状態管理
    public bool isTouchingPlane;

    // 効果音
    private AudioSource audioSource;
    [SerializeField] private AudioClip hitBlockAudio;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        // transform.position = rightHand.transform.position;
        // transform.rotation = rightHand.transform.rotation;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("collide");
        GameObject target = other.gameObject;
        if (target.tag == "Target") {
            target.GetComponent<Renderer>().material.color = Color.blue;
            audioSource.PlayOneShot(hitBlockAudio);
        }

        if (target.tag == "StagePlane") {
            isTouchingPlane = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.tag == "Target") {
            target.GetComponent<Renderer>().material.color = Color.white;
        }

        if (target.tag == "StagePlane") {
            isTouchingPlane = false;
        }
    }


}

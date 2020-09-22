using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleFollower : MonoBehaviour
{
    [SerializeField] private GameObject rightHand;
    private OVRSkeleton rightSkelton;

    // 状態管理
    public bool isTouchingPlane;

    // 効果音
    private AudioSource audioSource;
    [SerializeField] private AudioClip hitBlockAudio;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rightSkelton = rightHand.GetComponent<OVRSkeleton>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        // transform.position = new Vector3 (
        //     rightHand.transform.position.x, 
        //     rightHand.transform.position.y+0.1f, 
        //     rightHand.transform.position.z
        // );
        // transform.rotation = rightHand.transform.localRotation;

        #if UNITY_EDITOR
        #else
            Vector3 indexTipPos = rightSkelton.Bones[(int)OVRSkeleton.BoneId.Hand_Middle1].Transform.position;
            Quaternion indexTipRot = rightSkelton.Bones[(int)OVRSkeleton.BoneId.Hand_Middle1].Transform.rotation;
            this.transform.position = indexTipPos;
            this.transform.rotation = indexTipRot;
        #endif
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

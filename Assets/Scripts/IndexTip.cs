using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndexTip : MonoBehaviour
{
    [SerializeField] private OVRHand rightHand;
    private OVRSkeleton rightSkelton;
    [SerializeField] private GameObject sliderParent;
    private bool isIndexPinching;
    private float thumbPinchStrength;


    // 状態管理
    public bool isGrabbbing;

    private void Start() {
        rightSkelton = rightHand.GetComponent<OVRSkeleton>();
        
    }
    
    void Update()
    {
 
        #if UNITY_EDITOR
        #else
            isIndexPinching = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
            thumbPinchStrength = rightHand.GetFingerPinchStrength(OVRHand.HandFinger.Thumb);
            isGrabbbing = true;
        #endif

        
        
    }

    private void FixedUpdate() {
        #if UNITY_EDITOR
        #else
            Vector3 indexTipPos = rightSkelton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position;
            Quaternion indexTipRot = rightSkelton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.rotation;
            this.transform.position = indexTipPos;
            this.transform.rotation = indexTipRot;
        #endif
        
    }

    void OnTriggerStay(Collider other)
    {
        
        GameObject target = other.gameObject;
        if (target.tag == "GrabbableObject") {
            
        if (thumbPinchStrength > 0.9)
        
        {
            other.gameObject.transform.parent = null;
            other.gameObject.transform.parent = this.transform;
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.gameObject.transform.localPosition = Vector3.zero;

            isGrabbbing = true;

        

        } else
        {

            
            other.gameObject.transform.parent = sliderParent.transform;
            isGrabbbing = false;
        }

        } 
        
    }

    void OnTriggerExit(Collider other)
    {
        
        GameObject target = other.gameObject;
        if (target.tag == "GrabbableObject") {
            other.gameObject.transform.parent = sliderParent.transform;
            isGrabbbing = false;
        }
    }

    
}

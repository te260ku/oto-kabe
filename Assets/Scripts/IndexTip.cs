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

    [SerializeField] Text text;
    [SerializeField] Text text2;

    private void Start() {
        rightSkelton = rightHand.GetComponent<OVRSkeleton>();
        
    }
    
    void Update()
    {
        isIndexPinching = rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        thumbPinchStrength = rightHand.GetFingerPinchStrength(OVRHand.HandFinger.Thumb);

        text2.text = thumbPinchStrength.ToString();

    }

    private void FixedUpdate() {
        Vector3 indexTipPos = rightSkelton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position;
        Quaternion indexTipRot = rightSkelton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.rotation;
        this.transform.position = indexTipPos;
        this.transform.rotation = indexTipRot;
    }

    void OnTriggerStay(Collider other)
    {
        text.text = "enter";
        GameObject target = other.gameObject;
        if (target.tag.Contains("Object")) {
            text.text = "find tag";
        if (thumbPinchStrength > 0)
        
        {
            other.gameObject.transform.parent = null;
            other.gameObject.transform.parent = this.transform;
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.gameObject.transform.localPosition = Vector3.zero;

            text.text = "make parent";

        } else
        {

            
            other.gameObject.transform.parent = sliderParent.transform;
        }

        } else {
            if (thumbPinchStrength>0.9)///つかんだ
        {
            other.gameObject.transform.parent = this.transform;
            // other.GetComponent<Rigidbody>().isKinematic = true;
            other.gameObject.transform.localPosition = Vector3.zero;

        }
        else///はなした
        {
            // other.GetComponent<Rigidbody>().isKinematic = false;
            other.transform.parent = null;

        }
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        text.text = "exit";
        GameObject target = other.gameObject;
        if (target.tag.Contains("Object")) {
            other.gameObject.transform.parent = sliderParent.transform;
        
        }
    }

    
}

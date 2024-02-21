using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoneFollower : MonoBehaviour
{
    [SerializeField] OVRSkeleton.BoneId targetBone;
    [SerializeField] OVRSkeleton targetHandSkeleton;

    void Start()
    {
        
    }

    void Update()
    {
        if (targetHandSkeleton.Bones.Count > 0) {
            Vector3 pos = targetHandSkeleton.Bones[(int)targetBone].Transform.position;
            Quaternion rot = targetHandSkeleton.Bones[(int)targetBone].Transform.rotation;
            transform.position = pos;
            transform.rotation = rot;
        }
        
    }
}

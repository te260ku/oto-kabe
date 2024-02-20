using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneFollowerController : MonoBehaviour
{
    [System.Serializable]
    class Test {
        [SerializeField] OVRSkeleton.BoneId bone;
    }
    // [SerializeField] List<Test> test = new List<Test>();
    [SerializeField] List<BoneFollower> boneFollowers = new List<BoneFollower>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    
}

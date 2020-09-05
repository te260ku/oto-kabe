using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneFollowersController : MonoBehaviour
{
    [SerializeField] private GameObject rightHand;
    [SerializeField] public GameObject middle1Sphere;
    [SerializeField] public GameObject indexTipSphere;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        middle1Sphere.transform.position = rightHand.transform.position;
        indexTipSphere.transform.position = rightHand.transform.position;
    }
}

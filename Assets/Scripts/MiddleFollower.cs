using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiddleFollower : MonoBehaviour
{
    [SerializeField] private GameObject rightHand;
    private OVRSkeleton rightSkelton;
    public MainController mainController;


    // 状態管理
    public bool isTouchingPlane;

    // 効果音
    private AudioSource audioSource;
    [SerializeField] private AudioClip hitBlockAudio;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rightSkelton = rightHand.GetComponent<OVRSkeleton>();
    }

    private void FixedUpdate() {
        #if UNITY_EDITOR
        #else
            Vector3 indexTipPos = rightSkelton.Bones[(int)OVRSkeleton.BoneId.Hand_Middle1].Transform.position;
            Quaternion indexTipRot = rightSkelton.Bones[(int)OVRSkeleton.BoneId.Hand_Middle1].Transform.rotation;
            this.transform.position = indexTipPos;
            this.transform.rotation = indexTipRot;
        #endif  
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray();
            RaycastHit hit = new RaycastHit();
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
            {
                if(hit.collider.gameObject.CompareTag("Target"))
                {
                    GameObject target = hit.collider.gameObject;
                    Block targetBlock = target.GetComponent<Block>();
                    if (targetBlock.state == Block.STATE.ACTIVE) {
                        targetBlock.DeactivateBlock();
                        // mainController.AddScore();
                        audioSource.PlayOneShot(hitBlockAudio);
                    }
                }
            }
        }
    }

    

    void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.tag == "Target") {
            Block targetBlock = target.GetComponent<Block>();
            if (targetBlock.state == Block.STATE.ACTIVE) {
                targetBlock.DeactivateBlock();
                audioSource.PlayOneShot(hitBlockAudio);
                // mainController.AddScore();
            }
        }

        if (target.tag == "StagePlane") {
            isTouchingPlane = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.tag == "Target") {
            // target.GetComponent<Renderer>().material.color = Color.white;
        }

        if (target.tag == "StagePlane") {
            isTouchingPlane = false;
        }
    }


}

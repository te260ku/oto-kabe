using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HandTracking : MonoBehaviour
{
    [SerializeField] private Text text_isIndexPinching;
    [SerializeField] private Text text_middlePinchStrength;
    [SerializeField] private OVRHand rightHand;
    [SerializeField] private OVRHand leftHand;
    private OVRSkeleton rightHandSkeleton;
    [SerializeField] private GameObject handMenu;
    [SerializeField] private GameObject targetPrefab;

    // デバッグ
    [SerializeField] private Text handPoseText;
    [SerializeField] private Text thumbText;    
    

    
    
    private bool isLeftRingPinching;
    private bool releasedIndex;
    private bool releasedMiddle;
    private float OpenPosetimerCount = 0.0f;
    private float middleDoubleTaptimerCount = 0.0f;
    private float tapTime;
    private float doubleTapTime = 0.25f;
    

    // ハンドメニュー
    private bool enabledhandMenu;
    private Animator handMenuAnim;

    // 効果音
    private AudioSource audioSource;
    [SerializeField] AudioClip showhandMenuAudio;
    [SerializeField] AudioClip hidehandMenuAudio;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();  

        // ハンドメニューの初期化 
        handMenu.SetActive(false);
        enabledhandMenu = false;
        handMenuAnim = handMenu.GetComponent<Animator>();

        // ハンドトラッキング 
        rightHandSkeleton = rightHand.GetComponent<OVRSkeleton>();

        tapTime = Time.time;
    }


    
    
    void Update()
    {
        if ((leftHand.GetFingerIsPinching(OVRHand.HandFinger.Middle) && leftHand.GetFingerIsPinching(OVRHand.HandFinger.Thumb))
         || Input.GetKey(KeyCode.Space))
        {
            isLeftRingPinching = true;
        } else {
            isLeftRingPinching = false;
        }

        if ((leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index) && leftHand.GetFingerIsPinching(OVRHand.HandFinger.Thumb))
        || Input.GetMouseButton(0))
        {
            if (releasedMiddle) {
                float interval = Time.time - tapTime;
                if (interval < doubleTapTime) {
                    // GameObject target = Instantiate(targetPrefab).gameObject;
                    targetPrefab.transform.position = new Vector3(
                        leftHand.transform.position.x, 
                        leftHand.transform.position.y-0.15f, 
                        leftHand.transform.position.z);
            }
            tapTime = Time.time;
            releasedMiddle = false;
            }

            
            
                        
            
        } else {
            releasedMiddle = true;
        }

        
        

        if (isLeftRingPinching) {
            if (!enabledhandMenu) {
                audioSource.PlayOneShot(showhandMenuAudio);
                handMenu.SetActive(true);
                handMenuAnim.SetBool("Show", true);
                
                
                enabledhandMenu = true;
            }
        } else {
            if (enabledhandMenu) {
                audioSource.PlayOneShot(hidehandMenuAudio);
                handMenuAnim.SetBool("Show", false);
                
                // handMenu.SetActive(false);
                enabledhandMenu = false;
            }
        }

        // 右手を開いているかどうか
        // thumbText.text = rightHand.GetFingerPinchStrength(OVRHand.HandFinger.Thumb).ToString();
        // if (CheckOpenPose) {
        //     handPoseText.text = "Pose: Open";

        //     OpenPosetimerCount += Time.deltaTime;
 
        //     if (OpenPosetimerCount >= 5f)
        //     {
        //         handPoseText.text = "Pose: Open 5 seconds";
                
        //     }
        // } else {
        //     handPoseText.text = "Pose: None";
        //     OpenPosetimerCount = 0.0f;
        // }

        
    }

    public bool CheckOpenPose
    {
        get
        {
            return (rightHand.GetFingerPinchStrength(OVRHand.HandFinger.Thumb) < 0.01f 
            && rightHand.GetFingerPinchStrength(OVRHand.HandFinger.Index) < 0.01f
            && rightHand.GetFingerPinchStrength(OVRHand.HandFinger.Middle) < 0.01f
            && rightHand.GetFingerPinchStrength(OVRHand.HandFinger.Ring) < 0.01f
            && rightHand.GetFingerPinchStrength(OVRHand.HandFinger.Pinky) < 0.01f);
        }
    }

    void HideHandMenu() {
        handMenu.SetActive(false);        
    }


    

    
}

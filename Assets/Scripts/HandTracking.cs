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
    [SerializeField] private GameObject HandMenu;
    [SerializeField] private Text text_HandMenuPosiotionX;
    
    // state
    private bool isLeftIndexPinching;
    private bool enabledHandMenu;
    private float HandMenuPositionX;
    private bool releasedIndex;
    private bool releasedMiddle;

    // audio
    private AudioSource audioSource;
    [SerializeField] AudioClip showHandMenuAudio;
    [SerializeField] AudioClip HideHandMenuAudio;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();  

        // ハンドメニューの初期化 
        HandMenu.SetActive(false);
        enabledHandMenu = false;
    }
    
    
    void Update()
    {
        if ((leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index) && leftHand.GetFingerIsPinching(OVRHand.HandFinger.Thumb))
         || Input.GetKey(KeyCode.Space))
        {
                isLeftIndexPinching = true;
        } else {
                isLeftIndexPinching = false;
        }

        if (isLeftIndexPinching) {
            if (!enabledHandMenu) {
                audioSource.PlayOneShot(showHandMenuAudio);
                HandMenu.SetActive(true);
                enabledHandMenu = true;
            }
        } else {
            if (enabledHandMenu) {
                audioSource.PlayOneShot(HideHandMenuAudio);
                HandMenu.SetActive(false);
                enabledHandMenu = false;
            }
        }
        
        



        
    }


    

    
}

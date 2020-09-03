using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandMenu : MonoBehaviour
{
    private Animator handMenuAnim;
    // 効果音
    private AudioSource audioSource;
    [SerializeField] AudioClip showhandMenuAudio;
    [SerializeField] AudioClip hidehandMenuAudio;
    [SerializeField] Text text_pushedButton;
    [SerializeField] GameObject targetPrefab;
    void Start()
    {

    }

 
    void Update()
    {
        
    }

    void HideHandMenu() {
        gameObject.SetActive(false);
    }

    public void ButtonPush(int buttonNum) {
        text_pushedButton.text = "Button " + buttonNum.ToString();
        // GameObject target = Instantiate(targetPrefab).gameObject;
        // target.transform.position = new Vector3(transform.position.x, transform.position.y-0.15f, transform.position.z);

    }
}

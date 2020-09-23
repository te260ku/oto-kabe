using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;

public class StageGenerator : MonoBehaviour
{
    [SerializeField] private GameObject stagePlanePrefab;
    [SerializeField] private GameObject middleSphere;
    private MiddleFollower middleFollower;
    [SerializeField] private IndexTip indexTip;


    private Transform planeTransform;
    private Vector3[] anchorPos = new Vector3[10];
    private GameObject[] anchors = new GameObject[10];
    private int anchorCount;
    [SerializeField] private GameObject anchorPrefab;
    [SerializeField] private GameObject stagePrefab;
    [SerializeField] private GameObject anchorsParent;
    private GameObject stage;


    [SerializeField] private ButtonController _buttonController;
    [SerializeField] private MovableSlider widthSlider;
    [SerializeField] private MovableSlider heightSlider;

    // 状態管理
    private bool existsPlane;

    // 効果音
    private AudioSource audioSource;
    [SerializeField] private AudioClip pushButtonAudio;
    [SerializeField] private AudioClip bgm1;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        middleFollower = middleSphere.GetComponent<MiddleFollower>();

        

        _buttonController.ActionZoneEvent += args =>
        {
            if (args.InteractionT == InteractionType.Enter)
            {
                SetAnchor(middleSphere);
            }
        };

        
    }

    float Map(float value, float start1, float stop1, float start2, float stop2)
    {
        return start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {
            CreatePlane(middleSphere);
        } else if (Input.GetKeyDown(KeyCode.W)) {
            SetAnchor(middleSphere);
        } else if (Input.GetKeyDown(KeyCode.E)) {
            audioSource.clip = bgm1;
            audioSource.Play();
            CreateStage();
        } else if (Input.GetKeyDown(KeyCode.T)) {
            
            float eulerAngleX = planeTransform.rotation.eulerAngles.y;
            eulerAngleX = Mathf.Clamp(eulerAngleX, 0f, 360f);
            Debug.Log(eulerAngleX);
        }

        if (indexTip.isGrabbbing) {
            
            float width = widthSlider.value;
            float height = heightSlider.value;
            
            float mappedWidth = Map(width, -0.2f, 0.2f, 0.05f, 0.3f);
            float mappedHeight = Map(height, -0.2f, 0.2f, 0.05f, 0.3f);

            

            if (stage) {
                stage.transform.localScale = new Vector3(
                mappedWidth, 
                0.5f, 
                mappedHeight
            );
            }
            
        }
        
    }

    public void CreatePlane(GameObject middleSphere) {

        if (!existsPlane) {
            existsPlane = true;
            GameObject blockPlane = Instantiate(stagePlanePrefab);
            blockPlane.transform.position = middleSphere.transform.position;
            blockPlane.transform.rotation = middleSphere.transform.rotation;

            // anchorの親オブジェクトをplaneの傾きに合わせる
            anchorsParent.transform.rotation = blockPlane.transform.rotation;

            planeTransform = blockPlane.transform;

        }
        
    }

    public void ButtonPush(int buttonNum) {

        if (buttonNum == 1) {
            audioSource.PlayOneShot(pushButtonAudio);
            // planeボタン
            if (!existsPlane) {
                CreatePlane(middleSphere);
                existsPlane = true;
            }
            
        } else if (buttonNum == 2) {
            audioSource.PlayOneShot(pushButtonAudio);
                    // anchorボタン
                    if (middleFollower.isTouchingPlane) {
                        SetAnchor(middleSphere);
                    }
                    
                    
        } else if (buttonNum == 3) {
            audioSource.PlayOneShot(pushButtonAudio);
                    // stageボタン
                    if (middleFollower.isTouchingPlane) {
                        CreateStage();
                    }
                    
        }
    }

    public void SetAnchor(GameObject middleSphere) {

        if (middleFollower.isTouchingPlane) {
            anchorPos[anchorCount] = middleSphere.transform.position;

            GameObject anchor = Instantiate(anchorPrefab);
            anchor.transform.position = anchorPos[anchorCount];
            anchor.transform.parent = anchorsParent.transform;

            anchors[anchorCount] = anchor;
            
            anchorCount++;
        }
    }

    public void CreateStage() {

        if (!existsPlane) {
            existsPlane = true;
            GameObject originalStage = Instantiate(stagePrefab);
            
            originalStage.transform.position = middleSphere.transform.position;
            originalStage.transform.rotation = middleSphere.transform.rotation;

            // anchorの親オブジェクトをplaneの傾きに合わせる
            anchorsParent.transform.rotation = originalStage.transform.rotation;
            planeTransform = originalStage.transform;

            stage = originalStage;

        }

        // adashを算出する
        Vector3 vec = ( anchors[2].transform.position - anchors[0].transform.position ).normalized;
        Vector3 ab = anchors[1].transform.position-anchors[0].transform.position;
        float dist = Vector3.Distance(anchors[1].transform.position, anchors[0].transform.position);
        Vector3 ac = anchors[2].transform.position-anchors[0].transform.position;
        float dot = Vector3.Dot(ab, ac);
        float tmp = dot/Mathf.Pow(dist, 2);
        Vector3 adash = anchors[2].transform.position - tmp*ab;
        GameObject adashAnchor = Instantiate(anchorPrefab);
        adashAnchor.transform.parent = anchorsParent.transform;
        adashAnchor.transform.position = adash;
        // adashAnchor.transform.rotation = Quaternion.FromToRotation(Vector3.up, vec);

        // ステージの中心点を算出する
        Vector3 centerPos = new Vector3 (
            (anchorPos[1].x + adash.x) / 2,  
            (anchorPos[1].y + adash.y) / 2,  
            (anchorPos[1].z + adash.z) / 2);
        GameObject centerAnchor = Instantiate(anchorPrefab);
        centerAnchor.transform.parent = anchorsParent.transform;
        centerAnchor.transform.position = centerPos;
        // centerAnchor.transform.rotation = planeTransform.rotation;

        // ステージの高さと幅を算出する
        float width = Vector3.Distance(anchorPos[0], anchorPos[1]);
        float height = Vector3.Distance(anchorPos[0], adashAnchor.transform.position);

        Debug.Log("width: " + width);
        Debug.Log("height: " + height);


        // ステージを生成する
        // GameObject stage = Instantiate(stagePrefab);
        stage = Instantiate(stagePrefab);

        // ステージの座標を中心点に合わせる
        stage.transform.position = centerAnchor.transform.position;
        
        stage.transform.parent = planeTransform.transform;

        // ステージの大きさを調整する
        stage.transform.localScale = new Vector3(width*3, 0.1f, height*3);

        // ステージの傾きを調整する
        float planeRotY = planeTransform.rotation.eulerAngles.y;
        // if (180f < planeRotY) {
        //     planeRotY = 360 - planeRotY;
        // }
        float stageYaw = Vector3.Angle(anchors[0].transform.position-anchors[1].transform.position, Vector3.right);
        Vector3 stageRot = new Vector3(
            0, 
            -planeRotY-stageYaw, 
            0
        );
        
        Debug.Log(planeRotY);
        Debug.Log(stageYaw);
        stage.transform.localRotation = Quaternion.Euler(stageRot);

        stage.transform.localRotation = Quaternion.Euler(new Vector3(
            0f, 
            -planeRotY, 
            0f
        ));
        stage.transform.Rotate(0f, -planeRotY, 0f);
            
    }
}

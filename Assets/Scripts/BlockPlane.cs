using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlane : MonoBehaviour
{
    
    private Vector3[] anchorsPos = new Vector3[5];
    private int anchorCount;
    [SerializeField] private GameObject anchorPrefab;
    [SerializeField] private GameObject stagePrefab;
    private GameObject anchorsParent;
    private GameObject[] anchors = new GameObject[5];
    
    // Start is called before the first frame update
    void Start()
    {
        anchorsParent = GameObject.Find("AnchorsParent");
        anchorsParent.transform.rotation = transform.rotation;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GameObject stage = Instantiate(stagePrefab);
            stage.transform.position = anchorsPos[0];
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            anchors[0].transform.localPosition = new Vector3(
                anchors[0].transform.localPosition.x, 
                anchors[0].transform.localPosition.y, 
                anchors[0].transform.localPosition.z + 0.1f
            );
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            Vector3 centerPosLocal = new Vector3 (
                (anchors[1].transform.localPosition.x + anchors[0].transform.localPosition.x) / 2,  
                (anchors[1].transform.localPosition.y + anchors[0].transform.localPosition.y) / 2, 
                (anchors[2].transform.localPosition.z + anchors[0].transform.localPosition.z) / 2);
            Vector3 centerPos = new Vector3 (
                (anchorsPos[1].x + anchorsPos[0].x) / 2,  
                (anchorsPos[2].y + anchorsPos[0].y) / 2,  
                (anchorsPos[2].z + anchorsPos[0].z) / 2);

            Debug.Log(centerPos);
            Debug.Log(centerPosLocal);

            GameObject centerAnchor = Instantiate(anchorPrefab);
            
            float width = Vector3.Distance(anchorsPos[0], anchorsPos[1]);
            // float height = Vector3.Distance(anchorsPos[0], anchorsPos[2]);
            // float width = (Mathf.Abs(anchorsPos[0].x-anchorsPos[1].x))/2;

            float height = (Mathf.Abs(anchorsPos[0].z-anchorsPos[2].z))/2;
            Debug.Log("width: " + width);
            Debug.Log("height: " + height);
            
            centerAnchor.transform.parent = anchorsParent.transform;
            centerAnchor.transform.position = centerPos;
            centerAnchor.transform.rotation = transform.rotation;

            GameObject stage = Instantiate(stagePrefab);
            // ***heightをどうするか
            stage.transform.localScale = new Vector3(width/3, 0.1f, height/3);
            stage.transform.position = centerAnchor.transform.position;

            stage.transform.parent = transform;

            Vector3 planeRot = transform.rotation.eulerAngles;
            Vector3 center01 = new Vector3 (
                (anchors[1].transform.localPosition.x + anchors[0].transform.localPosition.x) / 2,  
                (anchors[1].transform.localPosition.y + anchors[0].transform.localPosition.y) / 2,  
                (anchors[1].transform.localPosition.z + anchors[0].transform.localPosition.z) /2
            );

            Vector3 proj0 = Vector3.ProjectOnPlane(anchors[0].transform.localPosition, transform.position.normalized);
            Vector3 proj1 = Vector3.ProjectOnPlane(anchors[1].transform.localPosition, transform.position.normalized);
            
            // float stageYaw = Vector3.SignedAngle(anchorsPos[0], anchorsPos[1], Vector3.up);
            float stageYaw = Vector3.Angle(anchors[0].transform.position-anchors[1].transform.position, Vector3.right);

            Vector3 stageRot = new Vector3(
                0, 
                // ***angleがおかしい
                // Vector3.Angle(anchorsPos[0], anchorsPos[1]),
                stageYaw, 
                // Vector3.Angle(anchors[0].transform.localPosition, anchors[1].transform.localPosition),
                // Vector3.Angle(anchorsPos[1] - center01, anchorsPos[0] - center01),  
                0
                
                
            );
            
            Debug.Log(Vector3.Angle(anchorsPos[0], anchorsPos[1]));
            Debug.Log(stageYaw);
            
            
            stage.transform.localRotation = Quaternion.Euler(stageRot);
            
            
            
        }

        if (Input.GetKeyDown(KeyCode.T)) {
            Debug.Log(Vector3.Angle(anchors[0].transform.position-anchors[1].transform.position, Vector3.right));
            
        }
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.tag == "IndexTip") {
            anchorsPos[anchorCount] = target.transform.position;

            GameObject anchor = Instantiate(anchorPrefab);
            anchor.transform.position = anchorsPos[anchorCount];
            anchor.transform.parent = anchorsParent.transform;

            anchors[anchorCount] = anchor;
            
            Debug.Log(anchorsPos[anchorCount]);
            anchorCount++;
        }
        
        
    }

    void CreateStage() {
        float width = Vector3.Distance(anchorsPos[0], anchorsPos[2]);
        float height = Vector3.Distance(anchorsPos[0], anchorsPos[1]);
        Vector3 vec = ( anchorsPos[2] - anchorsPos[0]).normalized;

        Vector3 ab = anchorsPos[1]-anchorsPos[0];
        float dist = Vector3.Distance(anchorsPos[1], anchorsPos[0]);
        Vector3 ac = anchorsPos[2]-anchorsPos[0];
        float dot = Vector3.Dot(ab, ac);
        float tmp = dot/Mathf.Pow(dist, 2);
        Vector3 adash = anchorsPos[2] - tmp*ab;

                
        GameObject block = Instantiate(anchorPrefab);
        block.transform.localScale = new Vector3(width/3, 0.1f, height/3);
        block.transform.position = adash;
        block.transform.rotation = transform.rotation;
    }
}

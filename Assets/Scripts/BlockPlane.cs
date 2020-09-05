using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPlane : MonoBehaviour
{
    
    private Vector3[] anchorsPos = new Vector3[5];
    private int anchorCount;
    [SerializeField] private GameObject anchorPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.tag == "IndexTip") {
            anchorsPos[anchorCount] = target.transform.position;
            GameObject anchor = Instantiate(anchorPrefab);
            anchor.transform.position = anchorsPos[anchorCount];
            Debug.Log(anchorsPos[anchorCount]);
            anchorCount++;
        }
        
        
    }
}

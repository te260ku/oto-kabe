﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGnerator : MonoBehaviour
{

    private Vector3[] vertexes = new Vector3[5];
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject[] anchors = new GameObject[3];
    [SerializeField] private GameObject middle1Sphere;
    [SerializeField] private GameObject blockPlanePrefab;
    [SerializeField] private GameObject stagePrefab;
    void Start()
    {

        Debug.Log(Vector3.SignedAngle(anchors[0].transform.position, anchors[1].transform.position, Vector3.up));

        vertexes[0] = anchors[0].transform.position;
        vertexes[1] = anchors[1].transform.position;
        vertexes[2] = anchors[2].transform.position;
        
        
        
        float width = Vector3.Distance(vertexes[0], vertexes[2]);
        float height = Vector3.Distance(vertexes[0], vertexes[1]);
        
        Vector3 mid = new Vector3 ((vertexes[2].x + vertexes[0].x) / 2, 
        (vertexes[2].y + vertexes[0].y) / 2, 
        (vertexes[2].z + vertexes[0].z) / 2);
        Vector3 vec = ( anchors[2].transform.position - anchors[0].transform.position ).normalized;

        Vector3 ab = anchors[1].transform.position-anchors[0].transform.position;
        float dist = Vector3.Distance(anchors[1].transform.position, anchors[0].transform.position);
        Vector3 ac = anchors[2].transform.position-anchors[0].transform.position;
        float dot = Vector3.Dot(ab, ac);
        float tmp = dot/Mathf.Pow(dist, 2);
        Vector3 adash = anchors[2].transform.position - tmp*ab;

        


        GameObject blockPlane = Instantiate(blockPlanePrefab);
        blockPlane.transform.position = middle1Sphere.transform.position;
        blockPlane.transform.rotation = middle1Sphere.transform.rotation;

        

        // GameObject block = Instantiate(blockPrefab);
        // block.transform.localScale = new Vector3(width/3, 0.1f, height/3);
        // block.transform.position = adash;
        // block.transform.rotation = Quaternion.FromToRotation(Vector3.up, vec);

        
        


        width = vertexes[1].x-vertexes[0].x;
        height = vertexes[1].z-vertexes[0].z;

        // // 3*3のマトリックスを生成する
        // for (int i=0; i<3; i++) {
        //     for (int j=0; j<3; j++) {
        //         GameObject block = Instantiate(blockPrefab).gameObject;
        //         block.transform.localScale = new Vector3(width/3, 0.1f, height/3);
        //         block.transform.position = new Vector3(vertexes[0].x+width*1/6+(width/3*i), 0.2f, vertexes[0].z+height*1/6+(height/3*j));
        //     }
        // }
    
            
        

        
    }

    
    void Update()
    {
        
    }
}

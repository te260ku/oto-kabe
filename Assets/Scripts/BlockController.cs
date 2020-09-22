using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    // private GameObject[,] blocks = new GameObject[3,3];
    private Block[] blocks = new Block[9];
    void Start()
    {
        GameObject[] blockObject = new GameObject[9];
        int c1 = 0;
        foreach (Transform child in this.transform){
            blockObject[c1] = child.gameObject;
            c1++;
        }
        foreach (GameObject child in blockObject){
            blocks[c1] = child.gameObject.GetComponent<Block>();
            c1++;
        }

        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            SetNextBlock();
        }   
    }

    private void SetNextBlock() {
        int nextBlockNum = Random.Range(0, 9);
        Block target = blocks[nextBlockNum];
        
        if (target.tag == "Target") {
            target.GetComponent<Renderer>().material.color = Color.blue;
            // audioSource.PlayOneShot(hitBlockAudio);
        }

    }
}

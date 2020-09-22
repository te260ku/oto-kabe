using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    // private GameObject[,] blocks = new GameObject[3,3];
    private Block[] blocks = new Block[9];
    void Start()
    {
        GameObject[] blockObjects = new GameObject[9];
        int count = 0;
        foreach (Transform child in this.transform){
            blocks[count] = child.gameObject.GetComponent<Block>();
            // blocks[count] = blockObjects[count].GetComponent<Block>();
            
            count++;
        }

        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            SetNextBlock();
        }  
        
      
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray();
            RaycastHit hit = new RaycastHit();
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //マウスクリックした場所からRayを飛ばし、オブジェクトがあればtrue 
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
            {
                if(hit.collider.gameObject.CompareTag("Target"))
                {
                    hit.collider.gameObject.GetComponent<Renderer>().material.color = Color.white;
                }
            }
        }
    }

    private void SetNextBlock() {
        int nextBlockNum = Random.Range(0, 9);
        Block target = blocks[nextBlockNum];
        
        
            target.SetColor();
            // audioSource.PlayOneShot(hitBlockAudio);
        

    }
}

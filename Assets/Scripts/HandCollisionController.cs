using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollisionController : MonoBehaviour
{
    [SerializeField] BlockController blockController;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {   
        GameObject target = other.gameObject;
        if (target.tag == "Block") {
            OnHitBlock(other.gameObject);
        }
        
    }

    public void OnHitBlock(GameObject target) 
    {
        var hitBlock = target.GetComponent<Block>();
        blockController.OnHit(hitBlock.BlockID);
    }
}

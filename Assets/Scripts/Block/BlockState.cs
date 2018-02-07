using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockState : MonoBehaviour {
    public BlockType blockType{get; private set;}

    //初期化
    void Awake() {
        blockType = BlockType.NONE;
    }

    public void DecideBlock(BlockType type) {
        blockType = type;
        Debug.Log("block:"+transform.position+" type:"+blockType);
        GetComponent<BlockProvider>().Move(transform.position + Vector3.up, 0.4f);
    }
}

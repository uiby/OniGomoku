using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockState : MonoBehaviour {
    public Owner owner{get; private set;}
    public Vector2 address{get; private set;}

    //初期化
    void Awake() {
        owner = Owner.NONE;
    }

    public void Initialize(Vector2 _address) {
        address = _address;
    }

    public void DecideOwner(Owner type) {
        owner = type;
        Debug.Log("block:"+transform.position+" owner:"+owner);
        GetComponent<BlockProvider>().Move(transform.position + Vector3.up, 0.4f);
    }
}

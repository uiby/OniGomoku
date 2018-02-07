using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCollision : MonoBehaviour {
    static int layerMask;

    void Awake() {
        layerMask = LayerMask.GetMask("Block");        
    }

    public static bool HasBlock(Vector3 center, Vector3 scale) {
        return Physics.CheckBox(center, scale/2f, Quaternion.identity, layerMask);
    }

    public static Transform GetBlock(Vector3 center) {
        return Physics.OverlapBox(center, new Vector3(0.5f, 2, 0.5f), Quaternion.identity, layerMask)[0].transform;
    }
}

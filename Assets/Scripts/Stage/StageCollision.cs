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
        return Physics.OverlapBox(center, new Vector3(0.1f, 2, 0.1f), Quaternion.identity, layerMask)[0].transform;
    }

    public static Transform GetBlockByRay(Vector3 basePos, Vector3 direction) {
        RaycastHit hit;
        if (Physics.Raycast(basePos, direction, out hit, Mathf.Infinity, layerMask)){
            return hit.transform;
        }  
        return null;
    }

    public static Transform GetBlockByRayFormCamera(Vector3 mousePos) {
        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)){
            return hit.transform;
        }  
        return null;
    }

    public static bool CanStraight(Vector3 basePos, Vector3 idealPos) {
    var direction = (idealPos - basePos).normalized;
    var distance = (idealPos - basePos).magnitude;
    RaycastHit hit;

    //Debug.Log("distance:"+distance);
    var result = Physics.Raycast (basePos, direction, out hit, distance, layerMask);
    return result;
  }

}

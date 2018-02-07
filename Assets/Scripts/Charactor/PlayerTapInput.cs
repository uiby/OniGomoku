using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTapInput : MonoBehaviour {
    PlayerProvider playerProvider;

    void Awake() {
        playerProvider = GetComponent<PlayerProvider>();
    }

	// Update is called once per frame
	void Update () {
		if (!DecideDestination())
            return;

        playerProvider.StartMove(GetTapPos());
	}

    bool DecideDestination() {
        return Input.GetMouseButtonDown(0);
    }

    Vector3 GetTapPos() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        var layerMask = LayerMask.GetMask("Block");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
            var pos = hit.point;
            //Debug.Log("tap pos:"+pos);
            return pos;
        }

        //Debug.Log("no block");
        return Vector3.zero;
    }
}

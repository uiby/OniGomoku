using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTapInput : CharacterTapInput {
	// Update is called once per frame
	void Update () {
		if (DecideDestination())
            playerProvider.StartMove(GetWorldPosByTap());

        if (DecidePutting()) {
            var block = StageCollision.GetBlockByRay(transform.position + Vector3.up, Vector3.down); //現在の場所
            //Debug.Log("block pos:"+block.position);
            if (block == null) return;
            if (block.GetComponent<BlockProvider>().Decided())
                return;
            playerProvider.DecideBlock(block.GetComponent<BlockProvider>());
            playerProvider.StopMove();
        }
	}

    bool DecideDestination() {
        return Input.GetMouseButtonDown(0);
    }

    Vector3 GetWorldPosByTap() {
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

    bool DecidePutting() {
        return Input.GetMouseButtonDown(1);
    }


}

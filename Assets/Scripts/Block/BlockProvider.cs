using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockProvider : MonoBehaviour {
    BlockMover blockMover;

    void Awake() {
        blockMover = GetComponent<BlockMover>();
    }

    public void Move(Vector3 idealPos, float duration) {
        blockMover.Move(idealPos, duration);
    }
    public void Move(Vector3 startPos, Vector3 idealPos, float duration) {
        transform.position = startPos;
        blockMover.Move(idealPos, duration);
    }
}

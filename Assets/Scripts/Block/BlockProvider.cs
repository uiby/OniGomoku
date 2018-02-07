using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockProvider : MonoBehaviour {
    BlockMover blockMover;
    BlockState blockState;

    void Awake() {
        blockMover = GetComponent<BlockMover>();
        blockState = GetComponent<BlockState>();
    }

    public void Move(Vector3 idealPos, float duration) {
        blockMover.Move(idealPos, duration);
    }
    public void Move(Vector3 startPos, Vector3 idealPos, float duration) {
        transform.position = startPos;
        blockMover.Move(idealPos, duration);
    }

    public void DecideOwner(Owner type) {
        blockState.DecideOwner(type);
    }

    public Vector2 GetAddress() {
        return blockState.address;
    }

    public bool Decided() {
        return blockState.owner != Owner.NONE;
    }

    public void Initialize(Vector2 address) {
        blockState.Initialize(address);
    }
}

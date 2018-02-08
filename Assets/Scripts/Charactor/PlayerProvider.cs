using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerProvider : MonoBehaviour {
    [SerializeField] Owner onwer;
    PlayerMover playerMover;
    CharacterTapInput charaTapInput;
    [Inject] BlockManager blockManager;
    Rigidbody rigidbody;

	// Use this for initialization
	void Awake () {
		playerMover = GetComponent<PlayerMover>();
        charaTapInput = GetComponent<CharacterTapInput>();
        rigidbody = GetComponent<Rigidbody>();
	}

    public void DecideBlock(BlockProvider blockProvider) {
        blockManager.DecideOwner(blockProvider, onwer);
    }

    public void StartMove(Vector3 tapPos) {
        playerMover.Move(tapPos);
    }
    public void StopMove() {
        playerMover.Stop();
    }

    public void BeginControl() {
        rigidbody.useGravity = true;
        charaTapInput.canOperation = true;
    }
}

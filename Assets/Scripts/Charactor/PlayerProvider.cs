using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerProvider : MonoBehaviour {
    [SerializeField] Owner onwer;
    PlayerMover playerMover;
    PlayerTapInput playerTapInput;
    [Inject] BlockManager blockManager;

	// Use this for initialization
	void Awake () {
		playerMover = GetComponent<PlayerMover>();
        playerTapInput = GetComponent<PlayerTapInput>();
	}

    public void DecideBlock(BlockProvider blockProvider) {
        blockManager.DecideOwner(blockProvider, onwer);
    }

    public void StartMove(Vector3 tapPos) {
        playerMover.Move(tapPos);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProvider : MonoBehaviour {
    PlayerMover playerMover;
    PlayerTapInput playerTapInput;

	// Use this for initialization
	void Awake () {
		playerMover = GetComponent<PlayerMover>();
        playerTapInput = GetComponent<PlayerTapInput>();
	}

    public void StartMove(Vector3 tapPos) {
        playerMover.Move(tapPos);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTapInput : MonoBehaviour {
    public PlayerProvider playerProvider{get; private set;}
    public bool canOperation;

    void Awake() {
        playerProvider = GetComponent<PlayerProvider>();
        canOperation = false;
    }

}

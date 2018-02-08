using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerTapInput : CharacterTapInput {
    [SerializeField] Transform oppnent;

	// Use this for initialization
	void Start () {
        StartCoroutine(DecideDestination());		
	}

    IEnumerator DecideDestination() {
        while(true) {
            if (canOperation)
                playerProvider.StartMove(oppnent.position);
            yield return new WaitForSeconds(0.1f);
        }
    }
}

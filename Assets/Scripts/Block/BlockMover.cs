using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMover : MonoBehaviour {
    [SerializeField] AnimationCurve moveCurve;
    public void Move(Vector3 idealPos, float duration) {
        StartCoroutine(Movement(idealPos, duration));
    }

    public IEnumerator Movement(Vector3 endPos, float duration) {
        var startPos = transform.position;
        var timer = 0f;
        var rate = 0f;

        while (rate < 1) {
            timer += Time.deltaTime;
            rate = Mathf.Clamp01(timer/duration);
            var curveRate = moveCurve.Evaluate(rate);

            transform.position = Vector3.Lerp(startPos, endPos, curveRate);
            yield return null;
        }
    }
}

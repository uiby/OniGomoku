using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerMover : MonoBehaviour {
    [SerializeField, Range(2, 10)] int runSpeed = 5;
    List<Vector2> checkPointList = new List<Vector2>();
    int checkCount = 0;
    bool run = false;
    Rigidbody rigidbody;
    [Inject]
    AStar aStar;

    void Awake() {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start() {
        aStar.Initialize(10);
    }

    void FixedUpdate() {
        if (!run) return;

        if (checkCount < checkPointList.Count) {
            transform.LookAt(Vector3.up * transform.position.y + new Vector3(checkPointList[checkCount].x, 0, checkPointList[checkCount].y));
            Run();
            ArriveCheckPoint();
        }
    }

    public void Move(Vector3 destination) {
        Reset(); //初期化
        checkPointList = aStar.MakePath(new Vector2(transform.position.x, transform.position.z), new Vector2(destination.x, destination.z));

        run = true;
    }

    void Reset() {
        checkPointList.Clear();
        checkCount = 0;
    }

    void Run() {
        /*if (animationMode != 2) {
            animationMode = 2;
            animator.SetBool("Run", true);
        }*/
        var amount = transform.forward * runSpeed * Time.deltaTime;
        rigidbody.MovePosition(rigidbody.position + amount);
    }

    void ArriveCheckPoint() {
        var nowPos = new Vector2(transform.position.x, transform.position.z);
        var checkPos = checkPointList[checkCount];

        if ((checkPos - nowPos).magnitude > 0.2f) return;
        checkCount++;
    }
}

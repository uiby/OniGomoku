using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] StageCreater stageCreater;
    [SerializeField] List<PlayerProvider> charaList;

    void Start() {
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop() {
        yield return StartCoroutine(stageCreater.CreateStage());
        yield return new WaitForSeconds(1f);

        charaList.ForEach(chara => chara.BeginControl());
    }
}

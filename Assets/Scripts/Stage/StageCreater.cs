using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StageCreater : MonoBehaviour {
    [SerializeField] GameObject blockPrefab;
    [SerializeField, Range(5, 10)] int range = 9;
    [Inject] BlockManager blockManager;


	// Use this for initialization
	void Awake () {
        blockManager.Initialize(range);
	}
	
    public IEnumerator CreateStage() {
        float popDuration = 0.4f;
        /*for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                var block = (GameObject)Instantiate(blockPrefab, Vector3.zero, Quaternion.identity);
                block.GetComponent<BlockProvider>().Move(new Vector3(x, -10, y), new Vector3(x, 0, y), popDuration);
                yield return new WaitForSeconds(popDuration/width);
            }
        }*/

        int counter = 0;
        var diff = (range - 1)/2f;
        var opPos = (range - 1);
        while (counter < range) {
            var y = counter;
            for (int x = 0; x <= counter; x++) {
                var block = (GameObject)Instantiate(blockPrefab, Vector3.zero, Quaternion.identity);
                block.GetComponent<BlockProvider>().Move(new Vector3(x - diff, -10, y - diff), new Vector3(x - diff, -1, y - diff), popDuration);
                block.GetComponent<BlockProvider>().Initialize(new Vector2(x, y));
                block.transform.SetParent(transform);
                blockManager.SetOwner(x, y, Owner.NONE);
                if (x == opPos - y && y == opPos - x) {
                    y--;
                    continue;
                }
                var block2 = (GameObject)Instantiate(blockPrefab, Vector3.zero, Quaternion.identity);
                block2.GetComponent<BlockProvider>().Move(new Vector3(opPos - x - diff, -10, opPos - y - diff), new Vector3(opPos - x - diff, -1, opPos - y - diff), popDuration);
                block2.GetComponent<BlockProvider>().Initialize(new Vector2(opPos - x, opPos - y));
                block2.transform.SetParent(transform);
                blockManager.SetOwner(opPos - x, opPos - y, Owner.NONE);
                y--;
            }

            counter++;
            yield return new WaitForSeconds(popDuration/range);
        }
    }
}

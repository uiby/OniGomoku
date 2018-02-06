using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCreater : MonoBehaviour {
    [SerializeField] GameObject blockPrefab;
    [SerializeField, Range(5, 10)] int range = 10;

	// Use this for initialization
	void Start () {
        StartCoroutine(CreateStage());		
	}
	
	// Update is called once per frame
	void Update () {
		
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
        while (counter < range) {
            var y = counter;
            for (int x = 0; x <= counter; x++) {
                var block = (GameObject)Instantiate(blockPrefab, Vector3.zero, Quaternion.identity);
                block.GetComponent<BlockProvider>().Move(new Vector3(x, -10, y), new Vector3(x, -1, y), popDuration);
                block.transform.SetParent(transform);
                if (x == (range - 1) - y && y == (range - 1) - x) {
                    y--;
                    continue;
                }
                var block2 = (GameObject)Instantiate(blockPrefab, Vector3.zero, Quaternion.identity);
                block2.GetComponent<BlockProvider>().Move(new Vector3((range - 1) - x, -10, (range - 1) - y), new Vector3((range - 1) - x, -1, (range - 1) - y), popDuration);
                block2.transform.SetParent(transform);
                y--;
            }

            counter++;
            yield return new WaitForSeconds(popDuration/range);
        }
    }
}

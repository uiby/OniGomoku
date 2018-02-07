using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour {
    Owner[,] blocks;

    public void Initialize(int range) {
        blocks = new Owner[range, range];
        for (int y = 0; y < range; y++) {
            for (int x = 0; x < range; x++) {
                blocks[y, x] = Owner.NONE;
            }
        }
    }

    public void SetOwner(int x, int y, Owner type) {
        blocks[y, x] = type;
    }

    public void DecideOwner(BlockProvider blockProvider, Owner owner) {
        blockProvider.DecideOwner(owner);
        var address = blockProvider.GetAddress();
        blocks[(int)address.y, (int)address.x] = owner;
    }
}

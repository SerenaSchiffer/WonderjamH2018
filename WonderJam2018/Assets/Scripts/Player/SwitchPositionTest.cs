using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPositionTest : MonoBehaviour {

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
            {
                go.GetComponent<PlayerController>().SwapPositions();
            }
        }
    }

}

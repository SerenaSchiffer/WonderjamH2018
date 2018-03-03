using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targets : MonoBehaviour {

    bool occupied;

	// Use this for initialization
	void Start () {
        occupied = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetOccupied()
    {
        occupied = true;
    }

    public void SetNotOccupied()
    {
        occupied = false;
    }

    public bool IsOccupied()
    {
        return occupied;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override PickableItem InteractWithPlayer(PickableItem item)
    {
        return null; //Delete that shit
    }

}

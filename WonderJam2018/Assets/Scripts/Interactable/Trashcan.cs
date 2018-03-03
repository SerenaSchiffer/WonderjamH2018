using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashcan : Interactable {

	// Use this for initialization
	void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
		
	}

    public override PickableItem InteractWithPlayer(PickableItem playerItem)
    {
        if(playerItem as Melange != null)
        {
            return null;
        }
        else
        {
            return playerItem;
        }
    }

    public override void Highlight(PickableItem playerItem)
    {
        if (playerItem as Melange != null)
        {
            base.Highlight(playerItem);
        }
    }
}

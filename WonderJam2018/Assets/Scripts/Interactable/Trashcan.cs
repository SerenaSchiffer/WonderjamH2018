using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashcan : Interactable {

    Animator myanimator;

	// Use this for initialization
	public override void Start () {
        base.Start();
        myanimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
		
	}

    public override PickableItem InteractWithPlayer(PickableItem playerItem)
    {
        if(playerItem as Melange != null)
        {
            myanimator.SetTrigger("Trashing");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Interactable {

    [SerializeField ]Ingredient myItem;
    SpriteRenderer itemRenderer;

	// Use this for initialization
	public override void Start () {
        base.Start();
        //myItem = null;
        itemRenderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
		if(myItem != null)
        {
            itemRenderer.sprite = myItem.mySprite;
        }
        else
        {
            itemRenderer.sprite = null;
        }
	}

    public override Ingredient InteractWithPlayer(Ingredient playerItem)
    {
        if(playerItem != null)
        {
            if(myItem == null)
            {
                myItem = playerItem;
                return null;
            }
            else
            {
                Ingredient temp = null;
                temp = myItem;
                myItem = playerItem;
                return temp;
            }
        }
        else
        {
            if(myItem != null)
            {
                Ingredient temp = myItem;
                myItem = null;
                return temp;
            }
            else
            {
                return null;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Interactable {

    [SerializeField ]Ingredient myItem;
    SpriteRenderer itemRenderer;

	// Use this for initialization
	void Start () {
        myItem = null;
        itemRenderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if(myItem != null)
        {
            itemRenderer.sprite = myItem.ingredientSprite;
        }
	}

    public override void InteractWithPlayer(Ingredient playerItem)
    {
        if(playerItem != null)
        {
            if(myItem == null)
            {

            }
            else
            {

            }
        }
        else
        {
            if(myItem != null)
            {

            }
        }
    }
}

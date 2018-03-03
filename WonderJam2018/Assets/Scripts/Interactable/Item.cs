using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable {

    Ingredient myItem;
	// Use this for initialization
	void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}

    void AutoDestroy()
    {
        Destroy(gameObject);
    }

    public void SetItem(Ingredient newIngredient)
    {
        myItem = newIngredient;
    }

    void ResetSprite(Ingredient item)
    {
        if (item == null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = null;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = item.mySprite;
        }
        
    }
    

    public override PickableItem InteractWithPlayer(PickableItem playerItem)
    {
        if (playerItem as Ingredient == null)
        {
            Ingredient temp = myItem;
            ResetSprite(null);
            Invoke("AutoDestroy", 0.05f);
            return temp;



        }

        if(playerItem as Ingredient != null)
        {
            Ingredient tempVar = myItem;
            myItem = (Ingredient)playerItem;
            ResetSprite(myItem);
            return tempVar;
            
        }else
        {
            return myItem;
        }
    }

    public override void Highlight(PickableItem playerItem)
    {
        if (playerItem as Melange == null)
        {
            base.Highlight(playerItem);
        }
    }

}

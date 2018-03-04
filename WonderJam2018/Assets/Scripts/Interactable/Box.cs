using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Interactable {

    public Ingredient myItem;
    SpriteRenderer itemRenderer;
    SpriteRenderer parentRenderer;

    [SerializeField] AudioClip audio_Swap;
    AudioManager audioMixer;

	// Use this for initialization
	public override void Start () {
        base.Start();
        //myItem = null;
        itemRenderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        parentRenderer = transform.parent.gameObject.GetComponent<SpriteRenderer>();
        audioMixer = GameObject.Find("AudioMixer").GetComponent<AudioManager>();
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

    public override PickableItem InteractWithPlayer(PickableItem playerItem)
    {
        if (playerItem as Ingredient != null)
        {
            if (playerItem != null)
            {
                if (myItem == null)
                {
                    audioMixer.PlaySfx(audio_Swap, 0);
                    myItem = (Ingredient)playerItem;
                    return null;
                }
                else
                {
                    audioMixer.PlaySfx(audio_Swap, 0);
                    Ingredient temp = null;
                    temp = myItem;
                    myItem = (Ingredient)playerItem;
                    return temp;
                }
            }
            else
            {                
                    return null;
            }

        }
        else if (playerItem as Melange != null)
        {
            return playerItem;
        }
        else
        {
            
            if (myItem != null)
            {
            Ingredient temp = myItem;
            myItem = null;
            return temp;
            }
            return null;
        }
    }

    public override void Highlight(PickableItem playerItem)
    {
        if(playerItem as Melange == null)
        {
            base.Highlight(playerItem);
        }
    }

    public int GetHighlight()
    {

        return highlightBuffer;
    }
}

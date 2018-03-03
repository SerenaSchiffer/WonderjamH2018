using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CounterSide
{
    Left,
    Right
};

public class ServiceCounter : Interactable {

    public CounterSide side;
    bool isGoodMelange = false;
    
    Queue<GameObject> clientsEnFile;

    [HideInInspector] public Melange potion;

    public void Awake()
    {
        clientsEnFile = new Queue<GameObject>();
    }

	// Use this for initialization
	public override void Start () {
        base.Start();       
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
	}

    public void AddClientToQueue(GameObject newClient)
    {
        clientsEnFile.Enqueue(newClient);
    }

    public void PopClientFromQueue()
    {
        clientsEnFile.Dequeue();
    }

    public bool IsQueueFull()
    {
        if (clientsEnFile.Count == 2)
            return true;
        else
            return false;
    }

    public int GetQueueCount()
    {
        return clientsEnFile.Count;
    }
    
    public bool DoesQueueContain(GameObject client)
    {
        if (clientsEnFile.Contains(client))
            return true;
        else
            return false;
    }

    public override PickableItem InteractWithPlayer(PickableItem playerItem)
    {
        if(playerItem as Melange != null && potion != null)
        {
            isGoodMelange = VerifiIfGoodPotion((Melange)playerItem);
            return null;
        }
        else
        {
            return playerItem;
        }
    }

    private bool VerifiIfGoodPotion(Melange playerMelange)
    {
        if(playerMelange.mesIngredients.Count != potion.mesIngredients.Count)
        {
            return false;
        }
        else
        {
            for(int i = 0; i < playerMelange.mesIngredients.Count; i++)
            {
                Ingredient playerIng = playerMelange.mesIngredients.Dequeue();
                Ingredient clientIng = potion.mesIngredients.Dequeue();
                if(playerIng.ingredientName != clientIng.ingredientName)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public bool InteractWithClient(Melange clientMelange)
    {
        if(potion == null)
            potion = clientMelange;
        if(isGoodMelange)
        {
            isGoodMelange = false;
            return true;
        }
        else
        {
            return false;
        }
            
    }

    public override void Highlight(PickableItem playerItem)
    {
        if(playerItem as Melange != null && potion != null)
            base.Highlight(playerItem);
    }
}

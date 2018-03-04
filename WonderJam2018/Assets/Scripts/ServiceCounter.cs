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
    ClientStatePotion potionState = ClientStatePotion.Waiting;
    StateClient clientState = StateClient.Joy;
    
    Queue<GameObject> clientsEnFile;

    [HideInInspector] public Melange potion;
    UIManager uiManager;
    Animator myAnimator;

    public void Awake()
    {
        clientsEnFile = new Queue<GameObject>();
    }

	// Use this for initialization
	public override void Start () {
        base.Start();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        myAnimator = GetComponent<Animator>();      
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
            if (potion == null)
                return playerItem;
            isGoodMelange = VerifiIfGoodPotion((Melange)playerItem);
            if(isGoodMelange)
            {
                potionState = ClientStatePotion.Good;
            }
            else
            {
                potionState = ClientStatePotion.Bad;
            }
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
            CalculateScore(playerMelange.player);
            myAnimator.SetTrigger("Pay");
            return true;
        }
    }

    void CalculateScore(int player)
    {
        float score = potion.MelangeValue();
        switch (clientState)
        {
            case StateClient.Joy:               
                score += 10f;
                break;
            case StateClient.Annoyed:
                score += 2f;
                break;
            default:
                break;
        }
        uiManager.UpdateScore(player, score);
    }

    public ClientStatePotion InteractWithClient(Melange clientMelange, StateClient clientState)
    {
        this.clientState = clientState;
        if(potion == null)
            potion = clientMelange;
        if(potionState != ClientStatePotion.Waiting)
        {
            potion = null;
            return potionState;
        }
        else
        {
            return ClientStatePotion.Waiting;
        }
            
    }

    public override void Highlight(PickableItem playerItem)
    {
        if(playerItem as Melange != null && potion != null)
            base.Highlight(playerItem);
    }
}

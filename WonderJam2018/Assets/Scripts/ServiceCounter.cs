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
    PotionState potionState = PotionState.Waiting;
    StateClient clientState = StateClient.Joy;
    
    Queue<GameObject> clientsEnFile;

    [HideInInspector] public Melange potion;
    UIManager uiManager;
    Animator myAnimator;

    [SerializeField] AudioClip audio_moneySound;
    [SerializeField] AudioClip audio_wrongRecipe;
    private AudioManager audioMixer;

    public void Awake()
    {
        clientsEnFile = new Queue<GameObject>();
    }

	// Use this for initialization
	public override void Start () {
        base.Start();
        audioMixer = GameObject.Find("AudioMixer").GetComponent<AudioManager>();
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
        if(clientsEnFile.Count > 0)
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
            GameObject firstClient = clientsEnFile.Peek();
            Animator animator = firstClient.GetComponent<Animator>();

            isGoodMelange = VerifiIfGoodPotion((Melange)playerItem);
            if (isGoodMelange)
            {
                //potionState = PotionState.Good;
                //TODO donner plus d'argent si content
                audioMixer.PlaySfx(audio_moneySound, 0);

                if (!animator.GetBool("Angry"))
                {
                    animator.SetBool("Joy", true);
                    animator.SetBool("Annoyed", false);
                    animator.SetBool("Angry", false);
                    animator.SetBool("Idle", false);
                }

                firstClient.GetComponent<Client>().ExitShop();
            }
            else
            {
                audioMixer.PlaySfx(audio_wrongRecipe, 0);
                
                if (!animator.GetBool("Angry"))
                {
                    animator.SetBool("Angry", true);
                    animator.SetBool("Joy", false);
                    animator.SetBool("Annoyed", false);
                    animator.SetBool("Idle", false);
                }

                firstClient.GetComponent<Client>().ExitShop();
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
                else
                {
                    playerMelange.mesIngredients.Enqueue(playerIng);
                    potion.mesIngredients.Enqueue(clientIng);
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
                score += 5f;
                break;

            case StateClient.Annoyed:
                score += 2f;
                break;

            default:
                break;
        }
        uiManager.UpdateScore(player, score);
    }

    public PotionState InteractWithClient(Melange clientMelange, StateClient clientState)
    {
        this.clientState = clientState;
        if(potion == null)
            potion = clientMelange;
        if(potionState != PotionState.Waiting)
        {
            potion = null;
            PotionState temp = potionState;
            potionState = PotionState.Waiting;
            return temp;
        }
        else
        {
            return PotionState.Waiting;
        }
            
    }

    public override void Highlight(PickableItem playerItem)
    {
        if(playerItem as Melange != null && potion != null)
            base.Highlight(playerItem);
    }
}

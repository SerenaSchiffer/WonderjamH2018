using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PotionState
{
    Good,
    Bad,
    Waiting,
    Finished,
}

public enum StateClient
{
    Joy,
    Annoyed,
    Angry,
    RageQuit,
}

public class Client : MonoBehaviour {

    public Melange melangeRef;
    public float maxTimer;
    
    [HideInInspector] public Melange melangeClient;
    [HideInInspector] public ServiceCounter myCounter;

    [SerializeField] RuntimeAnimatorController[] animController;

    Rigidbody2D rb;
    InstantiableObjectContainer objectsReferences;
    Animator animator;
    float timer;
    bool hasArrived;
    PotionState melangeState = PotionState.Waiting;
    StateClient clientState = StateClient.Joy;
    GameObject melangeClientPopup;
    
    [SerializeField] AudioClip audio_annoyed;
    [SerializeField] AudioClip audio_angry;

    [SerializeField] AudioClip audio_Quitannoyed;
    [SerializeField] AudioClip audio_Quitangry;
    [SerializeField] AudioClip audio_QuitEnjoyed;
    [SerializeField] AudioClip audio_RageQuit;

    [SerializeField] AudioClip audio_Mix;

    private AudioManager audioMixer;
    bool hasTerminated = false;


    // Use this for initialization
    void Start () {
        audioMixer = GameObject.Find("AudioMixer").GetComponent<AudioManager>();
        timer = maxTimer;
        hasArrived = false;
        rb = GetComponent<Rigidbody2D>();
        objectsReferences = GetComponent<InstantiableObjectContainer>();
        animator = GetComponent<Animator>();

        switch (UnityEngine.Random.Range(1, 4))
        {
            case 1:
                animator.runtimeAnimatorController = animController[0];
                break;

            case 2:
                animator.runtimeAnimatorController = animController[1];
                break;

            case 3:
                animator.runtimeAnimatorController = animController[2];
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!hasTerminated)
        {
            if (timer >= (maxTimer / 3) * 2)
            {
                ChangeClientState(StateClient.Joy);
            }
            else if (timer >= (maxTimer / 3) && timer < (maxTimer / 3) * 2)
            {
                ChangeClientState(StateClient.Annoyed);
            }
            else if (timer >= 0 && timer < (maxTimer / 3))
            {
                ChangeClientState(StateClient.Angry);
            }
            else
            {
                clientState = StateClient.RageQuit;
                ExitShop();
            }
        }

        if (hasArrived)
            timer -= Time.deltaTime;
        if (hasTerminated)
        {
            if (myCounter.side == CounterSide.Left)
                rb.velocity = new Vector2(-1f, 0f);
            else
                rb.velocity = new Vector2(1f, 0f);
        }
        else
        {
            if (melangeState == PotionState.Waiting)
            {
                if (!IsInFrontOfSomething())
                {
                    rb.velocity = new Vector2(0, -1);
                }
                else
                {
                    hasArrived = true;
                    if (melangeClientPopup)
                        melangeClientPopup.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0.5f, 0.56f, 0));
                }
            }
        }
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    private void ChangeClientState(StateClient clientState)
    {
        if (this.clientState != clientState)
        {
            switch (clientState)
            {
                case StateClient.Joy:
                    animator.SetBool("Idle", true);
                    break;

                case StateClient.Annoyed:
                    audioMixer.PlaySfx(audio_annoyed, 0);
                    animator.SetBool("Annoyed", true);
                    animator.SetBool("Idle", false);
                    break;

                case StateClient.Angry:
                    audioMixer.PlaySfx(audio_angry, 0);
                    animator.SetBool("Angry", true);
                    animator.SetBool("Annoyed", false);
                    break;
            }

            this.clientState = clientState;
        }
    }

    public void ExitShop()
    {
        hasTerminated = true;
        PlayQuitSound(clientState);
        myCounter.PopClientFromQueue();
        Destroy(melangeClientPopup);
        Invoke("AutoDestroy", 3f);
    }

    private void PlayQuitSound(StateClient stateClient)
    {
        switch (stateClient)
        {
            case StateClient.Joy:
            case StateClient.Annoyed:
                audioMixer.PlaySfx(audio_QuitEnjoyed, 0);
                break;

            case StateClient.Angry:
                audioMixer.PlaySfx(audio_Quitangry, 0);
                break;

            case StateClient.RageQuit:
                audioMixer.PlaySfx(audio_RageQuit, 0);
                break;
        }
    }

    private bool IsInFrontOfSomething()
    {
        if (!hasTerminated)
        {
            int layerMask = LayerMask.GetMask(new String[] { "Interactable", "ObjectToStopClient" });
            RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0f, 0.15f), new Vector3(0f, -1f), 0.9f, layerMask);

            //GameObject other = hit.collider.gameObject;
            if (hit.collider != null)
            {
                rb.velocity = Vector2.zero;
                GenerateRecipe();


                if (hit.collider.gameObject.layer == 8)
                {
                    myCounter = hit.collider.gameObject.GetComponent<ServiceCounter>();
                    melangeState = myCounter.InteractWithClient(melangeClient, clientState);
                }
                else if (hit.collider.gameObject.layer == 10)
                    myCounter = hit.collider.gameObject.GetComponent<Client>().myCounter;

                if (!myCounter.DoesQueueContain(gameObject))
                    myCounter.AddClientToQueue(gameObject);

                return true;
            }

            return false;
        }
        else
            return false;
    }

    private void GenerateRecipe()
    {
        if (!melangeClient)
        {
            melangeClient = Instantiate<Melange>(melangeRef);
            melangeClient.GenerateRandomRecipe();


            melangeClientPopup = Instantiate(objectsReferences.MelangeClientPopup);
            melangeClientPopup.transform.SetParent(GameObject.Find("Canvas").transform, false);

            AddIngredientToPopup(melangeClientPopup);

            Vector2 spawnPosition = new Vector2(transform.position.x + 0.5f, transform.position.y + 0.56f);
            melangeClientPopup.transform.position = Camera.main.WorldToScreenPoint(spawnPosition);
        }
    }

    private void AddIngredientToPopup(GameObject melangeClientPopup)
    {
        foreach(Ingredient ingredient in melangeClient.mesIngredients)
        {
            GameObject ingredientImage = Instantiate(objectsReferences.ImageIngredientUI);
            ingredientImage.transform.SetParent(melangeClientPopup.transform, false);
            ingredientImage.GetComponent<Image>().sprite = ingredient.mySprite;
            //ingredientImage.transform.SetAsFirstSibling();
            ingredientImage.transform.SetAsLastSibling();
        }
    }

    private void AutoDestroy()
    {
        Destroy(gameObject);
    }
}

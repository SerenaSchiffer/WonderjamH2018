using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ClientStatePotion
{
    Good,
    Bad,
    Waiting
}

public enum StateClient
{
    Joy,
    Annoyed,
    Angry,
}

public class Client : MonoBehaviour {

    public Melange melangeRef;
    public float maxTimer;
    
    [HideInInspector] public Melange melangeClient;
    [HideInInspector] public ServiceCounter myCounter;

    Rigidbody2D rb;
    InstantiableObjectContainer objectsReferences;
    Animator animator;
    float timer;
    bool hasArrived;
    ClientStatePotion melangeState = ClientStatePotion.Waiting;
    StateClient clientState = StateClient.Joy;
    GameObject melangeClientPopup;

    // Use this for initialization
    void Start () {
        timer = maxTimer;
        hasArrived = false;
        rb = GetComponent<Rigidbody2D>();
        objectsReferences = GetComponent<InstantiableObjectContainer>();
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(rb.velocity);
        if (timer >= (maxTimer / 3) * 2)
        {
            animator.SetBool("Idle", true);
            clientState = StateClient.Joy;

        }
        else if (timer >= (maxTimer / 3) && timer < (maxTimer / 3) * 2)
        {
            animator.SetBool("Annoyed", true);
            animator.SetBool("Idle", false);
            clientState = StateClient.Annoyed;
        }
        else if (timer >= 0 && timer < (maxTimer / 3))
        {
            animator.SetBool("Angry", true);
            animator.SetBool("Annoyed", false);
            clientState = StateClient.Angry;
        }
        else
        {
            ExitShop();
        }

        if(melangeState == ClientStatePotion.Good)
        {
            //TODO donner plus d'argent si content
            if(!animator.GetBool("Angry"))
            {
                animator.SetBool("Joy", true);
                animator.SetBool("Annoyed", false);
                animator.SetBool("Idle", false);
            }
            ExitShop();

        }
        else if(melangeState == ClientStatePotion.Bad)
        {
            if (!animator.GetBool("Angry"))
            {
                animator.SetBool("Angry", true);
                animator.SetBool("Joy", false);
                animator.SetBool("Annoyed", false);
                animator.SetBool("Idle", false);
            }
            ExitShop();
        }

        if (hasArrived)
            timer -= Time.deltaTime;

        if(melangeState == ClientStatePotion.Waiting)
        {
            if (!IsInFrontOfSomething())
            {
                hasArrived = true;
                rb.velocity = new Vector2(0, -1);
            }
        }
    }


    private void ExitShop()
    {
        if (myCounter.side == CounterSide.Left)
            rb.velocity = new Vector2(-1f, 0f);
        else
            rb.velocity = new Vector2(1f, 0f);

        Destroy(melangeClientPopup);
        Invoke("AutoDestroy", 3f);
    }

    private bool IsInFrontOfSomething()
    {
        int layerMask = LayerMask.GetMask(new String[] { "Interactable", "ObjectToStopClient" });
        RaycastHit2D hit = Physics2D.Raycast(transform.position - new Vector3(0f, 0.15f), new Vector3(0f, -1f), 0.2f, layerMask);
        Debug.DrawRay(transform.position - new Vector3(0f, 0.15f), new Vector3(0f, -0.2f));

        //GameObject other = hit.collider.gameObject;
        if (hit.collider != null)
        {
            rb.velocity = Vector2.zero;
            GenerateRecipe();
            

            if (hit.collider.gameObject.layer == 8)
            {
                myCounter = hit.collider.gameObject.GetComponent<ServiceCounter>();
                melangeState = myCounter.InteractWithClient(melangeClient,clientState);
            }
            else if (hit.collider.gameObject.layer == 10)
                myCounter = hit.collider.gameObject.GetComponent<Client>().myCounter;

            if (!myCounter.DoesQueueContain(gameObject))
                myCounter.AddClientToQueue(gameObject);

            return true;
        }

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

            Vector2 spawnPosition = new Vector2(transform.position.x, transform.position.y + 0.5f);
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
        }
    }

    private void AutoDestroy()
    {
        Destroy(gameObject);
    }
}

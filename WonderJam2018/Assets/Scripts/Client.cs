using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour {

    public Melange melangeRef;
    
    [HideInInspector] public Melange melangeClient;
    [HideInInspector] public ServiceCounter myCounter;

    Rigidbody2D rb;
    bool atPosition;
    InstantiableObjectContainer objectsReferences;

    // Use this for initialization
    void Start () {
        atPosition = false;
        rb = GetComponent<Rigidbody2D>();
        objectsReferences = GetComponent<InstantiableObjectContainer>();

    }
	
	// Update is called once per frame
	void Update () {
        
        if (!IsInFrontOfSomething())
            rb.velocity = new Vector2(0, -1);
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

            if(hit.collider.gameObject.layer == 8)
                myCounter = hit.collider.gameObject.GetComponent<ServiceCounter>();
            else if (hit.collider.gameObject.layer == 10)
                myCounter = hit.collider.GetComponent<Client>().myCounter;

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


            GameObject melangeClientPopup = Instantiate(objectsReferences.MelangeClientPopup);
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
}

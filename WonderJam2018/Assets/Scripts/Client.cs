using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour {

    public Melange melangeRef;
    
    [HideInInspector] public Melange melangeClient;

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
        
        if (!atPosition)
            rb.velocity = new Vector2(0, -1);
	}

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ClientTarget")
        {
            rb.velocity = Vector2.zero;
            GenerateRecipe();
            atPosition = true;
            other.GetComponent<Targets>().SetOccupied();
        }
        
        if (other.tag == "Potion")
        {
            Destroy(other);
            rb.velocity = new Vector2(1, 0);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "ClientTarget")
        {
            atPosition = false;
            other.GetComponent<Targets>().SetNotOccupied();
        }
    }

    private void GenerateRecipe()
    {
        melangeClient = Instantiate<Melange>(melangeRef);
        melangeClient.GenerateRandomRecipe();
        
        GameObject melangeClientPopup = Instantiate(objectsReferences.MelangeClientPopup);
        melangeClientPopup.transform.SetParent(GameObject.Find("Canvas").transform, false);

        AddIngredientToPopup(melangeClientPopup);

        Vector2 spawnPosition = new Vector2(transform.position.x - melangeClientPopup.transform.lossyScale.x, transform.position.y + 1f);
        melangeClientPopup.transform.position = Camera.main.WorldToScreenPoint(spawnPosition);
    }

    private void AddIngredientToPopup(GameObject melangeClientPopup)
    {
        foreach(Ingredient ingredient in melangeClient.mesIngredients)
        {
            GameObject ingredientImage = Instantiate(objectsReferences.ImageIngredientUI);
            ingredientImage.transform.SetParent(melangeClientPopup.transform, false);

            ingredientImage.GetComponent<Image>().sprite = ingredient.ingredientSprite;
        }
    }
}

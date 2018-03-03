using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaudron : MonoBehaviour {

    public Melange melangeRef;
    Melange myMelange;

    public float cookTime = 10f;
    public float burnTime = 6f;

    private float originalBurnTime;
    private float originalCookTime;

    private bool isFinished;

    private void Start()
    {
        myMelange = Instantiate<Melange>(melangeRef);
        myMelange.GenerateRandomRecipe();
        GetComponent<SpriteRenderer>().color = myMelange.MelangeColor();
        DebugAllIngredients();

        originalCookTime = cookTime;
        originalBurnTime = burnTime;

        isFinished = false;
    }

    public void AddIngredient(Ingredient i)
    {
        myMelange.AddIngredient(i);
    }

    public void Update()
    {
        if (isFinished) return;

        burnTime -= Time.deltaTime;
        cookTime -= Time.deltaTime;

        if (burnTime < float.Epsilon)
            Burn();

        if (cookTime < float.Epsilon)
            FinishCooking();
    }

    private void Burn()
    {
        Destroy(myMelange);
        myMelange = Instantiate<Melange>(melangeRef);
        myMelange.GenerateRandomRecipe();
        GetComponent<SpriteRenderer>().color = myMelange.MelangeColor();
        cookTime = originalCookTime;
        burnTime = originalBurnTime;
    }

    private void FinishCooking()
    {
        isFinished = true;
    }


    private void DebugAllIngredients()
    {
        foreach(Ingredient i in myMelange.mesIngredients)
        {
            Debug.Log(i.ingredientName);
        }
    }
}

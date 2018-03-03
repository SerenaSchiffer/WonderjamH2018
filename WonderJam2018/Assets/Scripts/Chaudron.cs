using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaudron : Interactable {
    /* "State Machine" */
    private enum ChaudronStates
    {
        Preparation,
        Cooking,
        Finished
    }
    ChaudronStates state;

    public Melange melangeRef;
    Melange myMelange;

    public float cookTime = 10f;
    public float burnTime = 6f;

    private float originalBurnTime;
    private float originalCookTime;

    private Animator myAnimator;

    private void Start()
    {
        myMelange = Instantiate<Melange>(melangeRef);

        originalCookTime = cookTime;
        originalBurnTime = burnTime;
        
        state = ChaudronStates.Preparation;
        myAnimator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (state != ChaudronStates.Cooking)
            return;
        burnTime -= Time.deltaTime;
        cookTime -= Time.deltaTime;

        if (burnTime < float.Epsilon)
            Burn();

        if (cookTime < float.Epsilon)
            FinishCooking();
    }

    override public void InteractWithPlayer(Ingredient playerItem)
    {
        if(playerItem == null)
        {
                
            
            switch(state)
                {
                case ChaudronStates.Preparation:
                        StartCooking();
                        break;
                case ChaudronStates.Cooking:
                        Mix();
                        break;
                case ChaudronStates.Finished:
                        FinishCooking();
                        break;
                }
        }
        else
        {
            AddIngredient(playerItem);
        }

    }

    //If objet dans les mains
    public void AddIngredient(Ingredient i)
    {
        if (state == ChaudronStates.Preparation)
        {
            myMelange.AddIngredient(i);
            //TODO : Add Ingredient Icon
        }
    }

    public int NumberOfIngredients()
    {
        return myMelange.mesIngredients.Count;
    }


    //Rien dans les mains et pas commencé de cooking
    public void StartCooking()
    {
        if (NumberOfIngredients() > 0)
        {
            GetComponent<SpriteRenderer>().color = myMelange.MelangeColor();
            state = ChaudronStates.Cooking;
            myAnimator.SetTrigger("StartCooking");
        }
    }


    private void Burn()
    {
        Destroy(myMelange);
        myMelange = Instantiate<Melange>(melangeRef);
        cookTime = originalCookTime;
        burnTime = originalBurnTime;

        state = ChaudronStates.Preparation;
        myAnimator.SetTrigger("Burn");
    }

    
    //Is cooking 
    public void Mix()
    {
        burnTime = originalBurnTime;
        myAnimator.SetTrigger("Mix");
    }

    private void FinishCooking()
    {
        state = ChaudronStates.Finished;
        myAnimator.SetTrigger("FinishCooking");
    }
    
    //Cooking time fini
    private void EmptyChaudron()
    {
        // TODO : Gérer de servir le mélange

        Destroy(myMelange);
        myMelange = Instantiate<Melange>(melangeRef);
        cookTime = originalCookTime;
        burnTime = originalBurnTime;
        state = ChaudronStates.Preparation;
    }

    private void DebugAllIngredients()
    {
        foreach(Ingredient i in myMelange.mesIngredients)
        {
            Debug.Log(i.ingredientName);
        }
    }
}

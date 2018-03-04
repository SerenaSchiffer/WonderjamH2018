using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject UIChaudron;

    private Animator myAnimator;

    public const int maxIngredient = 3;

    public GameObject particulesBurn;
    public GameObject alertBurn;

    public override void Start()
    {
        base.Start();
        myMelange = Instantiate<Melange>(melangeRef);

        originalCookTime = cookTime;
        originalBurnTime = burnTime;
        
        state = ChaudronStates.Preparation;
        myAnimator = GetComponent<Animator>();
    }

    public override void Update()
    {
        base.Update();
        if (state != ChaudronStates.Cooking)
            return;
        burnTime -= Time.deltaTime;
        cookTime -= Time.deltaTime;
        
        alertBurn.SetActive(burnTime < 4f);
        particulesBurn.SetActive(burnTime < 4f);

        if (burnTime < float.Epsilon)
            Burn();

        if (cookTime < float.Epsilon)
            FinishCooking();
    }

    override public PickableItem InteractWithPlayer(PickableItem playerItem)
    {
        if (playerItem as Ingredient != null)
        {
            AddIngredient((Ingredient)playerItem);
            return playerItem;
        }
        else if (playerItem == null)
        {


            switch (state)
            {
                case ChaudronStates.Preparation:
                    StartCooking();
                    return playerItem;
                case ChaudronStates.Cooking:
                    Mix();
                    return playerItem;
                case ChaudronStates.Finished:
                    return EmptyChaudron();
                default:
                    return null;
            }
        }
        else
            return playerItem;


    }

    //If objet dans les mains
    public void AddIngredient(Ingredient i)
    {
        if (state == ChaudronStates.Preparation)
        {
            if(myMelange.mesIngredients.Count < maxIngredient)
            {
                myMelange.AddIngredient(i);
                GameObject ingredientImage = (GameObject)Instantiate(Resources.Load("Prefabs/ImageUIChaudron"));
                ingredientImage.transform.SetParent(UIChaudron.transform, false);

                ingredientImage.GetComponent<Image>().sprite = i.mySprite;
            }
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
    private Melange EmptyChaudron()
    {
        // TODO : Gérer de servir le mélange
        Melange temp = myMelange;
        myMelange = null;
        myMelange = Instantiate<Melange>(melangeRef);
        cookTime = originalCookTime;
        burnTime = originalBurnTime;
        state = ChaudronStates.Preparation;
        myAnimator.SetTrigger("Empty");
        EmptyUI();
        return temp;
    }

    void EmptyUI()
    {
        for(int i = 0; i < UIChaudron.transform.childCount; i++)
        {
            Destroy(UIChaudron.transform.GetChild(i).gameObject);
        }
    }

    private void DebugAllIngredients()
    {
        foreach(Ingredient i in myMelange.mesIngredients)
        {
            Debug.Log(i.ingredientName);
        }
    }

    private void ClearUI() //TODO add where necessary
    {
        for (int i = 0; i < UIChaudron.transform.childCount; i++)
        {
            Destroy(UIChaudron.transform.GetChild(i).gameObject);
        }
    }

    public override void Highlight(PickableItem playerItem)
    {
        if (playerItem != null)
        {       
            if (playerItem as Melange == null)
            {
                if(NumberOfIngredients() < maxIngredient)
                {
                    if(playerItem as Ingredient != null && state == ChaudronStates.Preparation)
                        base.Highlight(playerItem);
                }

            }
        }
        else
        {
            if(NumberOfIngredients() > 0)
            {
                base.Highlight(playerItem);
            }
        }
    }
}

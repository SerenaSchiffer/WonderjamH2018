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

    [SerializeField] AudioClip audio_PourSomeLiquid;
    [SerializeField] AudioClip audio_DropItemInWater;
    [SerializeField] AudioClip audio_LightChaudron;
    [SerializeField] AudioClip audio_RecipeReady;
    [SerializeField] AudioClip audio_Explode;
    [SerializeField] AudioClip audio_Mix;

    private AudioManager audioMixer;

    public GameObject UIChaudron;

    private Animator myAnimator;

    public const int maxIngredient = 3;

    public GameObject particulesBurn;
    public GameObject alertBurn;
    public GameObject fallIngredient;
    bool fbFalling;

    public override void Start()
    {
        base.Start();
        myMelange = Instantiate<Melange>(melangeRef);

        originalCookTime = cookTime;
        originalBurnTime = burnTime;
        
        state = ChaudronStates.Preparation;
        myAnimator = GetComponent<Animator>();
        audioMixer = GameObject.Find("AudioMixer").GetComponent<AudioManager>();
        fbFalling = false;
    }

    public override void Update()
    {
        base.Update();
        if (fbFalling)
        {
            if (fallIngredient.transform.localPosition.y < 0.2f)
            {
                fallIngredient.SetActive(false);
                fbFalling = false;
            }
        }

        alertBurn.SetActive(burnTime < 4f && burnTime > 0f);
        particulesBurn.SetActive(burnTime < 4f && burnTime > 0f);

        if (state != ChaudronStates.Cooking)
            return;
        burnTime -= Time.deltaTime;
        cookTime -= Time.deltaTime;
        

        if (burnTime < float.Epsilon)
            Burn();

        if (cookTime < float.Epsilon)
            FinishCooking();      
    }

    override public PickableItem InteractWithPlayer(PickableItem playerItem)
    {
        if (playerItem as Ingredient != null)
        {
            audioMixer.PlaySfx(audio_DropItemInWater, 0);
            AddIngredient((Ingredient)playerItem);
            return playerItem;
        }
        else if (playerItem == null)
        {


            switch (state)
            {
                case ChaudronStates.Preparation:
                    audioMixer.PlaySfx(audio_LightChaudron, 0);
                    StartCooking();
                    return playerItem;
                case ChaudronStates.Cooking:
                    audioMixer.PlaySfx(audio_Mix, 0);
                    Mix();
                    return playerItem;
                case ChaudronStates.Finished:
                    audioMixer.PlaySfx(audio_RecipeReady,0);
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

                transform.parent.GetComponent<Animator>().SetTrigger("Shake");

                ingredientImage.GetComponent<Image>().sprite = i.mySprite;

                fallIngredient.GetComponent<SpriteRenderer>().sprite = i.mySprite;
                fallIngredient.transform.localPosition = new Vector2(0, 0.8f);
                fallIngredient.SetActive(true);
                fallIngredient.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                fbFalling = true;
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
        audioMixer.PlaySfx(audio_Explode, 0);
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
        burnTime = originalBurnTime;
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

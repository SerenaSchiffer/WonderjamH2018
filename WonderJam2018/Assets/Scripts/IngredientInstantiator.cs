using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;

public class IngredientInstantiator : MonoBehaviour {


    [SerializeField] GameObject ingredientGameobject;
    [SerializeField] float phaseTime;

    private List<Ingredient> ingredientList;
    public float timerCounter;
    private GameObject ingredientContainer;

	// Use this for initialization
	void Start () {
        ingredientContainer = GameObject.Instantiate(new GameObject("IngredientContainer"), new Vector2(0, 0), new Quaternion());
        StartSortingPhase();
        
    }
	
	// Update is called once per frame
	void Update () {
        if (timerCounter >= 0)
            timerCounter -= Time.deltaTime;
	}

    public void StartSortingPhase()
    {
        //Spawn ingredient on the ground
        SpawnIngredients();

        //Set a timer and a Callback
        StartTimer();
    }

    void SpawnIngredients()
    {
        if(ingredientList == null)
            ingredientList = new List<Ingredient>();
        if (ingredientList.Count == 0)
            ingredientList.AddRange( Resources.LoadAll<Ingredient>("ScriptableObject/Ingredients"));

        
        int nbIngredient = ingredientList.Count;
        int rotationValue = 360 / ingredientList.Count;
        for (int i = 0; i < nbIngredient ; i++)
        {
            //Vector2 newDirection = new Vector2(0 * Mathf.Cos(rotationValue * i) - 0.1f * Mathf.Sin(rotationValue * i), 0 * Mathf.Cos(rotationValue * i) + 0.1f * Mathf.Sin(rotationValue * i));
            
            int nextRandom = Random.Range(0, nbIngredient - i);
            nextRandom = Random.Range(0, nbIngredient - i);
            nextRandom = Random.Range(0, nbIngredient - i);
            nextRandom = Random.Range(0, nbIngredient - i);
            nextRandom = Random.Range(0, nbIngredient - i);
            nextRandom = Random.Range(0, nbIngredient - i);

            gameObject.transform.RotateAround(gameObject.transform.position, Vector3.forward, rotationValue);
            GameObject newInstance = Instantiate(ingredientGameobject, ingredientContainer.transform, false);
            newInstance.transform.position = gameObject.transform.GetChild(0).transform.position;
            newInstance.GetComponent<SpriteRenderer>().sprite = ingredientList[nextRandom].mySprite;
            newInstance.GetComponent<Item>().SetItem(ingredientList[nextRandom]);
            ingredientList.RemoveAt(nextRandom);
        }
    }

    void StartTimer()
    {
        timerCounter = phaseTime;
        Invoke("EndOfPhase", phaseTime);
    }

    public void EndOfPhase()
    {
        //EffacerTimer?
        Debug.Log("FIN DU TIMER");
        SortTheRestOfIngredients();
    }

    public void SortTheRestOfIngredients()
    {
        // Get all Ingredient
       //GameObject AllIngredient

        // Get all Boxes


        // put all ingredient


    }
    
}



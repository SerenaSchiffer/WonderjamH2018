using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;

public class IngredientInstantiator : MonoBehaviour
{


    [SerializeField] GameObject ingredientGameobject;
    [SerializeField] GameObject[] allBoxesOfPlayer;
    [SerializeField] float phaseTime;

    private List<Ingredient> ingredientList;
    private float timerCounter;
    private GameObject ingredientContainer;

    // Use this for initialization
    void Start()
    {
        Component Comp = new AssetIdentity();
        ingredientContainer = new GameObject("IngredientContainer",new System.Type[] {Comp.GetType()});
        ingredientContainer.transform.position = Vector2.zero;
        ingredientContainer.tag = "IngredientsLoose";
        ingredientContainer.gameObject.GetComponent<AssetIdentity>().SetPlayerIdentity(gameObject.GetComponent<AssetIdentity>().GetPlayerIdentity());
        StartCoroutine(SpawnItems());
    }

    IEnumerator SpawnItems()
    {
        yield return new WaitForSeconds(2f);
        StartSortingPhase();
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
        if (ingredientList == null)
            ingredientList = new List<Ingredient>();
        if (ingredientList.Count == 0)
            ingredientList.AddRange(Resources.LoadAll<Ingredient>("ScriptableObject/Ingredients"));


        int nbIngredient = ingredientList.Count;
        int rotationValue = 360 / ingredientList.Count;
        for (int i = 0; i < nbIngredient; i++)
        {
            //Vector2 newDirection = new Vector2(0 * Mathf.Cos(rotationValue * i) - 0.1f * Mathf.Sin(rotationValue * i), 0 * Mathf.Cos(rotationValue * i) + 0.1f * Mathf.Sin(rotationValue * i));

            int nextRandom = Random.Range(0, nbIngredient - i);
            nextRandom = Random.Range(0, nbIngredient - i);
            nextRandom = Random.Range(0, nbIngredient - i);
            nextRandom = Random.Range(0, nbIngredient - i);
            nextRandom = Random.Range(0, nbIngredient - i);
            nextRandom = Random.Range(0, nbIngredient - i);

            gameObject.transform.RotateAround(gameObject.transform.position, Vector3.forward, rotationValue);
            GameObject newInstance = Instantiate(ingredientGameobject, ingredientContainer.transform, true);
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
        SortTheRestOfIngredients();
        if(GameLoop.currentState != GameLoop.States.Journee)
            GameObject.Find("EventSystem").GetComponent<GameLoop>().StartVentes();
    }

    public void SortTheRestOfIngredients()
    {
        // Get all Ingredient
        GameObject[] AllIngredient = GameObject.FindGameObjectsWithTag("IngredientsLoose");
        GameObject RightSetOfIngredients = null;
        for (int i = 0; i < AllIngredient.Length; i++)
        {
            int ingredientCount = AllIngredient[i].transform.childCount;
            if(gameObject.GetComponent<AssetIdentity>().GetPlayerIdentity() == AllIngredient[i].GetComponent<AssetIdentity>().GetPlayerIdentity())
            {
                RightSetOfIngredients = AllIngredient[i];
            }

            //TODO GÉRER LES JOUEURS
        }


        // Get all Boxes
        //GameObject[] AllBoxesOfPlayer = GameObject.FindGameObjectsWithTag("Box");
        //GameObject RightSetOfBox = AllIngredient[0];
        // for (int i = 0; i < AllIngredient.Length; i++)
        //{
        //     AllBoxesOfPlayer[i].GetComponent<AssetIdentity>().GetPlayerIdentity();

        //TODO GÉRER LES JOUEURS
        // }

        // put all ingredient
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject actualPlayer = null;

        if (players[0].GetComponent<PlayerController>().IsSwapped)
        {
            if (players[0].GetComponent<PlayerController>().player == gameObject.GetComponent<AssetIdentity>().GetPlayerIdentity())
            {
                actualPlayer = players[1];
            }

            if (players[1].GetComponent<PlayerController>().player == gameObject.GetComponent<AssetIdentity>().GetPlayerIdentity())
            {
                actualPlayer = players[0];
            }
        }
        else
        {
            if (players[0].GetComponent<PlayerController>().player == gameObject.GetComponent<AssetIdentity>().GetPlayerIdentity())
            {
                actualPlayer = players[0];
            }

            if (players[1].GetComponent<PlayerController>().player == gameObject.GetComponent<AssetIdentity>().GetPlayerIdentity())
            {
                actualPlayer = players[1];
            }
        }



        if (actualPlayer != null)
        {
            if (allBoxesOfPlayer.Length != RightSetOfIngredients.transform.childCount && actualPlayer.GetComponent<PlayerController>().myItem != null)
            {
                GameObject newInstance = Instantiate(ingredientGameobject, ingredientContainer.transform, true);
                newInstance.transform.position = actualPlayer.transform.position;
                newInstance.GetComponent<SpriteRenderer>().sprite = actualPlayer.GetComponent<PlayerController>().myItem.mySprite;
                newInstance.GetComponent<Item>().SetItem((Ingredient)actualPlayer.GetComponent<PlayerController>().myItem);
                actualPlayer.GetComponent<PlayerController>().myItem = null;
            }
        }

            int j = 0;
        if (RightSetOfIngredients != null)
        {
            for (int i = 0; i < allBoxesOfPlayer.Length; i++)
            {
                if (allBoxesOfPlayer[i].GetComponent<Box>().myItem == null)
                {
                    StartCoroutine(PutItemInBox(allBoxesOfPlayer[i].GetComponent<Box>(), RightSetOfIngredients.transform.GetChild(j).GetComponent<Item>()));
                    RightSetOfIngredients.transform.GetChild(j).GetComponent<Item>().GoToPosition(allBoxesOfPlayer[i].transform.position);
                    j++;
                }
            }
        }
    }

    public void ForceDropItem()
    {
        // Get all Ingredient
        GameObject[] AllIngredient = GameObject.FindGameObjectsWithTag("IngredientsLoose");
        GameObject RightSetOfIngredients = null;
        for (int i = 0; i < AllIngredient.Length; i++)
        {
            int ingredientCount = AllIngredient[i].transform.childCount;
            if (gameObject.GetComponent<AssetIdentity>().GetPlayerIdentity() == AllIngredient[i].GetComponent<AssetIdentity>().GetPlayerIdentity())
            {
                RightSetOfIngredients = AllIngredient[i];
            }

            //TODO GÉRER LES JOUEURS
        }


        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject actualPlayer = null;

        if (players[0].GetComponent<PlayerController>().IsSwapped)
        {
            if (players[0].GetComponent<PlayerController>().player == gameObject.GetComponent<AssetIdentity>().GetPlayerIdentity())
            {
                actualPlayer = players[1];
            }

            if (players[1].GetComponent<PlayerController>().player == gameObject.GetComponent<AssetIdentity>().GetPlayerIdentity())
            {
                actualPlayer = players[0];
            }
        }
        else
        {
            if (players[0].GetComponent<PlayerController>().player == gameObject.GetComponent<AssetIdentity>().GetPlayerIdentity())
            {
                actualPlayer = players[0];
            }

            if (players[1].GetComponent<PlayerController>().player == gameObject.GetComponent<AssetIdentity>().GetPlayerIdentity())
            {
                actualPlayer = players[1];
            }
        }


        if (actualPlayer != null)
        {
            if (allBoxesOfPlayer.Length != RightSetOfIngredients.transform.childCount && actualPlayer.GetComponent<PlayerController>().myItem != null)
            {
                GameObject newInstance = Instantiate(ingredientGameobject, ingredientContainer.transform, true);
                newInstance.transform.position = actualPlayer.transform.position;
                newInstance.GetComponent<SpriteRenderer>().sprite = actualPlayer.GetComponent<PlayerController>().myItem.mySprite;
                newInstance.GetComponent<Item>().SetItem((Ingredient)actualPlayer.GetComponent<PlayerController>().myItem);
                actualPlayer.GetComponent<PlayerController>().myItem = null;
            }
        }

        int j = 0;
        if (RightSetOfIngredients != null)
        {
            for (int i = 0; i < allBoxesOfPlayer.Length; i++)
            {
                if (allBoxesOfPlayer[i].GetComponent<Box>().myItem == null)
                {
                    StartCoroutine(PutItemInBox(allBoxesOfPlayer[i].GetComponent<Box>(), RightSetOfIngredients.transform.GetChild(j).GetComponent<Item>()));
                    RightSetOfIngredients.transform.GetChild(j).GetComponent<Item>().GoToPosition(allBoxesOfPlayer[i].transform.position);
                    j++;
                }
            }
        }
    }

    IEnumerator PutItemInBox(Box box, Item item)
    {
        yield return new WaitForSeconds(0.01f);
        box.myItem = item.myItem;
    }
}



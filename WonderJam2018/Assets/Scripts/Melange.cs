using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Melange")]
public class Melange : PickableItem {
    public static Ingredient[] ingredients;

    public Queue<Ingredient> mesIngredients;

    private void OnEnable()
    {
        if (ingredients == null)
            ingredients = Resources.LoadAll<Ingredient>("ScriptableObject/Ingredients");
        if (mesIngredients == null)
            mesIngredients = new Queue<Ingredient>();
    }

    public void GenerateRandomRecipe()
    {
        mesIngredients.Clear();
        int numberOfIngredients = Random.Range(1, 4);

        for (int i = 0; i < numberOfIngredients; i++)
        {
            int ingredient = Random.Range(0, ingredients.Length);
            mesIngredients.Enqueue(ingredients[ingredient]);
        }
    }

    public void AddIngredient(Ingredient i)
    {
        mesIngredients.Enqueue(i);
    }

    public Color MelangeColor()
    {
        List<Color> allColors = new List<Color>();
        foreach (Ingredient i in mesIngredients)
            allColors.Add(i.mainColor);

        float rSum = 0f, gSum = 0f, bSum = 0f;

        foreach (Color c in allColors)
        {
            rSum += (c.r * 255) * (c.r * 255);
            gSum += (c.g * 255) * (c.g * 255);
            bSum += (c.b * 255) * (c.b * 255);
        }

        rSum /= allColors.Count;
        gSum /= allColors.Count;
        bSum /= allColors.Count;

        rSum = Mathf.Sqrt(rSum);
        gSum = Mathf.Sqrt(gSum);
        bSum = Mathf.Sqrt(bSum);

        return new Color(rSum / 255, gSum / 255, bSum / 255, 1f);
    }

    static float[] multiplier = { 0, 1f, 1.2f, 1.5f }; 
    public float MelangeValue()
    {
        float value = 0f;

        foreach (Ingredient i in mesIngredients)
            value += i.baseIngredientValue;

        value *= multiplier[mesIngredients.Count];

        return value;
    }
}

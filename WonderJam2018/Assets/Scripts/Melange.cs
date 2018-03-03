using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Melange")]
public class Melange : ScriptableObject {
    public static Ingredient[] ingredients;

    Queue<Ingredient> mesIngredients;


}

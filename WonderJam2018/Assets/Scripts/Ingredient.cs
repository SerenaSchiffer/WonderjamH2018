using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Ingredient")]
public class Ingredient : PickableItem {

    public string ingredientName;
    public int baseIngredientValue;
    public Color mainColor;
    public AudioClip swap;
    public AudioClip take;
    public AudioClip drop;
}

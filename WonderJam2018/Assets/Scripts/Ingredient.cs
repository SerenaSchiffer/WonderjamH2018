using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Ingredient")]
public class Ingredient : ScriptableObject {

    public Sprite ingredientSprite;
    public int baseIngredientValue;
    public Color mainColor;
}

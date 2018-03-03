using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Ingredient")]
public class Ingredient : PickableItem {

    public string ingredientName;
    public Sprite ingredientSprite;
    public int baseIngredientValue;
    public Color mainColor;
}

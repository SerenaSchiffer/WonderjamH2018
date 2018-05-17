using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeItem
{
    Potion,
    Ingredient
}

public class PickableItem : ScriptableObject {
    public Sprite mySprite;
    public TypeItem typedeItem;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaudron : MonoBehaviour {

    public Melange melangeRef;
    Melange randomMelange;

    private void Start()
    {
        randomMelange = Instantiate<Melange>(melangeRef);
        randomMelange.GenerateRandomRecipe();
        GetComponent<SpriteRenderer>().color = randomMelange.MelangeColor();
    }
}

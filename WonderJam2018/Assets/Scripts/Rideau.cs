using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rideau : Interactable {

    public override void Start()
    {
    }

    public override void Update()
    {
    }

    public override PickableItem InteractWithPlayer(PickableItem item)
    {
        return item;
    }
}

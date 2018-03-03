﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CounterSide
{
    Left,
    Right
};

public class ServiceCounter : Interactable {

    public CounterSide side;
    
    Queue<GameObject> clientsEnFile;

    [HideInInspector] public Melange potion;

	// Use this for initialization
	public override void Start () {
        base.Start();
        clientsEnFile = new Queue<GameObject>();
	}
	
	// Update is called once per frame
	public override void Update () {
        base.Update();
	}

    public void AddClientToQueue(GameObject newClient)
    {
        clientsEnFile.Enqueue(newClient);
    }

    public void PopClientFromQueue()
    {
        clientsEnFile.Dequeue();
    }

    public bool IsQueueFull()
    {
        if (clientsEnFile.Count == 2)
            return true;
        else
            return false;
    }

    public int GetQueueCount()
    {
        return clientsEnFile.Count;
    }
    
    public bool DoesQueueContain(GameObject client)
    {
        if (clientsEnFile.Contains(client))
            return true;
        else
            return false;
    }

    public override PickableItem InteractWithPlayer(PickableItem playerItem)
    {
        if(playerItem as Melange != null)
        {
            potion = (Melange)playerItem;
            return null;
        }
        else
        {
            return playerItem;
        }
    }

    public override void Highlight(PickableItem playerItem)
    {
        if(playerItem as Melange != null)
            base.Highlight(playerItem);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceCounter : MonoBehaviour {

    bool occupied;
    Queue<GameObject> clientsEnFile;

	// Use this for initialization
	void Start () {
        occupied = false;
        clientsEnFile = new Queue<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddClientToQueue(GameObject newClient)
    {
        if (clientsEnFile.Count < 2)
            clientsEnFile.Enqueue(newClient);
    }

    public void PopClientFromQueue()
    {
        occupied = false;
    }

    public bool IsQueueFull()
    {
        return occupied;
    }
}

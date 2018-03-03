using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletedOrderCounter : MonoBehaviour {

    [HideInInspector] public Queue<Melange> completedMelanges;

	// Use this for initialization
	void Start () {
        completedMelanges = new Queue<Melange>();
	}
	
	// Update is called once per frame
	void Update () {
		if (FindMatchingClient())
	}

    public void AddMelange(Melange newCompletedMelange)
    {
        completedMelanges.Enqueue(newCompletedMelange);
    }

    private GameObject FindMatchingClient()
    {
        Client[] clients;
        foreach (Client client in clients)
        {
            int cptGoodIngredient = 0;

            if (client == client.melangeClient.mesIngredients.Dequeue())
                cptGoodIngredient++;

            if (completedMelanges.Count == cptGoodIngredient)
                return client.gameObject;
        }

        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletedOrderCounter : MonoBehaviour {

    [HideInInspector] public Queue<Melange> completedMelanges;

    private InstantiableObjectContainer objectsReferences;

    // Use this for initialization
    void Start () {
        completedMelanges = new Queue<Melange>();
        objectsReferences = GetComponent<InstantiableObjectContainer>();
    }
	
	// Update is called once per frame
	void Update () {
        GameObject matchingClient = FindMatchingClient();

		if (matchingClient)
        {
            //GameObject newPotion = Instantiate(objectsReferences.Potion);
            //newPotion.GetComponent<SpriteRenderer>().sprite = SpriteResultant de la potion #scriptableoject
            //newPotion.GetComponent<Rigidbody2D>().velocity = (matchingClient.transform.position - newPotion.transform.position).normalized;

            //TODO: gérer une file par chaudron
        }
	}

    public void AddMelange(Melange newCompletedMelange)
    {
        completedMelanges.Enqueue(newCompletedMelange);
    }

    private GameObject FindMatchingClient()
    {
        List<Client> clients = new List<Client>();
        foreach(Transform child in objectsReferences.ContainerClients.transform)
        {
            clients.Add(child.GetComponent<Client>());
        }

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

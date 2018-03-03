using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSpawner : MonoBehaviour {

    public float spawnTimer;
    public GameObject targets;

    private float timer;
    private GameObject target;
    private InstantiableObjectContainer objectsReferences;

	// Use this for initialization
	void Start () {
        timer = spawnTimer;
        objectsReferences = GetComponent<InstantiableObjectContainer>();
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(timer);

		if (timer <= 0)
        {
            foreach(Transform child in targets.transform)
            {
                if (!child.GetComponent<Targets>().IsOccupied())
                    target = child.gameObject;
            }

            if (target)
            {
                Vector3 spawnPosition = new Vector3(target.transform.position.x, transform.position.y, 0);
                GameObject newClient = Instantiate(objectsReferences.Client, spawnPosition, new Quaternion()); //TODO donner un rotation qui a de l'allure
                newClient.transform.parent = objectsReferences.ContainerClients.transform;
            }

            timer = spawnTimer;
        }

        timer -= Time.deltaTime;
	}
}

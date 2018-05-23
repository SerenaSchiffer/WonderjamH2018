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
        Transform selectedCounter = targets.transform.GetChild(0);
        //if (selectedCounter.GetComponent<ServiceCounter>().GetQueueCount() < 2)
        if (!selectedCounter.GetComponent<ServiceCounter>().IsQueueFull())
        {
            target = selectedCounter.gameObject;
            Vector3 spawnPosition = new Vector3(target.transform.position.x, transform.position.y, 0);
            GameObject newClient = Instantiate(objectsReferences.Client, spawnPosition, new Quaternion()); //TODO donner un rotation qui a de l'allure
            newClient.transform.parent = objectsReferences.ContainerClients.transform;
        }
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(timer);
        target = null;

		if (timer <= 0)
        {
            int rdmIndex = Random.Range(0, 2);

            Transform selectedCounter = targets.transform.GetChild(rdmIndex);
            //if (selectedCounter.GetComponent<ServiceCounter>().GetQueueCount() < 2)
            if (!selectedCounter.GetComponent<ServiceCounter>().IsQueueFull())
            {
                target = selectedCounter.gameObject;
                Vector3 spawnPosition = new Vector3(target.transform.position.x, transform.position.y, 0);
                GameObject newClient = Instantiate(objectsReferences.Client, spawnPosition, new Quaternion()); //TODO donner un rotation qui a de l'allure
                newClient.transform.parent = objectsReferences.ContainerClients.transform;
            }
            spawnTimer = Mathf.Clamp(spawnTimer - 1, 10, 20);
            timer = spawnTimer;
        }

        timer -= Time.deltaTime;
	}
}

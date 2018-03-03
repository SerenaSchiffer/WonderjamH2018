using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour {

    [HideInInspector] Rigidbody2D rb;
    bool atPosition;

	// Use this for initialization
	void Start () {
        atPosition = false;
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        
        if (atPosition)
        {
            
        }
        else
            rb.velocity = new Vector2(0, -1);
	}

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ClientTarget")
        {
            rb.velocity = Vector2.zero;
            atPosition = true;
            other.GetComponent<Targets>().SetOccupied();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "ClientTarget")
        {
            atPosition = false;
            other.GetComponent<Targets>().SetNotOccupied();
        }
    }
}

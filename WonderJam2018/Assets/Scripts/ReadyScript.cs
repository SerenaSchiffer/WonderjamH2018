using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReadyScript : MonoBehaviour {

    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;

    bool player1Ready = false;
    bool player2Ready = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1"))
        {
            player1.transform.GetChild(0).GetComponent<Text>().color = Color.green;
            player1Ready = true;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            player2.transform.GetChild(0).GetComponent<Text>().color = Color.green;
            player2Ready = true;
        }

        if (player1Ready && player2Ready)
        {
            SceneManager.LoadScene("Nichzee");
        }
	}
}

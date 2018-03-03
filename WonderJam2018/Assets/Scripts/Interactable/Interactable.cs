using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    int highlightBuffer = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(highlightBuffer > 0)
        {
            highlightBuffer--;
        }
        else
        {
            UpdateHighlightShader();
        }
	}

    public virtual void InteractWithPlayer(Ingredient item)
    {

    }
     
    public virtual void Highlight()
    {
        Debug.Log("woah!");
        highlightBuffer = 3;
    }
    
    void UpdateHighlightShader()
    {

    }   


}

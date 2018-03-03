using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    int highlightBuffer = 0;
    GameObject interactableObject;

	// Use this for initialization
	public virtual void Start () {
        interactableObject = transform.Find("Highlight").gameObject;
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if(highlightBuffer > 0)
        {
            UpdateHighlightShader(true);
            highlightBuffer--;
        }
        else
        {
            UpdateHighlightShader(false);
        }
	}

    public virtual Ingredient InteractWithPlayer(Ingredient item)
    {
        return null;
    }
     
    public virtual void Highlight()
    {
        Debug.Log("woah!");
        highlightBuffer = 3;
    }
    
    void UpdateHighlightShader(bool value)
    {
        interactableObject.SetActive(value);
    }   


}

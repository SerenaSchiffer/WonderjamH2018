using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mainbox : MonoBehaviour {

    SpriteRenderer myRenderer;
    Box[] childs;

	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<SpriteRenderer>();
        childs = new Box[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            childs[i] = transform.GetChild(i).GetComponent<Box>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		foreach(Box b in childs)
        {
            if (b.GetHighlight() > 0)
            {
                myRenderer.enabled = false;
                break;
            }
            else
                myRenderer.enabled = true;
        }
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mainbox : MonoBehaviour {

    Animator myAnimator;

    Box[] childs;

	// Use this for initialization
	void Start () {
        myAnimator = GetComponent<Animator>();
        childs = new Box[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            childs[i] = transform.GetChild(i).GetComponent<Box>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        myAnimator.SetBool("isClosed", LookifOpened());
	}

    bool LookifOpened()
    {
        foreach (Box b in childs)
        {
            if (b.GetHighlight() > 0)
                return false;
        }
            return true;
    }
}

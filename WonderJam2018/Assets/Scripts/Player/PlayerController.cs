using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] int player;
    [SerializeField] bool useKeyboard;

    Vector2 directionToRaycast;

	// Use this for initialization
	void Start () {
		
	}
	
    void SetVelocity(Vector2 speed)
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = speed;
        directionToRaycast = speed.x > speed.y ? new Vector2(speed.x, 0) : new Vector2(0, speed.y);
    }

    void DoRaycast()
    {
        ShowRaycast();
        RaycastHit2D hit = Physics2D.Raycast(transform.position,directionToRaycast,1);
        
        if(hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Interactable")
            {
                //Do Something
            }
        }
    }

    void ShowRaycast()
    {

    }


    void HandleKeyboard()
    {

        Vector2 newSpeed = Vector2.zero;

        newSpeed += new Vector2(Input.GetAxisRaw("Horizontalk"), 0);
        newSpeed += new Vector2(0, Input.GetAxisRaw("Verticalk"));

        if (newSpeed.magnitude != Vector2.zero.magnitude)
        {
            SetVelocity(newSpeed);
        }
        else
        {
            SetVelocity(Vector2.zero);
        }

        if (Input.GetButtonDown("Firek"))
        {
            //TODO Pickup/Put/Throw items
            Debug.Log("Pick");
        }
    }
    // Update is called once per frame
    void FixedUpdate () {
        try
        {
            if (player != 1 && player != 2)
                throw new PlayerIdException("L'ID du player n'est pas setté dans le script PlayerController");

            if (useKeyboard)
            {
                HandleKeyboard();
            }
            else
            {
                Vector2 newSpeed = Vector2.zero;

                newSpeed += new Vector2(Input.GetAxisRaw("Horizontal"+player), 0);
                newSpeed += new Vector2(0, Input.GetAxisRaw("Vertical"+ player));

                if (newSpeed.magnitude != Vector2.zero.magnitude)
                {
                    SetVelocity(newSpeed);
                }
                else
                {
                    SetVelocity(Vector2.zero);
                }

                if (Input.GetButtonDown("Fire"+player))
                {
                    //TODO Pickup/Put/Throw items
                }
            }


            
        }
        catch (PlayerIdException e)
        {
            Debug.LogError(e);
        }




        
    }



}

[System.Serializable]
public class PlayerIdException : System.Exception
{
    public PlayerIdException() { }
    public PlayerIdException(string message) : base(message) { }
    public PlayerIdException(string message, System.Exception inner) : base(message, inner) { }
    protected PlayerIdException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}
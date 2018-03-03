using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] int player;
    [SerializeField] bool useKeyboard;
    public Ingredient myItem;
    [SerializeField]
    float speed = 1;

    const bool WillInteract = true;

    Vector2 directionToRaycast;

	// Use this for initialization
	void Start () {
		
	}
	
    void SetVelocity(Vector2 basicSpeed)
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = basicSpeed * speed;
        if (basicSpeed.magnitude > Vector2.zero.magnitude)
        {
            directionToRaycast = (Mathf.Abs(basicSpeed.x) > Mathf.Abs(basicSpeed.y)) ? new Vector2(basicSpeed.x, 0) : new Vector2(0, basicSpeed.y);
        }
    }
    
    void DoRaycast(bool toInteractWith)
    {
        ShowRaycast();
        int layer_mask = LayerMask.GetMask("Interactable");
        RaycastHit2D hit = Physics2D.Raycast(transform.position,directionToRaycast,1 , layer_mask);
        
        
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Interactable")
            {
                if(!toInteractWith)
                    hit.collider.gameObject.GetComponent<Interactable>().Highlight();

                if (toInteractWith)
                    hit.collider.gameObject.GetComponent<Interactable>().InteractWithPlayer(myItem);

            }
        }
    }

    void ShowRaycast()
    {
        Debug.Log(directionToRaycast);
       Debug.DrawRay((Vector2)transform.position, directionToRaycast);
    }


    void HandleKeyboard()
    {

        Vector2 newSpeed = Vector2.zero;

        newSpeed += new Vector2(Input.GetAxisRaw("Horizontalk"), 0);
        newSpeed += new Vector2(0, Input.GetAxisRaw("Verticalk"));

        if (newSpeed.magnitude != Vector2.zero.magnitude)
        {
            SetVelocity(newSpeed);
            DoRaycast(!WillInteract);
        }
        else
        {
            SetVelocity(Vector2.zero);
        }

        if (Input.GetButtonDown("Firek"))
        {
            //TODO Pickup/Put/Throw items
            DoRaycast(WillInteract);
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
                    DoRaycast(WillInteract);
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
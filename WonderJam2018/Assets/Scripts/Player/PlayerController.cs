using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public int player;
    [SerializeField] bool useKeyboard;
    public PickableItem myItem;
    [SerializeField]
    float speed = 1;

    public bool IsSwapped;
    SpriteRenderer itemRenderer;
    SpriteRenderer potionContentRenderer;
    Animator myAnim;
    Rigidbody2D rb2d;
    SpriteRenderer spriteRenderer;
    const bool WillInteract = true;
    GameObject otherPlayer;

    Vector2 directionToRaycast;

	// Use this for initialization
	void Start () {
        itemRenderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        potionContentRenderer = transform.GetChild(1).GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        myAnim = GetComponent<Animator>();
        myAnim.SetBool("FaceFront", true);
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (go.GetInstanceID() != gameObject.GetInstanceID())
                otherPlayer = go;
        }
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
            if(!toInteractWith)
                hit.collider.gameObject.GetComponent<Interactable>().Highlight(myItem);

            if (toInteractWith)
            {
                myItem = hit.collider.gameObject.GetComponent<Interactable>().InteractWithPlayer(myItem);
                if (myItem as Melange != null)
                    ((Melange)myItem).player = player;
            }

        }
    }

    void ShowRaycast()
    {
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
    void Update () {
        try
        {
            if (swappingPosition)
            {
                HandleSwap();
                return;
            }

            if (myItem != null)
            {
                if(myItem as Ingredient != null)
                {
                    itemRenderer.gameObject.SetActive(true);
                    potionContentRenderer.transform.parent.gameObject.SetActive(false);
                    itemRenderer.sprite = myItem.mySprite;
                }
                else if(myItem as Melange != null)
                {
                    itemRenderer.gameObject.SetActive(false);
                    potionContentRenderer.transform.parent.gameObject.SetActive(true);
                    potionContentRenderer.color = ((Melange)myItem).MelangeColor();
                    ((Melange)myItem).player = player;
                }
            }
            else
            {
                itemRenderer.sprite = null;
                potionContentRenderer.color = Color.white;
                itemRenderer.gameObject.SetActive(false);
                potionContentRenderer.transform.parent.gameObject.SetActive(false);
            }

            DoRaycast(!WillInteract);
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

    private void LateUpdate()
    {
        // Send Animator Value
        myAnim.SetBool("Holding", (myItem != null) );
        myAnim.SetBool("Walking", rb2d.velocity.magnitude > 0);

        if(directionToRaycast.x != 0)
        {
            myAnim.SetBool("FaceFront", false);
            myAnim.SetBool("FaceSide", true);
            myAnim.SetBool("FaceBack", false);

            if (directionToRaycast.x > 0)
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;
            return;
        }

        if (directionToRaycast.y > float.Epsilon)
        {
            myAnim.SetBool("FaceFront", false);
            myAnim.SetBool("FaceSide", false);
            myAnim.SetBool("FaceBack", true);
            return;
        }

        if (directionToRaycast.y < float.Epsilon)
        {
            myAnim.SetBool("FaceFront", true);
            myAnim.SetBool("FaceSide", false);
            myAnim.SetBool("FaceBack", false);
            return;
        }
    }

    public float travelDuration = 1f;
    Vector3 originalPos;
    Vector3 targetPos;
    float travelTime = 0f;
    public bool swappingPosition = false;
    private void HandleSwap()
    {
        float currentStep = 0;

        if (travelTime < travelDuration / 4)
        {
            currentStep = Mathf.Lerp(0.3f, 1f, travelTime * 2);
        }
        else if (travelTime > travelDuration / 4 * 3)
        {
            currentStep = Mathf.Lerp(0.3f, 1f, travelDuration - (travelTime / 4 * 3));
        }
        else
        {
            currentStep = 1f;
        }

        travelTime += Time.deltaTime * currentStep;
        
        transform.position = Vector3.Lerp(originalPos, targetPos, travelTime);

        if (travelTime > travelDuration)
            swappingPosition = false;
    }

    public void SwapPositions()
    {
        targetPos = otherPlayer.transform.position;
        originalPos = transform.position;
        travelTime = 0f;
        swappingPosition = true;
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
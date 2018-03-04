using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable {

    public Ingredient myItem;

    private AudioManager audioMixer;
    [SerializeField] private AudioClip swapSound;

    public float fadeInTime = 0f;

	// Use this for initialization
	void Start () {
        base.Start();
        mySprite = GetComponent<SpriteRenderer>();
        audioMixer = GameObject.Find("AudioMixer").GetComponent<AudioManager>();
    }

    SpriteRenderer mySprite;
	// Update is called once per frame
	void Update () {
        base.Update();

        if(audioMixer == null)
        {
            audioMixer = GameObject.Find("AudioMixer").GetComponent<AudioManager>();
        }

        if (fadeInTime < 1f)
        {
            mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, fadeInTime);
            fadeInTime += Time.deltaTime;
        }
        if (swappingPosition)
            HandleTranslate();
    }

    void AutoDestroy()
    {
        Destroy(gameObject);
    }

    public void SetItem(Ingredient newIngredient)
    {
        myItem = newIngredient;
    }

    void ResetSprite(Ingredient item)
    {
        if (item == null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = null;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = item.mySprite;
        }
        
    }
    

    public override PickableItem InteractWithPlayer(PickableItem playerItem)
    {
        if (playerItem as Ingredient == null)
        {
            Ingredient temp = myItem;
            ResetSprite(null);
            audioMixer.PlaySfx(swapSound, 0);
            Invoke("AutoDestroy", 0.05f);
            return temp;



        }
        else
        {
            return playerItem;
        }
    }

    public override void Highlight(PickableItem playerItem)
    {
        if (playerItem == null)
        {
            base.Highlight(playerItem);
        }
    }


    public float travelDuration = 1f;
    Vector3 originalPos;
    Vector3 targetPos;
    float travelTime = 0f;
    public bool swappingPosition = false;
    private void HandleTranslate()
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
            Destroy(gameObject);
    }

    public void GoToPosition(Vector3 pos)
    {
        targetPos = pos;
        originalPos = transform.position;
        travelTime = 0f;
        swappingPosition = true;
    }

}

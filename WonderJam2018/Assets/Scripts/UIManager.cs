using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField]
    Text player1Text;

    [SerializeField]
    Text player2Text;

    [SerializeField]
    Image clockImage;

    [SerializeField]
    float maxTimer;

    Vector3 basePosition1, basePosition2, goal1, goal2;
    public bool test = false;
    public float lerpSpeed = 0.05f;
    float timer = 0;


	// Use this for initialization
	void Start () {
        basePosition1 = player1Text.transform.position;
        basePosition2 = player2Text.transform.position;
        goal1 = basePosition1;
        goal2 = basePosition2;
        clockImage.transform.rotation = new Quaternion(0, 0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
		if(test)
        {
            SwapUI();
            test = false;
            lerpSpeed = 0.01f;
        }
        LookPosition();
        lerpSpeed += 0.03f;
        timer += Time.deltaTime;
        UpdateTimerImage();
	}

    void SwapUI()
    {
        goal1 = basePosition2;
        goal2 = basePosition1;
        basePosition1 = goal1;
        basePosition2 = goal2;        
    }

    void LookPosition()
    {
        float newX1 = Mathf.Lerp(player1Text.transform.position.x, goal1.x, lerpSpeed * Time.deltaTime);
        player1Text.transform.position = new Vector2(newX1, player1Text.transform.position.y);
        float newX2 = Mathf.Lerp(player2Text.transform.position.x, goal2.x, lerpSpeed * Time.deltaTime);
        player2Text.transform.position = new Vector2(newX2, player2Text.transform.position.y);
    }

    void UpdateScore()
    {
        //TODO manage score when player are created
    }

    void UpdateTimerImage()
    {
        float degreeRatio = (timer / maxTimer * 360);
        Quaternion rotation = clockImage.transform.rotation;
        rotation = Quaternion.AngleAxis(degreeRatio, Vector3.back);
        clockImage.transform.rotation = Quaternion.Lerp(clockImage.transform.rotation, rotation, 1);
    }    
}

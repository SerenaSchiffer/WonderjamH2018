using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject player1Score;
    public GameObject player2Score;

    Text player1Text;
    Text player2Text;

    [SerializeField]
    Image clockImage;

    [SerializeField]
    float maxTimer;

    Vector3 basePosition1, basePosition2, goal1, goal2;
    public bool test = false;
    public float lerpSpeed;
    float timer = 0;
    public float swapFraction = 0.25f;
    float swapValue;
    Animator overlay;

	// Use this for initialization
	void Start () {
        basePosition1 = player1Score.transform.position;
        basePosition2 = player2Score.transform.position;
        goal1 = basePosition1;
        goal2 = basePosition2;
        clockImage.transform.rotation = new Quaternion(0, 0, 0, 0);
        swapValue = swapFraction;
        player1Text = player1Score.GetComponentInChildren<Text>();
        player2Text = player2Score.GetComponentInChildren<Text>();
        overlay = GameObject.Find("Overlay").GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		if(test)
        {
            SwapUI();
            test = false;
            //lerpSpeed = 0.01f;
        }
        if (GameLoop.currentState == GameLoop.States.Journee)
        {
            LookPosition();
            //lerpSpeed += 0.03f;
            timer += Time.deltaTime;
            UpdateTimerImage();
            if (timer / maxTimer > swapValue)
            {
                swapValue += swapFraction;
                foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
                {
                    go.GetComponent<PlayerController>().SwapPositions();
                }
                SwapUI();
            }

            if(timer > maxTimer)
            {
                GameObject.Find("EventSystem").GetComponent<GameLoop>().FermerPortes();
            }
        }
	}

    void SwapUI()
    {
        goal1 = basePosition2;
        goal2 = basePosition1;
        basePosition1 = goal1;
        basePosition2 = goal2;
        overlay.SetTrigger("SWAP");
    }

    void LookPosition()
    {
        float newX1 = Mathf.Lerp(player1Text.transform.position.x, goal1.x, lerpSpeed /* * Time.deltaTime*/);
        player1Score.transform.position = new Vector2(newX1, player1Text.transform.position.y);
        float newX2 = Mathf.Lerp(player2Text.transform.position.x, goal2.x, lerpSpeed /* * Time.deltaTime*/);
        player2Score.transform.position = new Vector2(newX2, player2Text.transform.position.y);
    }

    public void UpdateScore(int player, float score)
    {
        //TODO BetterScore
        if (player == 1)
            player1Text.text = score.ToString();
        else if(player == 2)
        {
            player2Text.text = score.ToString();
        }
        
    }

    void UpdateTimerImage()
    {
        float degreeRatio = (timer / maxTimer * 360);
        Quaternion rotation = clockImage.transform.rotation;
        rotation = Quaternion.AngleAxis(degreeRatio, Vector3.back);
        clockImage.transform.rotation = Quaternion.Lerp(clockImage.transform.rotation, rotation, 1);
    }    
}

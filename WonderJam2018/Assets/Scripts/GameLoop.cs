using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoop : MonoBehaviour {

    public enum States
    {
        Rangement,
        Journee,
        Closed,
        EndDay
    }
    [SerializeField] AudioClip DuringDay;
    [SerializeField] AudioClip AfterDay;
    AudioManager audioMixer;

    public static States currentState;
    public GameObject rideau;


    private Vector3 rideauTarget;
    private Vector3 rideauPosition;
    private float rideauTemps;
    public GameObject clientSpawner1, clientSpawner2;
    public ServiceCounter[] serviceCounters;


    public void Awake()
    {
        currentState = States.Rangement;
        rideauTarget = Vector3.zero;
        rideauPosition = rideau.transform.position;
        rideauTemps = 0f;
    }

    public void Start()
    {
        audioMixer = GameObject.Find("AudioMixer").GetComponent<AudioManager>();
        audioMixer.PlayMusic(DuringDay);
    }


    public void StartVentes()
    {
        
        // Monter le Rideau
        rideauTarget = rideau.transform.position + Vector3.up * 5f;
        rideau.GetComponent<BoxCollider2D>().enabled = false;

        // Activer l'horloge et le swap
        GameObject.Find("Canvas").GetComponent<UIManager>();

        clientSpawner1.SetActive(true);
        clientSpawner2.SetActive(true);

        currentState = States.Journee;
    }

    public void FermerPortes()
    {
        audioMixer.PlayMusic(AfterDay);
        //Faire le panel de fermeture
        GameObject.Find("ClosedSign").GetComponent<Animator>().SetTrigger("CloseSign");

        clientSpawner1.SetActive(false);
        clientSpawner2.SetActive(false);

        currentState = States.Closed;
    }

    public void Update()
    {
        if (rideauTarget != Vector3.zero)
        {
            rideau.transform.position = Vector3.Lerp(rideauPosition, rideauTarget, rideauTemps / 3f);
            rideauTemps += Time.deltaTime;
            if (rideauTemps > 3f)
                rideauTarget = Vector3.zero;
        }

        if (currentState == States.Closed)
        {
            for(int i = 0; i < serviceCounters.Length; i++)
            {
                Debug.Log(i + "     " + serviceCounters[i].GetQueueCount());
                if(serviceCounters[i].GetQueueCount() > 0)
                {
                    return;
                }
            }
            currentState = States.EndDay;
            if(Score.scoreP1 > Score.scoreP2)
            {
                SceneManager.LoadScene("P1win");
            }
            else
            {
                SceneManager.LoadScene("P2win");
            }
        }
    }
	
}

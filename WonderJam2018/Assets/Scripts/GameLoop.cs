using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour {

    public enum States
    {
        Rangement,
        Journee,
        Closed
    }

    public static States currentState;
    public GameObject rideau;

    [SerializeField] AudioClip DuringDay;
    [SerializeField] AudioClip AfterDay;



    private Vector3 rideauTarget;
    private Vector3 rideauPosition;
    private float rideauTemps;
    private AudioSource thisAudioSource;


    public void Awake()
    {
        currentState = States.Rangement;
        rideauTarget = Vector3.zero;
        rideauPosition = rideau.transform.position;
        rideauTemps = 0f;
        thisAudioSource = gameObject.GetComponent<AudioSource>();
        thisAudioSource.clip = DuringDay;
        thisAudioSource.loop = true;
        thisAudioSource.Play();
    }

    public void StartVentes()
    {
        
        // Monter le Rideau
        rideauTarget = rideau.transform.position + Vector3.up * 5f;

        // Activer l'horloge et le swap
        GameObject.Find("Canvas").GetComponent<UIManager>();

        currentState = States.Journee;
    }

    public void FermerPortes()
    {
        thisAudioSource.clip = AfterDay;
        thisAudioSource.loop = true;
        thisAudioSource.Play();
        //Faire le panel de fermeture
        GameObject.Find("ClosedSign").GetComponent<Animator>().SetTrigger("CloseSign");

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
    }
	
}

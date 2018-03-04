using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    [SerializeField] AudioSource music;
    [SerializeField] AudioSource sfx;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayMusic(AudioClip aclip)
    {
        music.Stop();
        music.volume = 0.1f;
        music.clip = aclip;
        music.loop = true;
        music.Play();
        
    }

    public void PlaySfx(AudioClip aclip, float timeToLoop)
    {
        sfx.clip = aclip;
        sfx.loop = timeToLoop ==0 ? false : true;
        sfx.Play();

        if(sfx.loop)
            Invoke("StopSfx", timeToLoop);
    }

    private void StopMusic()
    {
        music.Stop();
    }

    private void StopSfx()
    {
        sfx.Stop();
    }
}

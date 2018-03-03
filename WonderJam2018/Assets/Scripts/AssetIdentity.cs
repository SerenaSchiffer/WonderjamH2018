using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetIdentity : MonoBehaviour {

    [SerializeField] int playerIdentity = 0;
	// Use this for initialization
	void Start () {
		
	}

    public int GetPlayerIdentity()
    {
        return playerIdentity;
    }

    public void SetPlayerIdentity(int newIdentity)
    {
        playerIdentity = newIdentity;
    }
	
	// Update is called once per frame
	void Update () {
        try
        {
            if (playerIdentity != 1 && playerIdentity != 2)
                throw new PlayerNotSetException("L'identité du player n'est pas 1 ou 2");
        }
        catch(PlayerNotSetException e)
        {
            Debug.LogError(e);
        }
	}

   

    
}

[System.Serializable]
public class PlayerNotSetException : System.Exception
{
    public PlayerNotSetException() { }
    public PlayerNotSetException(string message) : base(message) { }
    public PlayerNotSetException(string message, System.Exception inner) : base(message, inner) { }
    protected PlayerNotSetException(
      System.Runtime.Serialization.SerializationInfo info,
      System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}

    
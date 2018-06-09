using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKeyTakenListener
{
    //this gets called when a key is taken/added to keyholder
    void HandleKeyTaken();
}

/// <summary>
/// Acts as a "backpack" for keys the player collects.
/// </summary>
public class KeyHolderScript : MonoBehaviour {

    public static KeyHolderScript instance = null; //static reference to this script's instance.

    //a list or an array of key ids.. could have key objects..
    public List<int> KeyList;

    //a list of listener objects which are notified when a key is taken in the game.
    private List<IKeyTakenListener> keyTakenListeners;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        keyTakenListeners = new List<IKeyTakenListener>();
    }

    // Use this for initialization
    void Start () {
		
	}

    /// <summary>
    /// Adds a key id to keyholder list.
    /// </summary>
    /// <param name="keyId"></param>
    public void AddKeyToKeyHolder(int keyId)
    {
        //make sure we dont add duplicate keys to the holder..
        if(KeyList.Contains(keyId) == false)
        {
            KeyList.Add(keyId);
            //notify listeners
            NotifyKeyAddedListeners();
        }
    }

    /// <summary>
    /// Removes all keys from key holder.
    /// </summary>
    public void ClearKeyHolderKeys()
    {
        KeyList.Clear();
    }

    /// <summary>
    /// Tells whether the given key id is in the keyholder list or not.
    /// </summary>
    /// <param name="keyId"></param>
    /// <returns>true if key id is in keyholder list.</returns>
    public bool IsKeyInKeyHolder(int keyId)
    {
        if(KeyList.Contains(keyId) == true)
        {
            return true;
        }
        else
        {
            return false;
        }  
    }

    /// <summary>
    /// Adds a key taken listener to a list of key taken listeners.
    /// </summary>
    /// <param name="listener"></param>
    public void AddKeyTakenListener(IKeyTakenListener listener)
    {
        keyTakenListeners.Add(listener);
    }

    /// <summary>
    /// Removes given key taken listener from a list of key taken listeners.
    /// </summary>
    /// <param name="listener"></param>
    public void RemoveKeyTakenListener(IKeyTakenListener listener)
    {
        keyTakenListeners.Remove(listener);
    }

    public void ClearKeyTakenListeners()
    {
        keyTakenListeners.Clear();
    }

    /// <summary>
    /// Gets a list of all key ids in the key holder.
    /// </summary>
    /// <returns></returns>
    public List<int> GetKeyIdList()
    {
        return KeyList;
    }

    /// <summary>
    /// Lets every key taken listener know a key has been taken (by the player).
    /// </summary>
    private void NotifyKeyAddedListeners()
    {
        for (int i = 0; i < keyTakenListeners.Count; i++)
        {
            keyTakenListeners[i].HandleKeyTaken();
        }
    }
}

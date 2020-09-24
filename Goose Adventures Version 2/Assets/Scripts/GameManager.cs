using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Vector3 lastCheckpoint;
    public GameObject completeLevelUI;

    // Start is called before the first frame update
    void Awake ()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }    
    }

    public void SetLastCheckpoint(Vector3 newcheckpoint)
    {
        lastCheckpoint = newcheckpoint;
    }

    public void CompleteLevel()
    {
        completeLevelUI.SetActive(true);
    }
}

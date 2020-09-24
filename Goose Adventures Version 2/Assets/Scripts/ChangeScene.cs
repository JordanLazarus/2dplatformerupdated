using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{

    public string sceneName;
    public UIManager uiManager;

    void OnTriggerEntereD(Collider2D other)
    {
        SceneManager.LoadScene(2);
    }

}

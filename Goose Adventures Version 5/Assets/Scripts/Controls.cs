using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public GameObject openPanelUI;
    public GameObject closePanelUI;
    public Player player;
    public PauseMenu pause;
    
    public void OpenPanel()
    {
        if (openPanelUI != null)
        {
            openPanelUI.SetActive(!openPanelUI.activeSelf);
        }
    }

    public void ClosePanel()
    {
        if (openPanelUI != null)
        {
            openPanelUI.SetActive(false);
            Debug.Log("WORKS");
        }
    }

    public void Aim()
    {

    }

    public void Control()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject MainCanvas;
    [SerializeField] private GameObject InGameCanvas;
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject LosePanel;
    
    public void StartButton()
    {
       
        MainCanvas.SetActive(false);
        InGameCanvas.SetActive(true);
     
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        WinPanel.SetActive(false);
        LosePanel.SetActive(true);
    }

    public void Win()
    {
        LosePanel.SetActive(false);
        WinPanel.SetActive(true);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject Instruction;
    public GameObject Credits;
    public GameObject MainMenuPanel;
    

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void HowToPlay()
    {
        Instruction.gameObject.SetActive(true);
    }
    
    public void Credit()
    {
        Credits.gameObject.SetActive(true);
    }


    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void Back()
    {
        Instruction.SetActive(false);
        Credits.SetActive(false);
        MainMenuPanel.SetActive(true);
    }


}

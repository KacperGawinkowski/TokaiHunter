using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    [SerializeField] private GameObject SpeedLines;

    public void SetSpeedLinesOn(bool state)
    { 
        SpeedLines.SetActive(state);
    }

    public void GotOmENU()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}

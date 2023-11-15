using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pauseText = null;

    public void Pause()
    {
        if (isPaused) { Time.timeScale = 0; }
        else          { Time.timeScale = 1; }
        pauseText.SetActive(isPaused);
        isPaused = !isPaused;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus) {
            isPaused = true;
            Pause();
        }
        else
        {
            isPaused = false;
            Pause();
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            isPaused = true;
            Pause();
        }
        else
        {
            isPaused = false;
            Pause();
        }
    }
}

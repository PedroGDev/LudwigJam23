using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 1.0f;
    }
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PauseScene(bool pause)
    {
        GameObject.Find("Canvas").transform.GetChild(1).gameObject.SetActive(pause);
        Time.timeScale = pause ? 0f : 1f;
    }
}

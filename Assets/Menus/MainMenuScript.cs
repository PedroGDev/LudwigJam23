using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private AudioClip titleTheme;

    private void Start()
    {
        GameObject gamemanager = GameObject.Find("GameManager");
        gamemanager.GetComponent<AudioSource>().clip = titleTheme;
        gamemanager.GetComponent<AudioSource>().Play();
    }
}

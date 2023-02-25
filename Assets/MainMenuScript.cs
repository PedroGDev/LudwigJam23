using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private AudioClip titleTheme;

    private void Start()
    {
        GameObject.Find("GameManager").GetComponent<AudioSource>().clip = titleTheme;
        GameObject.Find("GameManager").GetComponent<AudioSource>().Play();
    }
}

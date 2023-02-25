using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { set; get; }
    public SaveState state;

	private static GameObject instance;

    private void Awake()
    {
		DontDestroyOnLoad(gameObject);
		if (instance == null)
			instance = gameObject;
		else
			Destroy(gameObject);
		Instance = this;
        Load();

        StartCoroutine("PlayTimer");
    }

    private void Start()
    {
        AudioMixer audioMixer = GetComponent<AudioSource>().outputAudioMixerGroup.audioMixer;

        audioMixer.SetFloat("MasterVol", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume", 1f)) * 20);
        audioMixer.SetFloat("MusicVol", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume", 1f)) * 20);
        audioMixer.SetFloat("SoundsVol", Mathf.Log10(PlayerPrefs.GetFloat("SoundsVolume", 1f)) * 20);
    }




    // Save the whole state of this saveState script to the player pref
    public void Save()
    {
        PlayerPrefs.SetString("save",Helper.Serialize<SaveState>(state));
    }

    // Load the previous saveed state from the player prefs
    public void Load()
    {
        // Do we already have a save??
        if(PlayerPrefs.HasKey("save"))
        {
            state = Helper.Deserialize<SaveState>(PlayerPrefs.GetString("save"));
        }
        else
        {
            state = new SaveState();
            Save();
            state.saveLevel = 1;
            Save();
            Debug.Log("NO SAVE FILE FOUND CREATING A NEW ONE!");
        }
    }

    // Playtime Counter
    private IEnumerator PlayTimer()
	{
        while(true)
		{
            yield return new WaitForSeconds(1);
            Instance.state.playtime += 1;
            Instance.Save();
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AchievementScript : MonoBehaviour
{
    [SerializeField] private AchievementScriptable ach;

    [SerializeField] private GameObject prefab;

    [SerializeField] private Sprite lockedSprite;
    [SerializeField] private Sprite unlockedSprite;

    [SerializeField] private GameObject scrollContent;

    [SerializeField] private TMP_Text percentageText;

    [SerializeField] private TMP_Text playtime;

    public void Start()
    {
        scrollContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 200 * ach.achieve.Count);
        int i = 0;
        float percent = 0;
        foreach (AchievementBox achievement in ach.achieve)
        {
            GameObject spawn = Instantiate(prefab);
            if (i == 0)
            {
                Destroy(spawn);
                spawn = Instantiate(prefab);
            }

            spawn.transform.SetParent(scrollContent.transform, false);

            spawn.transform.localPosition = new Vector3(225, -200 * i - 1, 0);

            spawn.name = "Achievement_" + i;

            spawn.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = achievement.icon;
            spawn.transform.GetChild(1).GetComponent<TMP_Text>().text = achievement.name;
            spawn.transform.GetChild(2).GetComponent<TMP_Text>().text = achievement.description;

            if (AchievementChecker(i))
            {
                spawn.GetComponent<Image>().sprite = unlockedSprite;
                if(i == 1)
                    spawn.transform.GetChild(2).GetComponent<TMP_Text>().text += "<br>Total Punches: " + SaveManager.Instance.state.punches;
                else if (i == 2)
                    spawn.transform.GetChild(2).GetComponent<TMP_Text>().text += "<br>Total Matches: " + SaveManager.Instance.state.matches;
                else if (i == 3)
                    spawn.transform.GetChild(2).GetComponent<TMP_Text>().text += "<br>Times with no Stamina: " + SaveManager.Instance.state.noStamina;
                else if (i == 4)
                    spawn.transform.GetChild(2).GetComponent<TMP_Text>().text += "<br>Attacks Blocked: " + SaveManager.Instance.state.blocked;
                else if (i == 5)
                    spawn.transform.GetChild(2).GetComponent<TMP_Text>().text += "<br>Lost Matches: " + SaveManager.Instance.state.loses;
                else if (i == 6)
                    spawn.transform.GetChild(2).GetComponent<TMP_Text>().text += "<br>Won Matches: " + SaveManager.Instance.state.wins;
                else if (i == 7)
                    spawn.transform.GetChild(2).GetComponent<TMP_Text>().text += "<br>Won Matches: " + SaveManager.Instance.state.wins;
                else if (i == 9)
                    spawn.transform.GetChild(2).GetComponent<TMP_Text>().text += "<br>No Hit Runs: " + SaveManager.Instance.state.noHitRun;
                percent++;
            }
            else
            {
                spawn.GetComponent<Image>().sprite = lockedSprite;
            }

            i++;
        }
        percentageText.text = ((percent / ach.achieve.Count) * 100f).ToString() + "%";

        if ((percent / (ach.achieve.Count - 1)) == 1)
        {
            SaveManager.Instance.state.complete = true;
            SaveManager.Instance.Save();
        }
    }

    private void Update()
    {
        int timer = SaveManager.Instance.state.playtime;

        int hours = Mathf.FloorToInt((timer / 3600) % 24);
        int minutes = Mathf.FloorToInt((timer / 60) % 60);
        int seconds = Mathf.FloorToInt((timer - minutes * 60) % 60);

        playtime.text = "Playtime: " + string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    public bool AchievementChecker(int num)
    {
        if (num == 0 && SaveManager.Instance.state.complete)
            return true;
        else if (num == 1 && SaveManager.Instance.state.punches > 0)
            return true;
        else if (num == 2 && SaveManager.Instance.state.matches > 0)
            return true;
        else if (num == 3 && SaveManager.Instance.state.noStamina > 0)
            return true;
        else if (num == 4 && SaveManager.Instance.state.blocked > 0)
            return true;
        else if (num == 5 && SaveManager.Instance.state.loses > 0)
            return true;
        else if (num == 6 && SaveManager.Instance.state.wins > 0)
            return true;
        else if (num == 7 && SaveManager.Instance.state.wins >= 3)
            return true;
        else if (num == 8 && SaveManager.Instance.state.ending)
            return true;
        else if (num == 9 && SaveManager.Instance.state.noHitRun > 0)
            return true;

        return false;
    }


    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        SaveManager.Instance.state.saveLevel = 0;
        SaveManager.Instance.state.playtime = 0;
        SaveManager.Instance.state.matches = 0;
        SaveManager.Instance.state.punches = 0;
        SaveManager.Instance.state.loses = 0;
        SaveManager.Instance.state.wins = 0;
        SaveManager.Instance.state.noStamina = 0;
        SaveManager.Instance.state.blocked = 0;
        SaveManager.Instance.state.ending = false;
        SaveManager.Instance.state.noHitRun = 0;
        SaveManager.Instance.state.complete = false;
        SaveManager.Instance.Save();
        SceneManager.LoadScene("Achievements");
    }
}

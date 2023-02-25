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

    public void Start()
    {
        scrollContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 200 * ach.achieve.Count);
        int i = 0;
        float percent = 0;
        foreach (AchievementBox achievement in ach.achieve)
        {
            GameObject spawn = Instantiate(prefab);
            if(i == 0)
            {
                Destroy(spawn);
                spawn = Instantiate(prefab);
            }
                
            spawn.transform.SetParent(scrollContent.transform, false);

            spawn.transform.localPosition = new Vector3(225, -200 * i - 1, 0);

            spawn.name = "Achievement_" + i;

            prefab.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = achievement.icon;
            prefab.transform.GetChild(1).GetComponent<TMP_Text>().text = achievement.name;
            prefab.transform.GetChild(2).GetComponent<TMP_Text>().text = achievement.description;

            if (AchievementChecker(i))
            {
                prefab.GetComponent<Image>().sprite = unlockedSprite;
                percent++;
            }
            else
            {
                prefab.GetComponent<Image>().sprite = lockedSprite;
            }

            i++;
        }
        percentageText.text = ((percent / ach.achieve.Count) * 100f).ToString() + "%";
    }

    public bool AchievementChecker(int num)
    {
        if (num == 0 && SaveManager.Instance.state.punches > 0)
            return true;
        else if (num == 1 && SaveManager.Instance.state.matches > 0)
            return true;
        else if (num == 2 && SaveManager.Instance.state.loses > 0)
            return true;
        else if (num == 3 && SaveManager.Instance.state.noStamina > 0)
            return true;
        else if (num == 4 && SaveManager.Instance.state.wins > 0)
            return true;
        else if (num == 5 && SaveManager.Instance.state.wins > 3)
            return true;
        else if (num == 6 && SaveManager.Instance.state.noHitRun)
            return true;
        else if (num == 7 && SaveManager.Instance.state.complete)
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
        SaveManager.Instance.state.noHitRun = false;
        SaveManager.Instance.state.complete = false;
        SaveManager.Instance.Save();
        SceneManager.LoadScene("Achievements");
    }
}

using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    private List<TabButtons> buttons;
    [SerializeField] private Sprite tabIdle;
    [SerializeField] private Sprite tabHover;
    [SerializeField] private Sprite tabSelected;
    private TabButtons selectedTab;
    [SerializeField] private bool selectOnStart;
    [SerializeField] private List<GameObject> objectsToSwap;

    private void Start()
    {
        if (selectOnStart && buttons != null)
        {

            OnTabSelected(buttons[0]);
        }
        else if (buttons == null)
        {
            Debug.LogError("A button must be set to be selected on start.");
        }
    }

    public void Subscribe(TabButtons button)
    {
        if (buttons == null)
        {
            buttons = new List<TabButtons>();
        }
        buttons.Add(button);
    }

    public void OnTabEnter(TabButtons button)
    {
        ResetTabs();
        if (selectedTab == null && button != selectedTab)
        {
            button.background.sprite = tabHover;
        }
        
    }

    public void OnTabExit(TabButtons button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButtons button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.sprite = tabSelected;
        int index = button.transform.GetSiblingIndex();
        for(int i = 0; i < objectsToSwap.Count; i++)
        {
            if(i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach (TabButtons button in buttons)
        {
            if (selectedTab != null && button == selectedTab)
            {
                continue;
            }
            button.background.sprite = tabIdle;
        }
    }
}

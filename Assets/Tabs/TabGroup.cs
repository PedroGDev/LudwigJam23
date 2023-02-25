using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToSwap;

    public void ToggleTabs(int num)
    {
        if(num == 0)
        {
            objectsToSwap[0].SetActive(true);
            objectsToSwap[1].SetActive(false);
        }
        else
        {
            objectsToSwap[1].SetActive(true);
            objectsToSwap[0].SetActive(false);
        }
    }
}

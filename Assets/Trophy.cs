using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trophy : MonoBehaviour
{
    private void Start()
    {
        if(SaveManager.Instance.state.ending)
            GetComponent<Image>().enabled = true;
        else
            GetComponent<Image>().enabled = false;
    }
}

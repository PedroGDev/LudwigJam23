using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    private void Start()
    {
        SaveManager.Instance.state.ending = true;
        SaveManager.Instance.Save();
    }
}
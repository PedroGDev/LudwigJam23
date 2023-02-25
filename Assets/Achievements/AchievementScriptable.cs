using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Achievements", menuName = "ScriptableObjects/Achievements", order = 1)]

public class AchievementScriptable : ScriptableObject
{
    public List<AchievementBox> achieve;
}

[Serializable]
public class AchievementBox
{
    public string name;
    public string description;
    public Sprite icon;
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class ChatBoxRandomizer : ScriptableObject
{
    public List<Chat> chat;
}

[Serializable]
public class Chat
{
    public string name;
    public string text;
}


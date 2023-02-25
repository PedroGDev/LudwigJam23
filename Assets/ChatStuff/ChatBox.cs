using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ChatBox : MonoBehaviour
{
    [SerializeField] private ChatBoxRandomizer chat;
    [SerializeField] private TMP_Text chatbox;

    private void Start()
    {
        chatbox.text = "";
        StartCoroutine(Chatting());
    }

    IEnumerator Chatting()
    {
        Chat temp;
        for (int i = 0; i < chat.chat.Count - 1; i++)
        {
            int rnd = UnityEngine.Random.Range(i, chat.chat.Count);
            temp = chat.chat[rnd];
            chat.chat[rnd] = chat.chat[i];
            chat.chat[i] = temp;
        }
        
        foreach (Chat text in chat.chat)
        {
            Color color = UnityEngine.Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
            Color32 color32 = (Color32)color;
            string hexColor = color32.r.ToString("X2") + color32.g.ToString("X2") + color32.b.ToString("X2");

            chatbox.text += "<b><color=#" + hexColor + ">";
            chatbox.text += text.name;
            chatbox.text += "</color></b>: ";
            chatbox.text += text.text;
            chatbox.text += "<br>";
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.2f,1.5f));
        }
        StartCoroutine(Chatting());
    }
}

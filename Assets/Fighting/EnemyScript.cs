using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class EnemyScript : MonoBehaviour
{
    public float enemyHealth = 100;
    public Animator enemyAnim;
    [SerializeField] private Slider enemySlider;
    [SerializeField] private PlayerScript playerScript;

    [SerializeField] private TMP_Text chatbox;

    private bool attackBuffer = false;
    public bool blockBuffer = false;
    private bool specialBuffer = false;

    private void Start()
    {
        enemySlider.maxValue = enemyHealth;
        enemySlider.value = enemyHealth;
        StartCoroutine(Cooldown(3f));
        StartCoroutine(ChooseAttack());
    }
    private void FixedUpdate()
    {
        if (!playerScript.fightEnd)
        {
            enemySlider.value = enemyHealth;


            if (enemyHealth <= 0)
                StartCoroutine(playerScript.EndScreen(true));

        }
    }

    IEnumerator AttackAnim(string punch)
    {
        if(!attackBuffer)
        {
            enemyAnim.SetTrigger(punch);
            yield return new WaitForSeconds(.5f);
            if (!playerScript.blockBuffer)
            {
                StartCoroutine(playerScript.GetDamaged(5));
            }
            else
            {
                playerScript.hit2Sound.Play();
                SaveManager.Instance.state.blocked++;
                SaveManager.Instance.Save();
                StartCoroutine(ShieldIcon());
            }
        }
        
    }
    IEnumerator Cooldown(float cd)
    {
        attackBuffer = true;
        yield return new WaitForSeconds(cd);
        attackBuffer = false;
    }

    IEnumerator BlockBuffer(float cd)
    {
        blockBuffer = true;
        yield return new WaitForSeconds(cd);
        blockBuffer = false;
    }

    IEnumerator SpecialAnim()
    {
        if (!specialBuffer && !attackBuffer)
        {
            Color color = UnityEngine.Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
            Color32 color32 = (Color32)color;
            string hexColor = color32.r.ToString("X2") + color32.g.ToString("X2") + color32.b.ToString("X2");
            string chat = "";
            chat += "<b><color=#" + hexColor + ">";
            chat += "Backseater66";
            chat += "</color></b>: ";
            chat += "<b><color=red>Super Punch incoming!!</color></b>!!!";
            chat += "<br>";
            chatbox.text += chat;
            yield return new WaitForSeconds(1.6f);
            chat = "";
            chat += "<b><color=#" + hexColor + ">";
            chat += "TheOneAndOnly111";
            chat += "</color></b>: ";
            chat += "<b><color=red>BLOCK NOW </color></b>!!!";
            chat += "<br>";
            chatbox.text += chat;
            yield return new WaitForSeconds(0.4f);
            specialBuffer = true;
            enemyAnim.SetTrigger("Special");
            StartCoroutine(Cooldown(1f));
            yield return new WaitForSeconds(0.6f);
            if (!playerScript.blockBuffer)
            {
                StartCoroutine(playerScript.GetDamaged(10));
            }
            else
            {
                playerScript.hit2Sound.Play();
                StartCoroutine(ShieldIcon());
                SaveManager.Instance.state.blocked++;
                SaveManager.Instance.Save();
            }
            yield return new WaitForSeconds(0.4f);

            if (!playerScript.blockBuffer)
            {
                StartCoroutine(playerScript.GetDamaged(10));
            }
            else
            {
                playerScript.hit2Sound.Play();
                StartCoroutine(ShieldIcon());
                SaveManager.Instance.state.blocked++;
                SaveManager.Instance.Save();
            }
        }
    }


    IEnumerator ShieldIcon()
    {
        playerScript.blockShield.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        playerScript.blockShield.SetActive(false);
    }

    IEnumerator ChooseAttack()
    {
        yield return new WaitForSeconds(.5f);
        if (!playerScript.fightEnd && !attackBuffer)
        {
            int roll = Random.Range(1, 5);

            if (roll == 1 && !attackBuffer)
            {
                StartCoroutine(AttackAnim("LeftPunch"));
            }
            else if (roll == 2 && !attackBuffer)
            {
                StartCoroutine(AttackAnim("RightPunch"));
            }
            else if (roll == 3 && !blockBuffer && !attackBuffer)
            {
                enemyAnim.SetTrigger("Block");
                StartCoroutine(BlockBuffer(0.2f));
            }
            else if (roll == 4 && enemyHealth < enemySlider.maxValue / 4 && !specialBuffer && !attackBuffer)
            {
                StartCoroutine(SpecialAnim());
            }

            if (enemyHealth < enemySlider.maxValue / 2)
            {
                StartCoroutine(Cooldown(Random.Range(2f, 3f)));
            }
            else
            {
                StartCoroutine(Cooldown(Random.Range(2.5f, 4f)));
            }
        }
        StartCoroutine(ChooseAttack());
    }

}

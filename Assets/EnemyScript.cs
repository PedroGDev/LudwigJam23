using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public float enemyHealth = 100;
    public Animator enemyAnim;
    [SerializeField] private Slider enemySlider;
    [SerializeField] private PlayerScript playerScript;

    private bool attackBuffer = false;
    public bool blockBuffer = false;

    private void Start()
    {
        enemySlider.maxValue = enemyHealth;
        enemySlider.value = enemyHealth;
        StartCoroutine(Cooldown(3f));
    }

    private void Update()
    {
        if(!playerScript.fightEnd)
        {
            enemySlider.value = enemyHealth;

            if (enemyHealth <= 0)
                StartCoroutine(playerScript.EndScreen(true));

            if (!attackBuffer)
            {
                int test = Random.Range(1, 4);
                if (test == 1)
                {
                    StartCoroutine(AttackAnim("LeftPunch"));
                }
                else if (test == 2)
                {
                    StartCoroutine(AttackAnim("RightPunch"));
                }
                else if (test == 3 && !blockBuffer)
                {
                    enemyAnim.SetTrigger("Block");
                    StartCoroutine(BlockBuffer(0.2f));
                }
                if (enemyHealth < enemySlider.maxValue / 2)
                {
                    StartCoroutine(Cooldown(Random.Range(0.5f, 1.5f)));
                }
                else
                {
                    StartCoroutine(Cooldown(Random.Range(1, 4)));
                }
            }
        }
        
    }

    IEnumerator AttackAnim(string punch)
    {
        enemyAnim.SetTrigger(punch);
        yield return new WaitForSeconds(.6f);
        if (!playerScript.blockBuffer)
        {
            StartCoroutine(playerScript.GetDamaged());
        }
        else
        {
            playerScript.hit2Sound.Play();
            print("BLOCKED");
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

}

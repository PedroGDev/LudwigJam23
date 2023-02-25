using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private Vector2 move;
    public Animator anim;    
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private EnemyScript enemyScript;
    public float health = 100;
    public float stamina = 10;
    private bool attackBuffer = false;

    public bool blockBuffer = false;

    private bool useStamina = true;

    public  AudioSource hitSound;
    public  AudioSource hit2Sound;
    public  AudioSource hit3Sound;

    [SerializeField] private Animator winAnim;
    [SerializeField] private Animator loseAnim;
    [SerializeField] private GameObject charles;
    [SerializeField] private GameObject ludwig;

    [SerializeField] private GameObject contBtn;
    [SerializeField] private GameObject retryBtn;
    [SerializeField] private GameObject goBackBtn;

    [SerializeField] private AudioClip fightTheme;
    [SerializeField] private AudioClip winTheme;
    [SerializeField] private AudioClip loseTheme;

    public GameObject blockShield;

    private SpriteRenderer playerSprite;
    private bool justGotDamage = false;

    public bool fightEnd = false;

    private void Start()
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;

        staminaSlider.maxValue = stamina;
        staminaSlider.value = stamina;

        StartCoroutine(StaminaUsage());

        playerSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();

        GameObject.Find("GameManager").GetComponent<AudioSource>().clip = fightTheme;
        GameObject.Find("GameManager").GetComponent<AudioSource>().Play();

        SaveManager.Instance.state.matches++;
        SaveManager.Instance.Save();
    }

    private void FixedUpdate()
    {
        if(!fightEnd)
        {
            if (move.x > 0.8 && !attackBuffer && stamina > 0)
            {
                anim.SetFloat("Dodge", move.x);
                StartCoroutine(Cooldown(.4f));
                StartCoroutine(BlockBuffer(0.3f));
            }
            else if (move.x < -0.8 && !attackBuffer && stamina > 0)
            {
                anim.SetFloat("Dodge", move.x);
                StartCoroutine(Cooldown(.4f));
                StartCoroutine(BlockBuffer(0.3f));
            }
            else
            {
                anim.SetFloat("Dodge", 0);
            }

            healthSlider.value = health;
            staminaSlider.value = stamina;

            if (stamina <= 0)
            {
                playerSprite.color = Color.blue;
            }
            else if (!justGotDamage)
                playerSprite.color = Color.white;


            if (health <= 0)
                StartCoroutine(EndScreen(false));
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!fightEnd)
        {
            move = context.ReadValue<Vector2>();
        }
    }

    public void OnLeftPunch(InputAction.CallbackContext context)
    {
        if (!fightEnd)
        {
            if (context.performed && stamina > 0)
            {
                if (move.y > 0.8)
                {
                    if (enemyScript.blockBuffer)
                    {
                        anim.SetTrigger("LeftUpPunch");
                        hit2Sound.Play();
                        stamina -= 1.5f;
                    }
                    else if (!attackBuffer)
                    {
                        anim.SetTrigger("LeftUpPunch");
                        hit3Sound.Play();
                        stamina -= 1.5f;
                        enemyScript.enemyHealth -= 5;
                        enemyScript.enemyAnim.SetTrigger("Hit");
                        StartCoroutine(Cooldown(.2f));
                    }
                }
                else
                {
                    if (enemyScript.blockBuffer)
                    {
                        anim.SetTrigger("LeftPunch");
                        hit2Sound.Play();
                        stamina -= 1;
                    }
                    else if (!attackBuffer)
                    {
                        anim.SetTrigger("LeftPunch");
                        hitSound.Play();
                        stamina -= 1;
                        enemyScript.enemyHealth -= 2;
                        enemyScript.enemyAnim.SetTrigger("Hit");
                        StartCoroutine(Cooldown(.4f));
                    }
                }
            }
        }
    }

    public void OnRightPunch(InputAction.CallbackContext context)
    {
        if (!fightEnd)
        {
            if (context.performed && stamina > 0)
            {
                if (move.y > 0.8)
                {
                    if (enemyScript.blockBuffer)
                    {
                        anim.SetTrigger("RightUpPunch");
                        hit2Sound.Play();

                        stamina -= 1.5f;
                    }
                    else if (!attackBuffer)
                    {
                        anim.SetTrigger("RightUpPunch");
                        hit3Sound.Play();
                        stamina -= 1.5f;
                        enemyScript.enemyHealth -= 5;
                        enemyScript.enemyAnim.SetTrigger("Hit");
                        StartCoroutine(Cooldown(.2f));
                    }
                }
                else
                {
                    if (enemyScript.blockBuffer)
                    {
                        anim.SetTrigger("RightPunch");
                        stamina -= 1;
                        hit2Sound.Play();
                    }
                    else if (!attackBuffer)
                    {
                        anim.SetTrigger("RightPunch");
                        hitSound.Play();
                        stamina -= 1;
                        enemyScript.enemyHealth -= 2;
                        enemyScript.enemyAnim.SetTrigger("Hit");
                        StartCoroutine(Cooldown(.4f));
                    }
                }
            }
        }
    }

    public void OnSpecial(InputAction.CallbackContext context)
    {
        if (!fightEnd)
        {
            if (context.performed)
            {
                if (enemyScript.blockBuffer && stamina > 0)
                {
                    hit2Sound.Play();
                }
                else if (!attackBuffer && stamina > 0)
                {
                    anim.SetTrigger("Special");
                    StartCoroutine(SpecialAnim());
                    StartCoroutine(Cooldown(1.5f));
                }
            }
        }
    }

    public void OnDuck(InputAction.CallbackContext context)
    {
        if (!fightEnd)
        {
            if (!context.performed)
                return;
            if (context.interaction is PressInteraction)
            {
                if (!attackBuffer && !blockBuffer && stamina > 0)
                {
                    anim.SetTrigger("Block");
                    stamina -= 1;
                    StartCoroutine(Cooldown(.4f));
                    StartCoroutine(BlockBuffer(0.5f));
                }
            }
        }
    }


    IEnumerator Cooldown(float cd)
    {
        attackBuffer = true;
        SaveManager.Instance.state.punches++;
        SaveManager.Instance.Save();
        yield return new WaitForSeconds(cd);
        attackBuffer = false;
    }

    IEnumerator SpecialAnim()
    {
        yield return new WaitForSeconds(0.5f);
        hit3Sound.Play();
        stamina -= 6;
        enemyScript.enemyHealth -= 20;
        enemyScript.enemyAnim.SetTrigger("Hit");
    }

    IEnumerator BlockBuffer(float cd)
    {
        blockBuffer = true;
        yield return new WaitForSeconds(cd);
        blockBuffer = false;
    }


    public IEnumerator EndScreen(bool win)
    {
        if(!fightEnd)
        {
            fightEnd = true;
            if (win)
                enemyScript.enemyAnim.SetTrigger("EnemyDown");
            else
                anim.SetTrigger("PlayerDown");
            Time.timeScale = 0.3f;
            yield return new WaitForSeconds(0.4f);
            Time.timeScale = 0.5f;
            yield return new WaitForSeconds(0.4f);
            Time.timeScale = 1f;
            yield return new WaitForSeconds(2f);
            GameObject.Find("Canvas").transform.GetChild(2).gameObject.SetActive(true);
            if (win)
            {
                SaveManager.Instance.state.wins++;
                if (health == healthSlider.maxValue)
                    SaveManager.Instance.state.noHitRun++;

                SaveManager.Instance.Save();
                winAnim.gameObject.SetActive(true);
                ludwig.SetActive(true);
                winAnim.SetTrigger("win");
                GameObject.Find("GameManager").GetComponent<AudioSource>().clip = winTheme;
                GameObject.Find("GameManager").GetComponent<AudioSource>().Play();
            }
            else
            {
                SaveManager.Instance.state.loses++;
                SaveManager.Instance.Save();
                loseAnim.gameObject.SetActive(true);
                charles.SetActive(true);
                loseAnim.SetTrigger("lose");
                GameObject.Find("GameManager").GetComponent<AudioSource>().clip = loseTheme;
                GameObject.Find("GameManager").GetComponent<AudioSource>().Play();
            }
            useStamina = false;
            yield return new WaitForSeconds(2f);
            goBackBtn.SetActive(true);
            if (win)
                contBtn.SetActive(true);
            else
                retryBtn.SetActive(true);
        }
    }


    private IEnumerator StaminaUsage()
    {
        if (stamina <= 1)
        {
            SaveManager.Instance.state.noStamina++;
            SaveManager.Instance.Save();
        }
        yield return new WaitForSeconds(stamina <= 1 ? 4f:1f);

        if (stamina <= staminaSlider.maxValue)
            stamina += 1;

        if (useStamina)
            StartCoroutine(StaminaUsage());
    }

    public IEnumerator GetDamaged(int damage)
    {
        justGotDamage = true;
        hitSound.Play();
        health -= damage;
        anim.SetTrigger("Hit");
        playerSprite.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        playerSprite.color = Color.white;
        justGotDamage = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TrainingMode : MonoBehaviour
{
    [SerializeField] private Vector2 move;
    public Animator anim;
    public float stamina = 10;
    private bool attackBuffer = false;
    [SerializeField] Slider staminaSlider;

    public bool blockBuffer = false;

    private bool useStamina = true;

    public AudioSource hitSound;
    public AudioSource hit2Sound;
    public AudioSource hit3Sound;

    private SpriteRenderer playerSprite;
    private bool justGotDamage = false;

    private void Start()
    {
        StartCoroutine(StaminaUsage());

        staminaSlider.maxValue = stamina;
        staminaSlider.value = stamina;

        playerSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Update()
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
        if (stamina <= 0)
        {
            playerSprite.color = Color.blue;
        }
        else if (!justGotDamage)
            playerSprite.color = Color.white;

        staminaSlider.value = stamina;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnLeftPunch(InputAction.CallbackContext context)
    {
        if (context.performed && stamina > 0 && !attackBuffer)
        {
            if (move.y > 0.8)
            {
                anim.SetTrigger("LeftUpPunch");
                hit3Sound.Play();
                stamina -= 1;
                StartCoroutine(Cooldown(.2f));
            }
            else
            {
                anim.SetTrigger("LeftPunch");
                hitSound.Play();
                stamina -= 1;
                StartCoroutine(Cooldown(.4f));
            }
        }
    }

    public void OnRightPunch(InputAction.CallbackContext context)
    {
        if (context.performed && stamina > 0 && !attackBuffer)
        {
            if (move.y > 0.8)
            {
                anim.SetTrigger("RightUpPunch");
                hit3Sound.Play();
                stamina -= 1;
                StartCoroutine(Cooldown(.2f));
            }
            else
            {
                anim.SetTrigger("RightPunch");
                hitSound.Play();
                stamina -= 1;
                StartCoroutine(Cooldown(.4f));
            }
        }
    }

    public void OnSpecial(InputAction.CallbackContext context)
    {
        if (context.performed && stamina > 0 && !attackBuffer)
        {
            anim.SetTrigger("Special");
            StartCoroutine(SpecialAnim());
            StartCoroutine(Cooldown(1.5f));
        }
    }

    public void OnDuck(InputAction.CallbackContext context)
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
                StartCoroutine(BlockBuffer(0.3f));
            }
        }
    }


    IEnumerator Cooldown(float cd)
    {
        attackBuffer = true;
        yield return new WaitForSeconds(cd);
        attackBuffer = false;
    }

    IEnumerator SpecialAnim()
    {
        yield return new WaitForSeconds(0.5f);
        hit3Sound.Play();
        stamina -= 2;
    }

    IEnumerator BlockBuffer(float cd)
    {
        blockBuffer = true;
        yield return new WaitForSeconds(cd);
        blockBuffer = false;
    }

    private IEnumerator StaminaUsage()
    {
        yield return new WaitForSeconds(stamina <= 1 ? 4f : 1f);
        if (stamina <= staminaSlider.maxValue)
            stamina += 1;

        if (useStamina)
            StartCoroutine(StaminaUsage());
    }
}

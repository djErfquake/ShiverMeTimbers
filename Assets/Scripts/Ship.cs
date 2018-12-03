using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ship : MonoBehaviour
{
    private Player player;


    [Header("Movement")]
    public float rotationSpeed = 20f;
    public float maxSpeed = 0.15f;
    public float moveSpeed = 0.02f;
    public float slowSpeed = 0.01f;
    private float speed = 0f;



    [Header("Cannons")]
    public List<Cannon> cannons;


    [Header("Banners")]
    public SpriteRenderer banners;


    // components
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;


    // tweens
    private Tween invincibleTween;


    // starting values
    private Vector3 startingPosition;
    private Vector3 startingRotation;

    private float startingRotationSpeed = 20f;
    private float startingMaxSpeed = 0.15f;
    private float startingCannonCooldown = 1f;



    public void Setup(Player p)
    {
        player = p;

        rb = GetComponent<Rigidbody2D>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = player.mainShipSprites[0];

        startingPosition = transform.position;
        startingRotation = transform.eulerAngles;

        startingRotationSpeed = rotationSpeed;
        startingMaxSpeed = maxSpeed;
        startingCannonCooldown = cannons[0].cooldown;

        Debug.Log("startingRotationSpeed: " + startingRotationSpeed);
    }


    public void Reset()
    {
        rotationSpeed = startingRotationSpeed;
        maxSpeed = startingMaxSpeed;
        for (int i = 0; i < cannons.Count; i++) { cannons[i].cooldown = startingCannonCooldown; }
    }



    private void Update()
    {
        if (!player.invincible && !paused)
        {
            // rotate
            //transform.Rotate(Vector3.back * Input.GetAxis("Horizontal " + player.playerNumber) * rotationSpeed * speed);
            transform.Rotate(Vector3.back * player.joystick.GetJoystickState(PlayerJoystick.Buttons.Horizontal) * rotationSpeed * speed);

            // move forward
            //if (Input.GetButton("Go " + player.playerNumber))
            if (player.joystick.GetButtonState(PlayerJoystick.Buttons.Go))
            {
                speed = Mathf.Lerp(speed, maxSpeed, moveSpeed);
                rb.angularVelocity = 0;
            }
            else
            {
                speed = Mathf.Lerp(speed, 0f, slowSpeed);
                if (speed < 0.005) { speed = 0; }
            }

            transform.localPosition += transform.up * -speed;


            // fire cannons
            //if (Input.GetButton("Fire " + player.playerNumber))
            if (player.joystick.GetButtonState(PlayerJoystick.Buttons.Fire))
            {
                for (int i = 0; i < cannons.Count; i++)
                {
                    cannons[i].Fire();
                }
            }
        }
    }


    private bool paused = false;
    public void Pause(bool pause)
    {
        paused = pause;

        if (rb != null)
        {
            rb.isKinematic = pause;
            rb.constraints = (pause) ? RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.None;
            rb.velocity = Vector2.zero;
            rb.angularDrag = 0f;
        }
    }



    public void ReturnToDock()
    {
        transform.position = startingPosition;
        transform.eulerAngles = startingRotation;

        speed = 0;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    



    public void SetSprite(Sprite newShipSprite, Sprite newBannerSprite)
    {
        spriteRenderer.sprite = newShipSprite;
        banners.sprite = newBannerSprite;
    }




    
    public void Flash(bool active)
    {
        if (active)
        {
            invincibleTween.Kill();
            invincibleTween = transform.DOScale(2f, 1f).SetEase(Ease.InCubic).SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            invincibleTween.Kill();
            transform.localScale = Vector2.one * 1.5f;
        }
    }
    

}

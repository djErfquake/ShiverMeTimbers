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


    // starting values
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
        if (!player.invincible)
        {
            // rotate
            transform.Rotate(Vector3.back * Input.GetAxis("Horizontal " + player.playerNumber) * rotationSpeed * speed);

            // move forward
            if (Input.GetButton("Go " + player.playerNumber))
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
            if (Input.GetButton("Fire " + player.playerNumber))
            {
                for (int i = 0; i < cannons.Count; i++)
                {
                    cannons[i].Fire();
                }
            }
        }
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
            
        }
        else
        {
            
        }
    }
    

}

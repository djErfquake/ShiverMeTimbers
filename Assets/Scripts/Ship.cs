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
    public GameObject cannonballPrefab;
    public List<GameObject> cannons = new List<GameObject>();
    public float cannonballSpeed = 6f;
    public float fireRate = 1f;
    private bool canFire = true;


    // components
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;



    public void Setup(Player p)
    {
        player = p;

        rb = GetComponent<Rigidbody2D>();
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = player.sprites[0];
    }




    private void Update()
    {
        // rotate
        transform.Rotate(Vector3.back * Input.GetAxis("Horizontal " + player.playerNumber) * rotationSpeed * speed);

        // move forward
        if (Input.GetKey(KeyCode.Space) || Input.GetButton("Go " + player.playerNumber))
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
        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetButton("Fire " + player.playerNumber)) && canFire)
        {
            foreach (GameObject cannon in cannons)
            {
                StartCoroutine(JustFired());
                //Instantiate(cannonballPrefab, cannon.transform.position, cannon.transform.rotation);
                GameObject cannonball = Instantiate(cannonballPrefab, cannon.transform.position, cannon.transform.rotation);
                cannonball.layer = LayerMask.NameToLayer("Player " + player.playerNumber + " Cannonball");
                cannonball.tag = gameObject.tag;
                cannonball.GetComponent<Rigidbody2D>().velocity = cannonball.transform.right * cannonballSpeed;
                cannonball.GetComponent<Projectile>().firingPlayer = player;
                Destroy(cannonball, 10f);
            }

        }
    }


    private IEnumerator JustFired()
    {
        canFire = false;
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }



    public void SetSprite(Sprite newSprite)
    {
        spriteRenderer.sprite = newSprite;
    }


    
}

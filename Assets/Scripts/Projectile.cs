using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public GameObject explosion;

    [HideInInspector]
    public Player firingPlayer;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ship")
        {
            // hit player
            Player hitPlayer = collision.collider.GetComponent<Player>();
            if (hitPlayer != null)
            {
                hitPlayer.TakeDamageFrom(firingPlayer);
            }
        }

        // explosion
        GameObject ex = Instantiate(explosion);
        ex.transform.position = transform.position;

        // destroy self
        Destroy(gameObject);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public Player player;

    public GameObject cannonballPrefab;
    public Transform cannonballContainer;
    public float cannonOffset = 25f;
    public float cannonballSpeed = 500f;
    public float cooldown = 1f;
    private bool canFire = true;



    private void OnTriggerStay2D(Collider2D collision)
    {
        bool isShip = LayerMask.LayerToName(collision.gameObject.layer) == "Ship";
        bool samePlayer = collision.gameObject.tag == gameObject.tag;

        if (isShip && !samePlayer)
        {
            Fire(collision);
        }
    }


    public void Fire(Collider2D target = null)
    {
        if (canFire)
        {
            AudioManager.instance.PlaySoundEffect(AudioManager.SoundType.Cannon);

            StartCoroutine(JustFired());
            //Instantiate(cannonballPrefab, cannon.transform.position, cannon.transform.rotation);
            GameObject cannonball = Instantiate(cannonballPrefab);
            cannonball.tag = gameObject.tag;
            cannonball.transform.SetParent(cannonballContainer);
            cannonball.transform.position = transform.position + (transform.right * cannonOffset);
            cannonball.transform.rotation = transform.rotation;

            Vector2 direction = (target != null) ? -(transform.position - target.transform.position).normalized : cannonball.transform.right;
            Vector2 cannonballVelocity = direction * cannonballSpeed;
            
            cannonball.GetComponent<Rigidbody2D>().velocity = cannonballVelocity;
            cannonball.GetComponent<Projectile>().firingPlayer = player;
            Destroy(cannonball, 10f);
        }
    }



    private IEnumerator JustFired()
    {
        canFire = false;
        yield return new WaitForSeconds(cooldown);
        canFire = true;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay((transform.right * cannonOffset) + transform.position, transform.right * 50f);
    }

}

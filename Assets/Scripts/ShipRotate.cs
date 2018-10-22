using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRotate : MonoBehaviour
{
    public float rotationSpeed = 0.3f;

    private Player player;

    public void Setup(Player p)
    {
        player = p;
    }

    public void Reset()
    {
        player = null;
    }


    private void Update ()
    {
        if (player != null)
        {
            // rotate
            transform.Rotate(Vector3.back * Input.GetAxis("Horizontal " + player.playerNumber) * rotationSpeed);
        }
    }
}

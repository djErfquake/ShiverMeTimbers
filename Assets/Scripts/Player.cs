using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerNumber = "Any";

    [Header("Ship")]
    public Ship ship;

    [Header("Health")]
    public List<Sprite> sprites = new List<Sprite>();
    private int health = 0;


    [Header("Upgrades")]
    private UpgradeSystem upgradeSystem;




    private void Start()
    {
        ship.Setup(this);
    }




    public void GetUpgrade()
    {
        upgradeSystem.ShowUpgradeOptions();
        Debug.Log(gameObject.name + " getting upgrade");
    }


    public void TakeDamageFrom(Player offendingPlayer)
    {
        health++;
        if (health < 3)
        {
            ship.SetSprite(sprites[health]);
        }
        else if (health == 3)
        {
            Debug.Log(gameObject.name + " is dead!");
            offendingPlayer.GetUpgrade();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string playerNumber = "Any";

    [Header("Ship")]
    public Ship ship;

    [Header("Health")]
    public List<Sprite> mainShipSprites = new List<Sprite>();
    public List<Sprite> bannerSprites = new List<Sprite>();
    private int health = 0;
    [HideInInspector]
    public bool invincible = false;


    [Header("Upgrades")]
    public UpgradeSystem upgradeSystem;
    private Coroutine upgradeCoroutine;


    [HideInInspector]
    public bool joystickActive = false;




    private void Start()
    {
        ship.Setup(this);
        upgradeSystem.Hide();
    }



    public void Pause(bool pause)
    {
        ship.Pause(pause);
    }




    public void GetUpgrade()
    {
        Invincible(true);

        upgradeSystem.ShowUpgradeOptions();
        Debug.Log(gameObject.name + " getting upgrade");
        if (upgradeCoroutine != null) { StopCoroutine(upgradeCoroutine); upgradeCoroutine = null; }
        upgradeCoroutine = StartCoroutine(ExhibitUtilities.DoActionAfterTime(() =>
        {
            upgradeSystem.UpgradeFort();
            Invincible(false);
        }, 7f));
    }

    public void GotUpgrade()
    {
        if (upgradeCoroutine != null) { StopCoroutine(upgradeCoroutine); upgradeCoroutine = null; }
        Invincible(false);
    }


    public void Invincible(bool active)
    {
        invincible = active;
        ship.Flash(active);
    }


    public void TakeDamageFrom(Player offendingPlayer)
    {
        if (!invincible)
        {
            health++;
            if (health < mainShipSprites.Count)
            {
                ship.SetSprite(mainShipSprites[health], bannerSprites[health]);
            }

            if (health == mainShipSprites.Count - 1)
            {
                invincible = true;
                Debug.Log(gameObject.name + " is dead!");
                offendingPlayer.GetUpgrade();

                StartCoroutine(ExhibitUtilities.DoActionAfterTime(() =>
                {
                    Revive();
                }, 5f));
            }
        }
    }


    private void Revive()
    {
        health = 0;

        invincible = false;

        ship.ReturnToDock();
        ship.SetSprite(mainShipSprites[health], bannerSprites[health]);
    }
}

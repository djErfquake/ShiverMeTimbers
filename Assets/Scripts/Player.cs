using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public PlayerJoystick joystick;

    [Header("Ship")]
    public Color color;
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

    [Header("Join Screen")]
    public PlayerJoinSection playerJoinSection;


    [HideInInspector]
    public bool joystickActive = false;




    private void Start()
    {
        ship.Setup(this);
        upgradeSystem.Hide();
        joystick.joystickEvent.AddListener(JoystickEventHandler);
    }



    public void Pause(bool pause)
    {
        ship.Pause(pause);
    }

    private void JoystickEventHandler(JoystickData e)
    {
        if (upgradeSystem.UpgradeActive)
        {
            if (e.button == PlayerJoystick.Buttons.Yes) { upgradeSystem.UpgradeFort(); }
            else if (e.button == PlayerJoystick.Buttons.No) { upgradeSystem.UpgradeShip(); }
        }
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
            AudioManager.instance.PlaySoundEffect(AudioManager.SoundType.Hit);

            CameraShaker.Instance.ShakeOnce(1500f, 7f, 0.1f, 1f);
            joystick.Vibrate();

            health++;
            if (health < mainShipSprites.Count)
            {
                ship.SetSprite(mainShipSprites[health], bannerSprites[health]);
            }

            if (health == mainShipSprites.Count - 1)
            {
                AudioManager.instance.PlaySoundEffect(AudioManager.SoundType.Death);

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

    public void StartGame(bool playing)
    {
        ship.enabled = playing;
        SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < sr.Length; i++)
        {
            sr[i].enabled = playing;
        }

        Revive();
    }

    public void StopGame()
    {
        ship.enabled = false;
        SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < sr.Length; i++)
        {
            sr[i].enabled = false;
        }
    }
}

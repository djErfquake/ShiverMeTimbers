using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSystem : MonoBehaviour
{

    public const string SHIP_ROTATION = "SHIP_ROTATION";
    public const string SHIP_SPEED = "SHIP_SPEED";
    public const string SHIP_FIRE_RATE = "SHIP_FIRE_RATE";


    private int shipUpgradeCount = 0;
    private List<string> shipUpgrades = new List<string>()
    {
        SHIP_ROTATION, SHIP_SPEED, SHIP_FIRE_RATE
    };


    [Header("UI")]
    public GameObject ui;
    public MultipleTargetCamera cam;
    public GameObject fortButton, shipButton;

    [Header("Player")]
    public Player player;

    [Header("Fort")]
    public List<UpgradeSet> fortUpgrades;
    public GameObject banner;
    private int fortUpgradeCount = 0;


    private bool upgradeActive = false;
    public bool UpgradeActive { get { return upgradeActive; } }



    public void ShowUpgradeOptions()
    {
        ui.SetActive(true);
        cam.targets.Add(ui.transform);

        if (shipUpgradeCount >= shipUpgrades.Count) { shipButton.SetActive(false); }

        ui.transform.localScale = Vector2.zero;
        ui.transform.DOScale(1, 0.8f).SetEase(Ease.OutElastic);

        upgradeActive = true;
    }


    public void HideUpgradeOptions()
    {
        upgradeActive = false;

        ui.transform.localScale = Vector2.one;
        ui.transform.DOScale(0, 0.4f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            cam.targets.Remove(ui.transform);
            ui.SetActive(false);
        });
    }


    public void UpgradeShip()
    {
        if (shipUpgradeCount < shipUpgrades.Count)
        {
            AudioManager.instance.PlaySoundEffect(AudioManager.SoundType.ShipUpgrade);

            HideUpgradeOptions();

            switch (shipUpgrades[shipUpgradeCount])
            {
                case SHIP_ROTATION:
                    player.ship.rotationSpeed += 0.1f;
                    break;
                case SHIP_SPEED:
                    player.ship.maxSpeed += 1f;
                    break;
                case SHIP_FIRE_RATE:
                    for (int i = 0; i < player.ship.cannons.Count; i++) { player.ship.cannons[i].cooldown -= 0.3f; }
                    break;
            }

            shipUpgradeCount++;

        }

        player.GotUpgrade(fortUpgradeCount);
    }


    public void UpgradeFort()
    {
        AudioManager.instance.PlaySoundEffect(AudioManager.SoundType.FortUpgrade);

        HideUpgradeOptions();

        fortUpgradeCount++;
        for (int i = 0; i < fortUpgrades.Count; i++) { fortUpgrades[i].SetActive(false); }
        fortUpgrades[fortUpgradeCount - 1].SetActive(true);
        banner.SetActive(true);

        if (fortUpgradeCount >= fortUpgrades.Count) { GameManager.instance.ShowWinMenu(player); }

        player.GotUpgrade(fortUpgradeCount);
    }


    public void Reset()
    {
        Hide();

        shipUpgradeCount = 0;
        shipButton.SetActive(true);

        player.ship.Reset();
    }

    public void Hide()
    {
        HideUpgradeOptions();

        fortUpgradeCount = 0;
        for (int i = 0; i < fortUpgrades.Count; i++) { fortUpgrades[i].SetActive(false); }
        banner.SetActive(false);
    }
}

[System.Serializable]
public class UpgradeSet
{
    public List<GameObject> gameObjects = new List<GameObject>();

    public void SetActive(bool active)
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].SetActive(active);
        }
    }
}
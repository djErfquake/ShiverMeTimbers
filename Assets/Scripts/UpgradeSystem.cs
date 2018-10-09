﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour {

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
    public GameObject upgradeShipButton;
    public MultipleTargetCamera cam;

    [Header("Ship")]
    public Ship ship;

    [Header("Fort")]
    public List<GameObject> fortUpgrades;
    public List<GameObject> cannons;
    public GameObject banner;
    private int fortUpgradeCount = 0;




    public void ShowUpgradeOptions()
    {
        ui.SetActive(true);
        cam.targets.Add(ui.transform);
    }


    public void HideUpgradeOptions()
    {

        cam.targets.Remove(ui.transform);
        ui.SetActive(false);
    }


    public void UpgradeShip()
    {
        HideUpgradeOptions();

        switch (shipUpgrades[shipUpgradeCount])
        {
            case SHIP_ROTATION:
                ship.rotationSpeed += 0.1f;
                break;
            case SHIP_SPEED:
                ship.maxSpeed += 1f;
                break;
            case SHIP_FIRE_RATE:
                for (int i = 0; i < ship.cannons.Count; i++) { ship.cannons[i].cooldown -= 0.3f; }
                break;
        }

        shipUpgradeCount++;
        if (shipUpgradeCount >= shipUpgrades.Count)
        {
            upgradeShipButton.SetActive(false);
        }
    }


    public void UpgradeFort()
    {
        HideUpgradeOptions();

        fortUpgradeCount++;
        for (int i = 0; i < fortUpgrades.Count; i++) { fortUpgrades[i].SetActive(false); }

        fortUpgrades[fortUpgradeCount - 1].SetActive(true);
        if (fortUpgradeCount >= 1) { for (int i = 0; i < cannons.Count; i++) { cannons[i].SetActive(true); } }
        banner.SetActive(true);

        if (fortUpgradeCount >= fortUpgrades.Count)
        {
            Debug.Log("You win!");
        }
    }


    public void Reset()
    {
        HideUpgradeOptions();

        fortUpgradeCount = 0;
        for (int i = 0; i < fortUpgrades.Count; i++) { fortUpgrades[i].SetActive(false); }
        for (int i = 0; i < cannons.Count; i++) { cannons[i].SetActive(false); }
        banner.SetActive(false);

        shipUpgradeCount = 0;
        ship.Reset();
        upgradeShipButton.SetActive(true);
    }
}

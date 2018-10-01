using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour {

    public enum Upgrade { PlusRotation }

    public Ship ship;
    public List<GameObject> fortUpgrades;

    public GameObject cameraLockTransform;
    public GameObject ui;
    public List<GameObject> upgradeOptions;
    private GameObject randomUpgradeOption;

    //public MultipleTargetCamera cam;


    private void Start()
    {
        HideUpgradeOptions();
    }

    public void ShowUpgradeOptions()
    {
        int randomUpgradeIndex = Random.Range(0, upgradeOptions.Count);
        for (int i = 0; i < upgradeOptions.Count; i++)
        {
            if (i == randomUpgradeIndex)
            {
                randomUpgradeOption = upgradeOptions[i];
                randomUpgradeOption.SetActive(true);
            }
            else
            {
                upgradeOptions[i].SetActive(false);
            }
        }
        upgradeOptions.RemoveAt(randomUpgradeIndex);

        cameraLockTransform.SetActive(true);
        ui.SetActive(true);

        //cam.targets.Add(cameraLockTransform.transform);
    }


    public void HideUpgradeOptions()
    {
        if (randomUpgradeOption != null)
        {
            Destroy(randomUpgradeOption);
            randomUpgradeOption = null;
        }

        //cam.targets.Remove(cameraLockTransform.transform);
        cameraLockTransform.SetActive(false);
        ui.SetActive(false);
    }


    public void UpgradeRotationSpeed()
    {
        ship.rotationSpeed += 10f;
        HideUpgradeOptions();
    }


    public void FortUpgrade()
    {
        HideUpgradeOptions();

        bool upgraded = false;
        for (int i = 0; i < fortUpgrades.Count; i++)
        {
            if (!fortUpgrades[i].activeSelf)
            {
                fortUpgrades[i].SetActive(true);
                upgraded = true;
                break;
            }
        }

        if (!upgraded)
        {
            Debug.Log("You win!");
        }
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerJoinScreen : MonoBehaviour
{

    #region singleton
    // singleton
    public static PlayerJoinScreen instance;
    private void Awake()
    {
        if (instance && instance != this) { Destroy(gameObject); return; }
        instance = this;
    }
    #endregion


    public PlayerJoinSection[] playerJoinSections;
    public TextMeshProUGUI advanceText;


    public void ShowAdvanceText(bool show)
    {
        float alpha = show ? 1f : 0f;
        advanceText.DOFade(alpha, 3f);
    }


    public void AddPlayer(Player p, int index)
    {
        int numOfPlayers = GetTotalPlayers() + 1;

        int addedPlayers = 0;
        for (int i = 0; i < playerJoinSections.Length; i++)
        {
            float newWidth = Screen.width / numOfPlayers;
            float newX = newWidth * addedPlayers;

            if (i == index)
            {
                playerJoinSections[i].AddPlayer(p, newX, newWidth);
                addedPlayers++;
            }
            else if (playerJoinSections[i].playerActive)
            {
                playerJoinSections[i].SetNewSize(newX, newWidth);
                addedPlayers++;
            }
        }
    }


    public void RemovePlayer(int playerIndex)
    {
        playerJoinSections[playerIndex].RemovePlayer();

        int numOfPlayers = GetTotalPlayers();
        if (numOfPlayers > 0)
        {
            int addedPlayers = 0;
            for (int i = 0; i < playerJoinSections.Length; i++)
            {
                float newWidth = Screen.width / numOfPlayers;
                float newX = newWidth * addedPlayers;

                if (playerJoinSections[i].playerActive)
                {
                    playerJoinSections[i].SetNewSize(newX, newWidth, 1f);
                    addedPlayers++;
                }
            }
        }
    }




    public int GetTotalPlayers()
    {
        int numPlayers = 0;
        for (int i = 0; i < playerJoinSections.Length; i++)
        {
            if (playerJoinSections[i].playerActive)
            {
                numPlayers++;
            }
        }
        return numPlayers;
    }


    public bool PlayerAdded(int index)
    {
        return playerJoinSections[index].playerActive;
    }




    private void Start()
    {
        Reset();
    }


    public void Reset()
    {
        for (int i = 0; i < playerJoinSections.Length; i++)
        {
            playerJoinSections[i].Reset();
        }
    }

}

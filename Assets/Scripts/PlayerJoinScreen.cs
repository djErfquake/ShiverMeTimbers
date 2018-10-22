using System.Collections;
using System.Collections.Generic;
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



    public void AddPlayer(Player p)
    {
        int numOfPlayers = GetTotalPlayers();
        PlayerJoinSection newPlayerSection = playerJoinSections[0];
        for (int i = 0; i < playerJoinSections.Length; i++)
        {
            if (!playerJoinSections[i].playerActive)
            {
                newPlayerSection = playerJoinSections[i];
                break;
            }
        }
        numOfPlayers++;


        int addedPlayers = 0;
        for (int i = 0; i < playerJoinSections.Length; i++)
        {
            float newWidth = Screen.width / numOfPlayers;
            float newX = newWidth * addedPlayers;

            if (playerJoinSections[i] == newPlayerSection)
            {
                newPlayerSection.AddPlayer(p, newX, newWidth);
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
        playerJoinSections[playerIndex].Reset();

        int numOfPlayers = GetTotalPlayers();

        int addedPlayers = 0;
        for (int i = 0; i < playerJoinSections.Length; i++)
        {
            float newWidth = Screen.width / numOfPlayers;
            float newX = newWidth * addedPlayers;

            if (playerJoinSections[i].playerActive)
            {
                playerJoinSections[i].SetNewSize(newX, newWidth);
                addedPlayers++;
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

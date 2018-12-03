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


    public Player[] players;
    private Dictionary<Player, PlayerJoinSection> playerJoinSections = new Dictionary<Player, PlayerJoinSection>();
    public TextMeshProUGUI advanceText;


    public void ShowAdvanceText(bool show)
    {
        float alpha = show ? 1f : 0f;
        advanceText.DOFade(alpha, 3f);
    }


    public void AddPlayer(Player p)
    {
        int numOfPlayers = GetTotalPlayers() + 1;

        int addedPlayers = 0;
        foreach (KeyValuePair<Player, PlayerJoinSection> joinSection in playerJoinSections)
        {
            float newWidth = Screen.width / numOfPlayers;
            float newX = newWidth * addedPlayers;

            if (joinSection.Key == p)
            {
                joinSection.Value.AddPlayer(p, newX, newWidth);
                addedPlayers++;
            }
            else if (joinSection.Value.playerActive)
            {
                joinSection.Value.SetNewSize(newX, newWidth);
                addedPlayers++;
            }
        }
    }


    public void RemovePlayer(Player p)
    {
        playerJoinSections[p].RemovePlayer();

        int numOfPlayers = GetTotalPlayers();
        if (numOfPlayers > 0)
        {
            int addedPlayers = 0;
            foreach (KeyValuePair<Player, PlayerJoinSection> joinSection in playerJoinSections)
            {
                float newWidth = Screen.width / numOfPlayers;
                float newX = newWidth * addedPlayers;

                if (joinSection.Value.playerActive)
                {
                    joinSection.Value.SetNewSize(newX, newWidth, 1f);
                    addedPlayers++;
                }
            }
        }
    }




    public int GetTotalPlayers()
    {
        int numPlayers = 0;
        foreach (KeyValuePair<Player, PlayerJoinSection> joinSection in playerJoinSections)
        {
            if (joinSection.Value.playerActive)
            {
                numPlayers++;
            }
        }
        return numPlayers;
    }

    public bool PlayerAdded(Player p)
    {
        return playerJoinSections[p].playerActive;
    }




    private void Start()
    {
        playerJoinSections.Add(players[0], players[0].playerJoinSection);
        playerJoinSections.Add(players[1], players[1].playerJoinSection);
        playerJoinSections.Add(players[2], players[2].playerJoinSection);
        playerJoinSections.Add(players[3], players[3].playerJoinSection);

        Reset();
    }


    public void Reset()
    {
        foreach (KeyValuePair<Player, PlayerJoinSection> joinSection in playerJoinSections)
        {
            joinSection.Value.Reset();
        }
    }

}

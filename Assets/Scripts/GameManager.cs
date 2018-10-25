using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public enum GameState { MainMenu, PlayersReady, Game, End };
    private GameState gameState = GameState.Game;

    private JoystickManager joystickManager;
    private PlayerJoinScreen playersScreen;


    [Header("Menus")]
    public Vector3 mainMenuCameraPosition;
    public Vector3 playerMenuCameraPosition;
    public Button playButton;


    [Header("Game")]
    public List<Player> players;



    




    private void Start()
    {
        joystickManager = JoystickManager.instance;
        joystickManager.joystickEvent.AddListener(JoystickAddedOrRemoved);

        playersScreen = PlayerJoinScreen.instance;

        gameState = GameState.MainMenu;
        Camera.main.transform.DOMove(mainMenuCameraPosition, 0.8f).SetEase(Ease.InCubic);
        playButton.Select();
    }


    private void JoystickAddedOrRemoved(JoystickEvent.JoystickStatus status, int index)
    {
        if (status == JoystickEvent.JoystickStatus.JoystickAdded)
        {
            switch (gameState)
            {
                case GameState.MainMenu:
                    break;
                case GameState.PlayersReady:
                    break;
                case GameState.Game:
                    break;
                case GameState.End:
                    break;
            }

            players[index].joystickActive = true;

            //players[index].Activate();
        }
        else if (status == JoystickEvent.JoystickStatus.JoystickRemoved)
        {
            switch (gameState)
            {
                case GameState.MainMenu:
                    break;
                case GameState.PlayersReady:
                    playersScreen.RemovePlayer(index);
                    break;
                case GameState.Game:
                    break;
                case GameState.End:
                    break;
            }

            players[index].joystickActive = false;

            //players[index].Deactivate();
        }
    }





    public void MainMenuPlaySelected()
    {
        EventSystem.current.SetSelectedGameObject(null);

        Camera.main.transform.DOMove(playerMenuCameraPosition, 0.8f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            gameState = GameState.PlayersReady;
        });
    }



    public void MainMenuExitSelected()
    {
        ExhibitUtilities.ExitApplication();
    }






    private void Update()
    {
        for (int i = 1; i <= players.Count; i++)
        {
            if (Input.GetButtonDown("Go " + i.ToString()))
            {
                Debug.Log("Go " + i.ToString() + " Down");

                if (gameState == GameState.PlayersReady)
                {
                    if (playersScreen.PlayerAdded(i-1))
                    {
                        // start game!
                    }
                    else
                    {
                        Debug.Log("Adding Player " + i.ToString());
                        playersScreen.AddPlayer(players[i - 1], i - 1);
                    }
                }
            }
            else if (Input.GetButtonDown("Back " + i.ToString()))
            {
                if (gameState == GameState.PlayersReady)
                {
                    Debug.Log("Removing Player " + i.ToString());
                    playersScreen.RemovePlayer(i-1);
                }
            }
        }
    }
}

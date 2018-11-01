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

    private MultipleTargetCamera cam;
    private JoystickManager joystickManager;
    private PlayerJoinScreen playersScreen;


    

    [Header("Menus")]
    public Vector2 mainMenuCameraPosition;
    public Vector2 playerMenuCameraPosition;
    public Button playButton;

    [Header("Pause Menu")]
    public RectTransform pauseMenu; 
    public Button resumeButton;



    [Header("Game")]
    public List<Player> players;



    




    private void Start()
    {
        cam = MultipleTargetCamera.instance;

        joystickManager = JoystickManager.instance;
        joystickManager.joystickEvent.AddListener(JoystickAddedOrRemoved);

        playersScreen = PlayerJoinScreen.instance;

        pauseMenu.gameObject.SetActive(false);

        gameState = GameState.MainMenu;
        cam.MoveTo(mainMenuCameraPosition);
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

        cam.MoveTo(playerMenuCameraPosition, () => { gameState = GameState.PlayersReady; });
    }



    public void MainMenuExitSelected()
    {
        ExhibitUtilities.ExitApplication();
    }


    public void StartGame()
    {
        gameState = GameState.Game;

        for (int i = 0; i < players.Count; i++)
        {
            if (playersScreen.PlayerAdded(i))
            {
                players[i].gameObject.SetActive(true);
                cam.AddTarget(players[i].transform);
            }
        }

        playersScreen.ShowAdvanceText(true);
        playersScreen.Reset();
    }





    private void Update()
    {
        for (int i = 1; i <= players.Count; i++)
        {
            if (Input.GetButtonDown("Go " + i.ToString()))
            {
                if (gameState == GameState.PlayersReady)
                {
                    if (playersScreen.PlayerAdded(i-1))
                    {
                        if (playersScreen.GetTotalPlayers() > 1)
                        {
                            StartGame();
                        }
                    }
                    else
                    {
                        Debug.Log("Adding Player " + i.ToString());
                        playersScreen.AddPlayer(players[i - 1], i - 1);
                        playersScreen.ShowAdvanceText(true);
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

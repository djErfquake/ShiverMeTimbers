using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public enum ButtonName { Go, Fire, Start, Back }

    public enum GameState { MainMenu, PlayerAddition, Game, End };
    private GameState gameState = GameState.Game;

    private MultipleTargetCamera cam;
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
    private Dictionary<XInputDotNetPure.PlayerIndex, bool> activeJoysticks;



    




    private void Start()
    {
        cam = MultipleTargetCamera.instance;

        activeJoysticks = new Dictionary<XInputDotNetPure.PlayerIndex, bool>();
        for (int i = 0; i < players.Count; i++)
        {
            activeJoysticks.Add(players[i].joystick.playerIndex, false);
            players[i].joystick.joystickEvent.AddListener(JoystickEventHandler);
        }

        playersScreen = PlayerJoinScreen.instance;

        pauseMenu.gameObject.SetActive(false);

        gameState = GameState.MainMenu;
        cam.MoveTo(mainMenuCameraPosition);
        playButton.Select();
    }


    private Player GetPlayerFromPlayerIndex(XInputDotNetPure.PlayerIndex playerIndex)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].joystick.playerIndex == playerIndex) { return players[i]; }
        }
        return null;
    }


    private void JoystickEventHandler(JoystickData e)
    {
        if (e.joystickAction == JoystickData.JoystickAction.Added)
        {
            Debug.Log("added player " + e.playerIndex.ToString());
            activeJoysticks[e.playerIndex] = true;
        }
        else if (e.joystickAction == JoystickData.JoystickAction.Removed)
        {
            Debug.Log("removed player " + e.playerIndex.ToString());
            activeJoysticks[e.playerIndex] = false;
        }
        else if (e.joystickAction == JoystickData.JoystickAction.Button)
        {
            switch (e.button)
            {
                case PlayerJoystick.Buttons.Pause:
                    if (gameState == GameState.PlayerAddition)
                    {
                        Player p = GetPlayerFromPlayerIndex(e.playerIndex);
                        p.joystick.Vibrate();

                        if (!playersScreen.PlayerAdded(p)) { playersScreen.AddPlayer(p); }
                        else  if (playersScreen.GetTotalPlayers() > 1) { StartGame(); }
                    }
                    else if (gameState == GameState.Game)
                    {
                        Pause();
                    }
                    break;
                case PlayerJoystick.Buttons.Yes:
                    if (gameState == GameState.PlayerAddition)
                    {
                        Player p = GetPlayerFromPlayerIndex(e.playerIndex);
                        p.joystick.Vibrate();

                        if (!playersScreen.PlayerAdded(p)) { playersScreen.AddPlayer(p); }
                        else if (playersScreen.GetTotalPlayers() > 1) { StartGame(); }
                    }
                    break;
                case PlayerJoystick.Buttons.No:
                    if (gameState == GameState.PlayerAddition)
                    {
                        Player p = GetPlayerFromPlayerIndex(e.playerIndex);
                        p.joystick.Vibrate();

                        if (playersScreen.PlayerAdded(p)) { playersScreen.RemovePlayer(p); }
                    }
                    break;
            }
        }
    }





    public void MainMenuPlaySelected()
    {
        EventSystem.current.SetSelectedGameObject(null);

        cam.MoveTo(playerMenuCameraPosition, () => { gameState = GameState.PlayerAddition; });
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
            bool playing = playersScreen.PlayerAdded(players[i]);
            players[i].StartGame(playing);

            if (playing)
            {
                cam.AddTarget(players[i].transform);
            }
        }

        playersScreen.ShowAdvanceText(true);
        playersScreen.Reset();
    }



    public void Pause()
    {
        Time.timeScale = (Time.timeScale == 1f) ? 0f : 1f;
        pauseMenu.gameObject.SetActive(Time.timeScale == 0f);
        if (Time.timeScale == 0f) { resumeButton.Select(); }
        for (int i = 0; i < players.Count; i++) { players[i].Pause(Time.timeScale == 0f); }
    }
}

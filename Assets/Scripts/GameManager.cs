using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{

    #region singleton
    // singleton
    public static GameManager instance;
    private void Awake()
    {
        if (instance && instance != this) { Destroy(gameObject); return; }
        instance = this;
    }
    #endregion



    public enum ButtonName { Go, Fire, Start, Back }

    public enum GameState { MainMenu, PlayerAddition, Game, End };
    private GameState gameState = GameState.Game;

    private MultipleTargetCamera cam;
    private PlayerJoinScreen playersScreen;


    

    [Header("Menus")]
    public Vector2 mainMenuCameraPosition;
    public float mainMenuCameraSize;
    public Vector2 playerMenuCameraPosition;
    public Button playButton;

    [Header("Pause Menu")]
    public RectTransform pauseMenu; 
    public Button resumeButton;

    [Header("Winner Menu")]
    public RectTransform winnerMenu;
    public Image winBannerImage;
    public Button winAgainButton;



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
        winnerMenu.gameObject.SetActive(false);

        GoToTitle();
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
            AudioManager.instance.PlaySoundEffect(AudioManager.SoundType.Click);
            Debug.Log("added player " + e.playerIndex.ToString());
            activeJoysticks[e.playerIndex] = true;
        }
        else if (e.joystickAction == JoystickData.JoystickAction.Removed)
        {
            AudioManager.instance.PlaySoundEffect(AudioManager.SoundType.Click);
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
                        AudioManager.instance.PlaySoundEffect(AudioManager.SoundType.Click);

                        Player p = GetPlayerFromPlayerIndex(e.playerIndex);
                        p.joystick.Vibrate();

                        if (!playersScreen.PlayerAdded(p)) { playersScreen.AddPlayer(p); }
                        else if (playersScreen.GetTotalPlayers() > 1) { StartGame(); }
                    }
                    break;
                case PlayerJoystick.Buttons.No:
                    if (gameState == GameState.PlayerAddition)
                    {
                        AudioManager.instance.PlaySoundEffect(AudioManager.SoundType.Click);

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
        AudioManager.instance.PlaySoundEffect(AudioManager.SoundType.Click);

        EventSystem.current.SetSelectedGameObject(null);

        cam.MoveTo(playerMenuCameraPosition, 540, () => { gameState = GameState.PlayerAddition; });
    }



    public void MainMenuExitSelected()
    {
        AudioManager.instance.PlaySoundEffect(AudioManager.SoundType.Click);

        ExhibitUtilities.ExitApplication();
    }


    public void StartGame()
    {
        gameState = GameState.Game;

        AudioManager.instance.PlayThemeMusic();

        for (int i = 0; i < players.Count; i++)
        {
            bool playing = playersScreen.PlayerAdded(players[i]);
            players[i].StartGame(playing);

            if (playing)
            {
                players[i].gameObject.SetActive(true);
                cam.AddTarget(players[i].transform);
            }
            else
            {
                players[i].gameObject.SetActive(false);
            }
        }

        playersScreen.ShowAdvanceText(true);
        playersScreen.Reset();
    }



    public void Pause(bool showMenu = true)
    {
        Time.timeScale = (Time.timeScale == 1f) ? 0f : 1f;
        for (int i = 0; i < players.Count; i++) { players[i].Pause(Time.timeScale == 0f); }

        if (showMenu)
        {
            bool isPaused = Time.timeScale == 0f;

            if (isPaused)
            {
                pauseMenu.gameObject.SetActive(true);
                pauseMenu.localScale = Vector2.zero;
                pauseMenu.DOScale(1, 0.8f).SetEase(Ease.OutElastic);

                resumeButton.Select();
            }
            else
            {
                pauseMenu.localScale = Vector2.one;
                pauseMenu.DOScale(0, 0.4f).SetEase(Ease.InCubic).OnComplete(() =>
                {
                    pauseMenu.gameObject.SetActive(false);
                });
            }
        }
    }

    public void UnPause()
    {
        AudioManager.instance.PlaySoundEffect(AudioManager.SoundType.Click);

        Time.timeScale = 1f;
        for (int i = 0; i < players.Count; i++) { players[i].Pause(false); }

        pauseMenu.localScale = Vector2.one;
        pauseMenu.DOScale(0, 0.4f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            pauseMenu.gameObject.SetActive(false);
        });
    }


    public void ShowWinMenu(Player winningPlayer)
    {
        Pause(false);

        winBannerImage.color = winningPlayer.color;

        winnerMenu.gameObject.SetActive(true);
        winnerMenu.localScale = Vector2.zero;
        winnerMenu.DOScale(1, 0.8f).SetEase(Ease.OutElastic);

        winAgainButton.Select();
    }

    private void HideWinMenu()
    {
        AudioManager.instance.PlaySoundEffect(AudioManager.SoundType.Click);

        winnerMenu.localScale = Vector2.one;
        winnerMenu.DOScale(0, 0.4f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            winnerMenu.gameObject.SetActive(false);
        });
    }


    public void ResetGame()
    {
        HideWinMenu();
        UnPause();

        StartGame();
    }


    public void GoToTitle()
    {
        AudioManager.instance.StopThemeMusic();

        UnPause();

        for (int i = 0; i < players.Count; i++) { players[i].StopGame(); }

        gameState = GameState.MainMenu;
        cam.ClearTargets();
        cam.MoveTo(mainMenuCameraPosition, mainMenuCameraSize);
        playButton.Select();
    }


}

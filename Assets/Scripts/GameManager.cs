using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState { MainMenu, PlayersReady, Game, End };
    private GameState gameState = GameState.Game;


    public List<Player> players;



    private JoystickManager joystickManager;




    private void Start()
    {
        joystickManager = JoystickManager.instance;
        joystickManager.joystickEvent.AddListener(JoystickAddedOrRemoved);
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

            //players[index].Activate();
        }
        else if (status == JoystickEvent.JoystickStatus.JoystickRemoved)
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

            //players[index].Deactivate();
        }
    }





    public void MainMenuPlaySelected()
    {

    }



    public void MainMenuExitSelected()
    {
        ExhibitUtilities.ExitApplication();
    }



}

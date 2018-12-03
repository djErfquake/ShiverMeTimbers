using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XInputDotNetPure;
using DG.Tweening;

public class PlayerJoystick : MonoBehaviour
{
    public enum Buttons
    {
        None, Pause,
        Yes, No,
        Go, Fire,
        Horizontal, Vertical
    }

    public PlayerIndex playerIndex;
    private GamePadState state = new GamePadState();
    private GamePadState preveiousState = new GamePadState();

    public JoystickEvent joystickEvent = new JoystickEvent();

    private Tween vibrationTween;
    private bool vibrating = false;
    private float vibrationAmount = 1f;


    private void Update()
    {
        // update states and send events
        state = GamePad.GetState(playerIndex);

        // adding and removing
        if (state.IsConnected && !preveiousState.IsConnected)
        {
            joystickEvent.Invoke(new JoystickData(JoystickData.JoystickAction.Added, playerIndex, Buttons.None));
        }
        else if (!state.IsConnected && preveiousState.IsConnected)
        {
            joystickEvent.Invoke(new JoystickData(JoystickData.JoystickAction.Removed, playerIndex, Buttons.None));
        }

        // pause, yes, and no buttons
        if (state.Buttons.Start == ButtonState.Pressed && preveiousState.Buttons.Start != ButtonState.Pressed)
        {
            joystickEvent.Invoke(new JoystickData(JoystickData.JoystickAction.Button, playerIndex, Buttons.Pause));
        }

        if (state.Buttons.A == ButtonState.Pressed && preveiousState.Buttons.A != ButtonState.Pressed)
        {
            joystickEvent.Invoke(new JoystickData(JoystickData.JoystickAction.Button, playerIndex, Buttons.Yes));
        }

        if (state.Buttons.B == ButtonState.Pressed && preveiousState.Buttons.B != ButtonState.Pressed)
        {
            joystickEvent.Invoke(new JoystickData(JoystickData.JoystickAction.Button, playerIndex, Buttons.No));
        }

        preveiousState = state;



        // do vibration
        if (vibrating)
        {
            GamePad.SetVibration(playerIndex, vibrationAmount, vibrationAmount);
        }
    }


    public bool GetButtonState(Buttons button)
    {
        bool buttonState = false;

        switch (button)
        {
            case Buttons.Go:
                buttonState = (state.Buttons.A == ButtonState.Pressed || state.Buttons.RightShoulder == ButtonState.Pressed);
                break;
            case Buttons.Fire:
                buttonState = (state.Buttons.X == ButtonState.Pressed || state.Buttons.LeftShoulder == ButtonState.Pressed);
                break;
        }

        return buttonState;
    }


    public float GetJoystickState(Buttons button)
    {
        float joystickState = 0;

        switch (button)
        {
            case Buttons.Horizontal:
                joystickState = state.ThumbSticks.Left.X;
                break;
            case Buttons.Vertical:
                joystickState = state.ThumbSticks.Left.Y;
                break;
        }

        return joystickState;
    }




    public void Vibrate(float duration = 0.5f)
    {
        vibrationAmount = 1f;

        if (vibrationTween != null) { vibrationTween.Kill(); }

        vibrating = true;
        vibrationTween = DOTween.To(() => vibrationAmount, pos => vibrationAmount = pos, 0f, duration).OnComplete(() =>
        {
            vibrating = false;
            GamePad.SetVibration(playerIndex, 0, 0);
        });
    }


    public static int GetPlayerNumber(PlayerIndex index)
    {
        switch (index)
        {
            case PlayerIndex.One:
                return 1;
            case PlayerIndex.Two:
                return 2;
            case PlayerIndex.Three:
                return 3;
            case PlayerIndex.Four:
                return 4;
            default:
                return 0;
        }
    }

}





public class JoystickEvent : UnityEvent<JoystickData> { };

[System.Serializable]
public class JoystickData
{
    public enum JoystickAction { Added, Removed, Button };

    public JoystickAction joystickAction;
    public PlayerIndex playerIndex;
    public PlayerJoystick.Buttons button;

    public JoystickData(JoystickAction _action, PlayerIndex _index, PlayerJoystick.Buttons _button)
    {
        joystickAction = _action;
        playerIndex = _index;
        button = _button;
    }
}


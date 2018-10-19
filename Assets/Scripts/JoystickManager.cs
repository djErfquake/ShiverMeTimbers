using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JoystickManager : MonoBehaviour
{
    
    public JoystickEvent joystickEvent;
    private List<bool> joysticksConnected = new List<bool>();



    private void Update()
    {
        string[] joysticks = Input.GetJoystickNames();

        while (joysticksConnected.Count < joysticks.Length) { joysticksConnected.Add(false); }

        int connectedJoysticks = 0;
        for (int i = 0; i < joysticks.Length; i++)
        {
            int joystickIndex = i;
            bool connected = !string.IsNullOrEmpty(joysticks[i]);
            if (connected) { connectedJoysticks++; }

            if (connected && !joysticksConnected[i])
            {
                Debug.Log("Joystick " + joystickIndex.ToString() + " added.");
                joystickEvent.Invoke(JoystickEvent.JoystickStatus.JoystickAdded, joystickIndex);
            }
            else if (!connected && joysticksConnected[i])
            {
                Debug.Log("Joystick " + joystickIndex.ToString() + " removed.");
                joystickEvent.Invoke(JoystickEvent.JoystickStatus.JoystickRemoved, joystickIndex);
            }

            joysticksConnected[i] = connected;
        }
    }


}

[System.Serializable]
public class JoystickEvent : UnityEvent<JoystickEvent.JoystickStatus, int>
{
    public enum JoystickStatus { JoystickAdded, JoystickRemoved }
}

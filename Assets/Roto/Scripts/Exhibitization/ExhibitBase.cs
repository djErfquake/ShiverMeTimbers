using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[HelpURL("https://code.roto.com/standard-applications/unity-roto-base#exhibitbase")]
[AddComponentMenu("Roto/Exhibit Base")]
public class ExhibitBase : MonoBehaviour
{
    // variables
    [HideInInspector]
    public bool verbose = false;

    // escape key
    public bool exitOnEscapeKey = true;

    // cursor visible on start
    public bool cursorVisibleOnStart = false;


    // inactivity timeout
    public float inactivityTimeoutSeconds = 35f;
    private Coroutine inactivityTimeoutCoroutine;
    public delegate void InactivityTimeoutEvent();
    public InactivityTimeoutEvent OnInactivityTimeout;

    // corner code
    private CornerCodeHelper cornerCodes;




    /// <summary>
    /// hides the cursor upon startup
    /// </summary>
    private void Start()
    {
        Cursor.visible = cursorVisibleOnStart;
    }


    /// <summary>
    /// checks every frame for a few things:
    ///     - if the "ESC" button is pressed, the application exits.
    ///     - if the "`" (Backquote) button is pressed, the cursor toggles visibility
    ///     - mouse presses, which restart the inactivity timer
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            Cursor.visible = !Cursor.visible;
        }

        if (exitOnEscapeKey && Input.GetKeyDown(KeyCode.Escape))
        {
            ExhibitUtilities.ExitApplication();
        }

        if (OnInactivityTimeout != null && Input.GetMouseButtonUp(0))
        {
            RestartInactivityTimer();
            if (cornerCodes != null) { cornerCodes.BringToFront(); }
        }
    }




    /// <summary>
    /// calls the inactivity timeout action if it makes nothing is pressed for a certain amount of time
    /// </summary>
    /// <returns></returns>
    private IEnumerator InactivityTimeout()
    {
        yield return new WaitForSeconds(inactivityTimeoutSeconds);
        if (OnInactivityTimeout != null)
        {
            OnInactivityTimeout.Invoke();
            if (cornerCodes != null) { cornerCodes.BringToFront(); }
        }
    }






    /// <summary>
    /// Adds the Exit Sequence as the top overlay so that when the corners of the screen are pressed in a certain order, the application is exited.
    /// </summary>
    /// <param name="exitCode">the triggering code.  Defaults to the Top-Left, Top-Right, Bottom-Right, Bottom-Left, then Top-Right corners of the screen</param>
    public void AddExitCornerCode(string exitCode = "12432")
    {
        AddCornerCode(exitCode, ExhibitUtilities.ExitApplication);
    }

    /// <summary>
    /// Adds a Code Sequence as the top overlay so that when the corners of the screen are pressed in a certain order, the application performs an Action.
    /// 1 - Top-Left
    /// 2 - Top-Right
    /// 3 - Bottom-Left
    /// 4 - Bottom-Right
    /// </summary>
    /// <param name="newCode">the triggering code</param>
    /// <param name="codeAction">the Action to trigger</param>
    public void AddCornerCode(string newCode, Action codeAction)
    {
        if (cornerCodes == null)
        {
            cornerCodes = gameObject.AddComponent<CornerCodeHelper>();
        }
        cornerCodes.AddCornerCode(newCode, codeAction);
    }


    public void SetCornerCodeButtonColor(Color c)
    {
        if (cornerCodes == null)
        {
            cornerCodes = gameObject.AddComponent<CornerCodeHelper>();
        }
        cornerCodes.SetColor(c);
    }

    public void SetCornerCodeTimeoutSeconds(float ccTimeoutSeconds)
    {
        if (cornerCodes == null)
        {
            cornerCodes = gameObject.AddComponent<CornerCodeHelper>();
        }
        cornerCodes.SetTimeoutSeconds(ccTimeoutSeconds);
    }

    public void SetCornerCodeButtonSize(float ccButtonSize)
    {
        if (cornerCodes == null)
        {
            cornerCodes = gameObject.AddComponent<CornerCodeHelper>();
        }
        cornerCodes.SetButtonSize(ccButtonSize);
    }








    public void RestartInactivityTimer()
    {
        StopInactivityTimer();
        inactivityTimeoutCoroutine = StartCoroutine(InactivityTimeout());
    }

    

    public void StopInactivityTimer()
    {
        if (inactivityTimeoutCoroutine != null)
        {
            StopCoroutine(inactivityTimeoutCoroutine);
            inactivityTimeoutCoroutine = null;
        }
    }


    






    public static Canvas CreateCanvas(float width, float height)
    {
        GameObject canvasGameObject = new GameObject();
        canvasGameObject.name = "Canvas";
        Canvas canvas = canvasGameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler canvasScaler = canvasGameObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(width, height);
        canvasGameObject.AddComponent<GraphicRaycaster>();

        GameObject eventSystemGameObject = new GameObject();
        eventSystemGameObject.name = "EventSystem";
        eventSystemGameObject.AddComponent<EventSystem>();
        eventSystemGameObject.AddComponent<StandaloneInputModule>();

        return canvas;
    }




    // singleton
    public static ExhibitBase instance;
    private void Awake()
    {
        if (instance && instance != this) { Destroy(gameObject); return; }
        instance = this;
    }





}
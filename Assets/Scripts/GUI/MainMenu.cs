using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [Header("MAIN MENU")]
    public float scrW;
    public float scrH;
    public bool optionsMenu;

    [Header("ScreenSize")]
    public int resIndex;
    public int[] resX, resY;
    public bool fullScreen, windowed;

    [Header("Brightness and Audio")]
    public AudioSource menuMusic;
    public Light brightness;
    public float audioSlider, brightnessSlider;
    public float amBrightSlider;
    public bool mute;
    public float Brightness;

    [Header("Inputs")]
    public KeyCode forward;
    public KeyCode backward;
    public KeyCode left;
    public KeyCode right;
    public KeyCode crouch;
    public KeyCode interact;
    public KeyCode jump;
    public KeyCode sprint;
    public KeyCode tempkey;

    // Use this for initialization
    void Start()
    {
        scrW = 16;
        scrH = 9;
        #region References
        brightness = GameObject.FindGameObjectWithTag("Brightness").GetComponent<Light>();
        menuMusic = GameObject.Find("Music").GetComponent<AudioSource>();
        #endregion
        #region Music and Brightness
        if (PlayerPrefs.HasKey("Mute"))
        {
            RenderSettings.ambientIntensity = PlayerPrefs.GetFloat("AmLight");
            brightness.intensity = PlayerPrefs.GetFloat("DirLight");
            amBrightSlider = RenderSettings.ambientIntensity;
            audioSlider = PlayerPrefs.GetFloat("AudioVolume");

            if (PlayerPrefs.GetInt("Mute") == 0)
            {
                mute = true;
                menuMusic.volume = 0;
            }

            else
            {
                mute = false;
                audioSlider = menuMusic.volume;
            }
        }

        else
        {
            audioSlider = menuMusic.volume;
            brightnessSlider = brightness.intensity;
            amBrightSlider = RenderSettings.ambientIntensity;
        }
        #endregion
        #region Key Inputs
        forward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Forward", "W"));
        backward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Backward", "S"));
        left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left", "A"));
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right", "D"));
        crouch = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Crouch", "C"));
        interact = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Interact", "E"));
        jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump", "Space"));
        sprint = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Sprint", "LeftShift"));
        #endregion
    }

    void Update()
    {
        if (Screen.width / 16 != scrW || Screen.height / 9 != scrH)
        {
            scrW = Screen.width / 16;
            scrH = Screen.height / 9;
        }

        if (optionsMenu)
        {
            if (menuMusic.volume != audioSlider)
            {
                menuMusic.volume = audioSlider;
            }

            if (RenderSettings.ambientIntensity != amBrightSlider)
            {
                RenderSettings.ambientIntensity = amBrightSlider;
            }

            if (brightness.intensity != brightnessSlider)
            {
                brightness.intensity = brightnessSlider;
            }


        }
    }

    void OnGUI()
    {
        float scrW = Screen.width / 16;
        float scrH = Screen.height / 9;
        if (!optionsMenu)
        {



            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");

            if (GUI.Button(new Rect(6.75f * scrW, 3.5f * scrH, 2 * scrW, 1 * scrH), "Click Here to Begin"))
            {
                //SceneManager.LoadScene("Level_01");
            }

            if (GUI.Button(new Rect(6.75f * scrW, 5.25f * scrH, 2 * scrW, 1 * scrH), "Click Here to Change Your Settings"))
            {
                optionsMenu = true;
            }

            if (GUI.Button(new Rect(6.75f * scrW, 7 * scrH, 2 * scrW, 1 * scrH), "Click Here to Exit the Game"))
            {
                Application.Quit();

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        }

        else
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height),"");
            
            GUI.Box(new Rect(0.5f * scrW, 0.6f * scrH, 7.125f * scrW, 1f * scrH), "Volume");
            audioSlider = GUI.HorizontalSlider(new Rect(0.5f * scrW, 1f * scrH, 7.125f * scrW, 0.25f * scrH), audioSlider, 0.0f, 1.0f);
            GUI.Label(new Rect(4f * scrW, 1.25f * scrH, 0.75f * scrW, 0.35f * scrH), Mathf.FloorToInt(audioSlider * 100).ToString());

            

        }
    }
}

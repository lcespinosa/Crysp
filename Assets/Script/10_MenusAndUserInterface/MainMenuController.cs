using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

    public int whichMenu = 0;
    public GUISkin menuSkin;
    public string gameDisplayName = "- DEFAULT GAME NAME -";
    public string gamePrefsName = "DefaultGame";
    public string singleGameStartScene;
    public string coopGameStartScene;
    public float default_width = 720;
    public float default_height = 480;
    public float audioSFXSliderValue;
    public float audioMusicSliderValue;
    public float graphicsSliderValue;
    private int detailLevels = 6;

    void Start ( ) {
        // set up default options, if they have been saved out to 
        // prefs already
        if ( PlayerPrefs.HasKey(gamePrefsName + "_SFXVol") ) {
            audioSFXSliderValue = PlayerPrefs.GetFloat(gamePrefsName + "_SFXVol");
        } else {
            audioSFXSliderValue = 1;
        }
        if ( PlayerPrefs.HasKey(gamePrefsName + "_MusicVol") ) {
            audioMusicSliderValue = PlayerPrefs.GetFloat(gamePrefsName + "_MusicVol");
        } else {
            audioMusicSliderValue = 1;
        }
        if ( PlayerPrefs.HasKey(gamePrefsName + "_GraphicsDetail") ) {
            graphicsSliderValue = PlayerPrefs.GetFloat(gamePrefsName + "_GraphicsDetail");
        } else {
            string[ ] names = QualitySettings.names;
            detailLevels = names.Length;
            graphicsSliderValue = detailLevels;
        }
        // set the quality setting
        QualitySettings.SetQualityLevel((int)graphicsSliderValue, true);

        resX = 0;
        resY = 0;
    }

    float resX = 0;
    float resY = 0;
    void OnGUI ( ) {
        resX = Screen.width / default_width;
        resY = Screen.height / default_height;
        GUI.matrix = Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(resX, resY, 1));

        // set the GUI skin to use our custom menu skin
        GUI.skin = menuSkin;

        switch ( whichMenu ) {
            case 0:
                GUI.BeginGroup(new Rect(default_width / 2 - 150, default_height / 2 - 250, 500, 500));
                // All rectangles are now adjusted to the group. 
                // (0,0) is the topleft corner of the group.

                GUI.Label(new Rect(0, 50, 300, 50), gameDisplayName, "textarea");

                if ( GUI.Button(new Rect(0, 200, 300, 40), "START SINGLE", "button") ) {
                    PlayerPrefs.SetInt("totalPlayers", 1);
                    LoadLevel(singleGameStartScene);
                }

                if ( coopGameStartScene != "" ) {
                    if ( GUI.Button(new Rect(0, 250, 300, 40), "START CO-OP") ) {
                        PlayerPrefs.SetInt("totalPlayers", 2);
                        LoadLevel(coopGameStartScene);
                    }

                    if ( GUI.Button(new Rect(0, 300, 300, 40), "OPTIONS") ) {
                        ShowOptionsMenu( );
                    }
                } else {
                    if ( GUI.Button(new Rect(0, 250, 300, 40), "OPTIONS") ) {
                        ShowOptionsMenu( );
                    }
                }
                if ( GUI.Button(new Rect(0, 400, 300, 40), "EXIT") ) {
                    ConfirmExitGame( );
                }
                // End the group we started above. This is very   
                // important to remember!
                GUI.EndGroup( );

                break;
            case 1:
                // Options menu
                GUI.BeginGroup(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 250, 500, 500));
                // Are you sure you want to exit?
                GUI.Label(new Rect(0, 50, 300, 50), "OPTIONS", "textarea");

                if ( GUI.Button(new Rect(0, 250, 300, 40), "AUDIO OPTIONS") ) {
                    ShowAudioOptionsMenu( );
                }

                if ( GUI.Button(new Rect(0, 300, 300, 40), "GRAPHICS OPTIONS") ) {
                    ShowGraphicsOptionsMenu( );
                }

                if ( GUI.Button(new Rect(0, 400, 300, 40), "BACK TO MAIN MENU") ) {
                    GoMainMenu( );
                }

                GUI.EndGroup( );

                break;
            case 2:
                GUI.BeginGroup(new Rect(default_width / 2 - 150,
         default_height / 2 - 250, 500, 500));
                // Are you sure you want to exit?
                GUI.Label(new Rect(0, 50, 300, 50), "Are you sure you want to exit?", "textarea");

                if ( GUI.Button(new Rect(0, 250, 300, 40), "YES, QUIT PLEASE!") ) {
                    ExitGame( );
                }

                if ( GUI.Button(new Rect(0, 300, 300, 40), "NO, DON’T QUIT") ) {
                    GoMainMenu( );
                }
                GUI.EndGroup( );
                break;
            case 3:
                // AUDIO OPTIONS
                GUI.BeginGroup(new Rect(default_width / 2 - 150, default_height / 2 - 250, 500, 500));
                GUI.Label(new Rect(0, 50, 300, 50), "AUDIO OPTIONS", "textarea");

                GUI.Label(new Rect(0, 170, 300, 20), "SFX volume:");
                audioSFXSliderValue = GUI.HorizontalSlider(new Rect(0, 200, 300, 50), audioSFXSliderValue, 0.0f, 1f);
                GUI.Label(new Rect(0, 270, 300, 20), "Music volume:");
                audioMusicSliderValue = GUI.HorizontalSlider(new Rect(0, 300, 300, 50), audioMusicSliderValue, 0.0f, 1f);

                if ( GUI.Button(new Rect(0, 400, 300, 40), "BACK TO OPTIONS MENU") ) {
                    SaveOptionsPrefs( );
                    ShowOptionsMenu( );
                }

                GUI.EndGroup( );
                break;
            case 4:
                // GRAPHICS OPTIONS
                GUI.BeginGroup(new Rect(default_width / 2 - 150, default_height / 2 - 250, 500, 500));
                GUI.Label(new Rect(0, 50, 300, 50), "GRAPHICS OPTIONS", "textarea");

                GUI.Label(new Rect(0, 170, 300, 20), "Graphics quality:");
                graphicsSliderValue = Mathf.RoundToInt(GUI.HorizontalSlider(new Rect(0, 200, 300, 50), graphicsSliderValue, 0, detailLevels));

                if ( GUI.Button(new Rect(0, 400, 300, 40), "BACK TO OPTIONS MENU") ) {
                    SaveOptionsPrefs( );
                    ShowOptionsMenu( );
                }

                GUI.EndGroup( );
                break;
        }
    }

    void LoadLevel ( string whichLevel ) {
        Application.LoadLevel(whichLevel);
    }

    void GoMainMenu ( ) {
        whichMenu = 0;
    }

    void ShowOptionsMenu ( ) {
        whichMenu = 1;
    }

    void ShowAudioOptionsMenu ( ) {
        whichMenu = 3;
    }

    void ShowGraphicsOptionsMenu ( ) {
        whichMenu = 4;
    }

    void SaveOptionsPrefs ( ) {
        PlayerPrefs.SetFloat(gamePrefsName + "_SFXVol", audioSFXSliderValue);
        PlayerPrefs.SetFloat(gamePrefsName + "_MusicVol", audioMusicSliderValue);
        PlayerPrefs.SetFloat(gamePrefsName + "_GraphicsDetail", graphicsSliderValue);

        // set the quality setting
        QualitySettings.SetQualityLevel((int)graphicsSliderValue, true);
    }
    void ConfirmExitGame ( ) {
        whichMenu = 2;
    }
    void ExitGame ( ) {
        // tell level loader to shut down the game for us
        Application.Quit( );
    }

}

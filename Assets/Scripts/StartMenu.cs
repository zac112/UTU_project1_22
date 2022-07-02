using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class StartMenu : MonoBehaviour
{
    public string newGame;
    private string loadGame;
    private float defaultVolume = 100;
    [SerializeField] private GameObject noSavedGame = null;
    [SerializeField] private TMP_Text volumeValue = null;
    [SerializeField] private Slider slider = null;
    [SerializeField] private TMP_InputField serverAddress;
    private int quality;
    public TMP_Dropdown resolution;
    public TMP_Dropdown qualityDrop;
    private Resolution[] resolutions;

    public void Start()
    {
        resolutions = Screen.resolutions;
        resolution.ClearOptions();
        List<string> choices = new List<string>();

        int selectedResolution = 0;

        for (int i = 0; i <resolutions.Length; i++)
        {
            string choice = resolutions[i].width + " x " + resolutions[i].height;
            choices.Add(choice);

            if(resolutions[i].width == Screen.width && resolutions[i].width == Screen.width)
            {
                selectedResolution = i;
            }
        }
        resolution.AddOptions(choices);
        resolution.value = selectedResolution;
        resolution.RefreshShownValue();
    }


    public void NewGame()
    {
        SceneManager.LoadScene(newGame);
        DataManager.instance.NewGame();
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("SavedGame"))
        {
            loadGame = PlayerPrefs.GetString("SavedGame");
            SceneManager.LoadScene(loadGame);
            DataManager.instance.LoadGame();
        }
        else
        {
            noSavedGame.SetActive(true);
        }
    }

    public void Exit()
    {
        Application.Quit();
    }


    // Settings
    public void Quality(int qualityIndex)
    {
        quality = qualityIndex;
    }

    public void Resolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SliderVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeValue.text = volume.ToString("0");
    }

    public void ApplyGraphics()
    {
      PlayerPrefs.SetInt("quality", quality);
      QualitySettings.SetQualityLevel(quality);  
    }

    public void ApplyVolume()
    {
        PlayerPrefs.SetFloat("volume", AudioListener.volume);

    }

    public void Reset(string MenuType)
    {
        if(MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            slider.value = defaultVolume;
            ApplyVolume();
        }

        if(MenuType == "Graphics")
        {
            qualityDrop.value = 1;
            QualitySettings.SetQualityLevel(1);

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolution.value = resolutions.Length;
            ApplyGraphics();
        }
    }

    public void SetPlayModeHost() {
        GameSettings.Instance().networkMode = NetworkMode.Host;
    }
    public void SetPlayModeClient()
    {
        GameSettings.Instance().serverAddress = serverAddress.text;
        GameSettings.Instance().networkMode = NetworkMode.Client;
    }
    public void SetPlayModeOffline()
    {
        GameSettings.Instance().networkMode = NetworkMode.Offline;
        GameSettings.Instance().gameSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
    }
}

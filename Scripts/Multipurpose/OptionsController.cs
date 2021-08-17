using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DifficultyLevels
{
    isEasy,
    isNormal,
    isHard
}

public class OptionsController : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] float defaultVolume = 0.60f;
    [SerializeField] Slider SFXSlider;
    [SerializeField] float defaultSFX = 0.50f;
    [SerializeField] Slider difficultySlider;
    [SerializeField] int defaultDifficulty = 1;

    private DifficultyLevels gameDifficulty = DifficultyLevels.isNormal; //We set the default difficulty as normal
    public DifficultyLevels GameDifficulty => this.gameDifficulty;

    private MusicPlayer musicPlayer;

    
    // Start is called before the first frame update
    void Start()
    {
        this.volumeSlider.value = PlayerPrefsController.GetMasterVolume(this.defaultVolume);
        this.SFXSlider.value = PlayerPrefsController.GetMasterSFX(this.defaultSFX);

        if (this.difficultySlider != null)
            this.difficultySlider.value = PlayerPrefsController.GetDifficultyValue(this.defaultDifficulty);

        this.musicPlayer = GameObject.FindObjectOfType<MusicPlayer>();
    }

    public void SetVolumeFromSliderValue()
    {
        if (this.musicPlayer != null)
        {
            this.musicPlayer.SetVolume(this.volumeSlider.value);
        }
        else
        {
            Debug.LogWarning("No music player found.");
        }
    }

    public void SetSFXFromSliderValue()
    {
        if (this.musicPlayer != null)
        {
            this.musicPlayer.SetSFX(this.SFXSlider.value);
        }
        else
        {
            Debug.LogWarning("No music player found.");
        }
    }

    public void SetDifficultyFromSliderValue()
    {
        if (this.difficultySlider.value == 0)
            this.gameDifficulty = DifficultyLevels.isEasy;
        else if (this.difficultySlider.value == 1)
            this.gameDifficulty = DifficultyLevels.isNormal;
        else if (this.difficultySlider.value == 2)
            this.gameDifficulty = DifficultyLevels.isHard;
    }

    public void SetDefaultValues()
    {
        this.volumeSlider.value = this.defaultVolume;
        this.SFXSlider.value = this.defaultSFX;
        this.difficultySlider.value = this.defaultDifficulty;
    }

    public void SaveAndResumeGame()
    {
        PlayerPrefsController.SetMasterVolume(this.volumeSlider.value);
        PlayerPrefsController.SetMasterSFX(this.SFXSlider.value);

        GameObject.FindObjectOfType<LevelManager>().ResumeGame();
    }

    public void SaveAndExit()
    {
        PlayerPrefsController.SetMasterVolume(this.volumeSlider.value);
        PlayerPrefsController.SetMasterSFX(this.SFXSlider.value);
        PlayerPrefsController.SetDifficultyValue(this.difficultySlider.value);
        GameObject.FindObjectOfType<LevelLoader>().LoadMainMenu();
    }
}

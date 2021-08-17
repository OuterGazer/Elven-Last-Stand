using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] float loadingTime = 2.50f;
    private int currentSceneIndex;

    private MusicPlayer backgroundMusic;

    [SerializeField] GameObject creditsWindow;

    // Start is called before the first frame update
    void Start()
    {
        if(this.creditsWindow != null)
            this.creditsWindow.SetActive(false);

        this.backgroundMusic = GameObject.FindObjectOfType<MusicPlayer>();

        this.currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if(this.currentSceneIndex == 0)
            this.StartCoroutine(this.LoadStartMenu());
    }

    private IEnumerator LoadStartMenu()
    {
        yield return new WaitForSeconds(this.loadingTime);

        this.LoadMainMenu();
    }

    public void LoadMainMenu()
    {
        if (this.backgroundMusic != null && SceneManager.GetActiveScene().buildIndex != 1)
            this.backgroundMusic.PlayMenuMusic();

        SceneManager.LoadScene(2);

        Time.timeScale = 1;
    }

    public void LoadOptionsMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void ReplayLevel()
    {
        SceneManager.LoadScene(this.currentSceneIndex);
        Time.timeScale = 1;
    }

    public void ReplayGameHarder()
    {
        int currentDifficulty = PlayerPrefsController.GetDifficultyValue();

        if(currentDifficulty < 2)
        {
            PlayerPrefsController.SetDifficultyValue(currentDifficulty + 1.0f);
        }

        SceneManager.LoadScene("Level 1");

        Time.timeScale = 1;
    }

    public void LoadNextScene()
    {
        if (this.currentSceneIndex == 2)
            this.backgroundMusic.PlayLevelMusic();

        SceneManager.LoadScene(this.currentSceneIndex + 1);

        Time.timeScale = 1;
    }

    public void ShowAndHideCredits()
    {
        if (!this.creditsWindow.activeSelf)
            this.creditsWindow.SetActive(true);
        else
            this.creditsWindow.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

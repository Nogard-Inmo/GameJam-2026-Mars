using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject howToPlayMenu;
    public GameObject creditsMenu;
    public GameObject startButton;
    public GameObject creditsButton;
    public GameObject howToPlayButton;


    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void HowToPLay()
    {
        howToPlayButton.SetActive(false);
        startButton.SetActive(false);
        creditsButton.SetActive(false);

        howToPlayMenu.SetActive(true);
    }

    public void CloseHowToPlayMenu()
    {
        howToPlayButton.SetActive(true);
        startButton.SetActive(true);
        creditsButton.SetActive(true);

        howToPlayMenu.SetActive(false);
    }

    public void OpenCredits()
    {
        howToPlayButton.SetActive(false);
        startButton.SetActive(false);
        creditsButton.SetActive(false);

        creditsMenu.SetActive(true);
    }
    public void CloseCredits()
    {
        howToPlayButton.SetActive(true);
        startButton.SetActive(true);
        creditsButton.SetActive(true);

        creditsMenu.SetActive(false);
    }
}



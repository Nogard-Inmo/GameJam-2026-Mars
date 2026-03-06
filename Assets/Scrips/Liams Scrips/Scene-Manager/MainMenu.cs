using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject howToPlayMenu;
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void HowToPLay()
    {
        howToPlayMenu.SetActive(true);
    }

    public void CloseHowToPlayMenu()
    {

    }
}



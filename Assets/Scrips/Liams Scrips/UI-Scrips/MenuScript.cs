using UnityEngine;

public class MenuScript : MonoBehaviour
{

    [SerializeField] GameObject menu;

    public void OpenMenu()
    {
        menu.SetActive(true);
    }
}

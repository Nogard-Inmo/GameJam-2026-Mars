using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour, IPlayerTriggerable
{

    //[SerializeField] int sceneToLoad;
    public void OnPLayerTriggered(MovementV2 player)
    {
        SwitchScene();
    }

    IEnumerator SwitchScene()
    {
      yield return SceneManager.LoadSceneAsync("Liam 2");

    }
}

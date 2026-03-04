using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Portal : MonoBehaviour, IPlayerTriggerable
{

    [SerializeField] int sceneToLoad;
    [SerializeField] Transform spawnPoint;

    MovementV2 player;
    public void OnPLayerTriggered(MovementV2 player)
    {
        this.player = player;
        StartCoroutine(SwitchScene());
    }

    IEnumerator SwitchScene()
    {
        DontDestroyOnLoad(gameObject);

        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        var destPortal = FindObjectsOfType<Portal>().First(x => x != this);
         player.transform.position = destPortal.SpawnPoint.position;

        Destroy(gameObject);
    }

    public Transform SpawnPoint => spawnPoint;
}

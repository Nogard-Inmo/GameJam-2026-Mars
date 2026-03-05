using UnityEngine;

public class NpcController : MonoBehaviour, Interacteblels
{
    [SerializeField] Dialog dialog;
    public void Interact()
    {
        DialogManager.Instance.ShowDialog(dialog);
    }
}

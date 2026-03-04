using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] int lettersPerSecond;

    [SerializeField] Text dialogText;
    [SerializeField] GameObject actionSelector; 
    [SerializeField] GameObject abilitysSelector;
    [SerializeField] GameObject abilitysDetails;

    [SerializeField] List<Text> actionTexts;
    [SerializeField] List<Text> abilitysTexts;

    [SerializeField] Text UpText;
    [SerializeField] Text typeTexts;



    public void SetDialog(string dialog)
    {
        dialogText.text = dialog; 
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f/lettersPerSecond);
        }
    }

    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }

    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }

    public void EnableAbilitySelector(bool enabled)
    {
        abilitysSelector.SetActive(enabled);
        abilitysDetails.SetActive(enabled);
    }
}

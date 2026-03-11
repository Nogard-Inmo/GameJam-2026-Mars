using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;
    [SerializeField] Animator animator;

    Monster _monster;

    public void SetData(Monster monster)
    {
        _monster = monster;
        nameText.text = monster.Base.Name;
        levelText.text = "Lvl " + monster.Level;

        hpBar.SetHP((float) monster.HP / monster.MaxHp);


    }

    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float)_monster.HP / _monster.MaxHp);
    }
}
using UnityEngine;
using UnityEngine.UI;


public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

    Monster _monster;

    public void SetData(Monster monster)
    {
        _monster = monster;
        nameText.text = monster.Base.name;
        levelText.text = "Lvl " + monster.level;
        hpBar.SetHP((float)monster.Hp / monster.Base.MaxHp);
    }

    public void UpdateHP()
    {
        if (_monster == null)
        {
            return;
        }

        hpBar.SetHP((float)_monster.Hp / _monster.Base.MaxHp);
    }
}
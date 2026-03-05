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
        float normalized = Mathf.Clamp01((float)monster.Hp / monster.MaxHp);
        hpBar.SetHP(normalized);
    }

    public void UpdateHP()
    {
        if (_monster == null)
        {
            return;
        }

        float normalized = Mathf.Clamp01((float)_monster.Hp / _monster.MaxHp);
        hpBar.SetHP(normalized);
    }
}
using System.Collections;
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
        float normalized = (monster.MaxHp > 0) ? Mathf.Clamp01((float)monster.Hp / monster.MaxHp) : 0f;
        hpBar.SetHP(normalized);
    }

    public IEnumerator UpdateHP()
    {
        if (_monster == null || hpBar == null)
            yield break;

        float target = (_monster.MaxHp > 0) ? Mathf.Clamp01((float)_monster.Hp / _monster.MaxHp) : 0f;

        // If the target is effectively zero, set instantly and exit so callers aren't blocked.
        if (target <= Mathf.Epsilon)
        {
            hpBar.SetHP(0f);
            yield break;
        }

        yield return hpBar.SetHPSmooth(target);
    }
}
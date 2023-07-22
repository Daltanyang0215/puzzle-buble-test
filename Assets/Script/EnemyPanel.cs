using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPanel : MonoBehaviour
{
    public Enemy enemy;
    public Image enemyimage;

    public Slider enemy_HP;
    public int now_hp;
    public Slider AttackPoint;
    public int attack;

    private void Start()
    {
        enemyimage.sprite = enemy.enemy_Image;
        enemy_HP.maxValue = enemy.enemy_HP;
        enemy_HP.value = enemy.enemy_HP;
        now_hp = enemy.enemy_HP;
        StartCoroutine(SlideUpdata());
    }

    public void EnemyUpdata()
    {
        enemyimage.sprite = enemy.enemy_Image;
        enemy_HP.maxValue = enemy.enemy_HP;
        enemy_HP.value = enemy.enemy_HP;
        now_hp = enemy.enemy_HP;
    }

    public bool Attack()
    {
        bool isAttack = false;
        attack++;
        if (attack >= enemy.Attack_Count_MAX)
        {
            HexegonBubleManager.instance.now_Hp -= enemy.enemy_Attack;
            attack = 0;

            if (HexegonBubleManager.instance.now_Hp <= 0)
            {
                HexegonBubleManager.instance.GameOver();
            }

        }
        isAttack = true;
        return isAttack;
    }
    public IEnumerator SlideUpdata()
    {
        WaitForFixedUpdate wt = new WaitForFixedUpdate();

        while (true)
        {
            if (AttackPoint.value != attack)
            {
                AttackPoint.value++;
                if (AttackPoint.value == AttackPoint.maxValue)
                {
                    AttackPoint.value = 0;
                }
            }
            if (enemy_HP.value != now_hp)
            {
                enemy_HP.value = Mathf.Clamp(now_hp, enemy_HP.value - 2, enemy_HP.value + 2);
            }

            yield return wt;
        }
    }

}

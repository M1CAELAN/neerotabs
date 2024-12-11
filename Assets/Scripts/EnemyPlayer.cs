using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyPlayer : MonoBehaviour
{
    public int MaxHp = 100;
    public int CurrentHp;

    void Start()
    {
        CurrentHp = MaxHp;
    }

    public void TakeDamage(int damage)
    {
        CurrentHp -= damage;

        if (CurrentHp <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }
}

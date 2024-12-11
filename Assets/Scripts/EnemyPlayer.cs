using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyPlayer : MonoBehaviour
{
    public float MaxHp = 100;
    public float CurrentHp;
    public event Action<float> HealthChanged;
    public event Action<float> Damage;

    void Start()
    {
        CurrentHp = MaxHp;
    }

    public void TakeDamage(float damage)
    {
        CurrentHp -= damage;

        if (CurrentHp <= 0)
        {
            HealthChanged.Invoke(0);
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Menu");

        }
        else
        {
            float currentHeal = (float)CurrentHp / MaxHp;
            HealthChanged.Invoke(currentHeal);
            Damage.Invoke(1);
        }
            
    }
}

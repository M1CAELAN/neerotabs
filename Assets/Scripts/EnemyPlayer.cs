using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyPlayer : MonoBehaviour
{
    public DethScript dethScript;
    public float MaxHp = 100;
    public float CurrentHp;
    public event Action<float> HealthChanged;
    private bool is_dead = false;


    void Start()
    {
        CurrentHp = MaxHp;
        dethScript = FindObjectOfType<DethScript>();
    }

    public void TakeDamage(float damage)
    {
        CurrentHp -= damage;
        if (CurrentHp <= 0 && is_dead == false)
        {
            is_dead = true;
            HealthChanged.Invoke(0);
            Cursor.lockState = CursorLockMode.None;
            dethScript.gameOver();
        }
        else
        {
            float currentHeal = (float)CurrentHp / MaxHp;
            HealthChanged.Invoke(currentHeal);
        }
            
    }
}

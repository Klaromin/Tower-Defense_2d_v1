using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public static Action<Enemy> OnEnemyKilled;
    public static Action<Enemy> OnEnemyHit; //eventlerimizde bilgi paylaşımı için 

    [SerializeField] private GameObject healthBarPrefab;
    [SerializeField] private Transform barPosition;
    private bool isKilled = false;
    [SerializeField] private float initialHealth = 100f;
    [SerializeField] private float maxHealth = 100f;

    public float CurrentHealth { get; set; }
    
    private Image _healthBar;
    private Enemy _enemy;
    
    private void Start()
    {
        CreateHealthBar();
        CurrentHealth = initialHealth;

        _enemy = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            DealDamage(50f);
        }

        _healthBar.fillAmount = Mathf.Lerp(_healthBar.fillAmount, 
            CurrentHealth / maxHealth, Time.deltaTime * 10f);
    }

    private void CreateHealthBar()
    {
        GameObject newBar = Instantiate(healthBarPrefab, barPosition.position, Quaternion.identity);
        newBar.transform.SetParent(transform);

        EnemyHealthContainer container = newBar.GetComponent<EnemyHealthContainer>();
        _healthBar = container.FillAmountImage;
    }

    public void DealDamage(float damageReceived)
    {
        CurrentHealth -= damageReceived;
        if (CurrentHealth <= 0)
        {
            isKilled = true;
            CurrentHealth = 0;
            Die();
        }
        else
        {
            OnEnemyHit?.Invoke(_enemy);//null değilse invoke et. _enemyi tanımladık çünkü aksi takdirde hasar alınınca bütün enemylerde triggerlanıyor Hurt animasyonu.
        }
    }

    public void ResetHealth()
    {
        if (isKilled)
        {
            ScoreKeeper.Instance.Score++; //Bunu buraya koymamın sebebi eğer öldüğü yere koyarsak kaç adet turret vuruyorsa skor o kadar artıyor.
            isKilled = false;
        }
        
        CurrentHealth = initialHealth;
        _healthBar.fillAmount = 1f;
    }
    
    private void Die()
    {
        
        OnEnemyKilled?.Invoke(_enemy);
    }
}

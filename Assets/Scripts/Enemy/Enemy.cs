using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Action<Enemy> OnEndReached;
    
    [SerializeField] private float moveSpeed = 3f;

    /// <summary>
    /// Move speed of our enemy
    /// </summary>
    public float MoveSpeed { get; set; }
    
    /// <summary>
    /// The waypoint reference
    /// </summary>
    public Waypoint Waypoint { get; set; }

    public EnemyHealth EnemyHealth { get; set; }
    
    /// <summary>
    /// Returns the current Point Position where this enemy needs to go
    /// </summary>
    public Vector3 CurrentPointPosition => Waypoint.GetWaypointPosition(_currentWaypointIndex);
    
    private int _currentWaypointIndex;
    private Vector3 _lastPointPosition;
    
    private EnemyHealth _enemyHealth;
    private SpriteRenderer _spriteRenderer;
    
    private void Start()
    {
        _enemyHealth = GetComponent<EnemyHealth>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        EnemyHealth = GetComponent<EnemyHealth>();
        
        _currentWaypointIndex = 0;
        MoveSpeed = moveSpeed;
        _lastPointPosition = transform.position;
    }

    private void Update()
    {
        Move();
        Rotate();
        
        if (CurrentPointPositionReached()) //true dönerse/ vardıysa arttır ki yeni nokta belirlensin oraya yönelsin.
        {
            UpdateCurrentPointIndex();
        }
        
    }
    

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, 
            CurrentPointPosition, MoveSpeed * Time.deltaTime);
    }

    public void StopMovement()
    {
        MoveSpeed = 0f;
    }

    public void ResumeMovement()
    {
        MoveSpeed = moveSpeed;
    }

    private void Rotate()
    {
        if (CurrentPointPosition.x > _lastPointPosition.x)
        {
            _spriteRenderer.flipX = false;
        }
        else
        {
            _spriteRenderer.flipX = true;
        }
    }
    
    private bool CurrentPointPositionReached()
    {
        float distanceToNextPointPosition = (transform.position - CurrentPointPosition).magnitude;  //enemynin bulunduğu pozisyon ile hedef waypoint noktası arasındaki uzaklığın büyüklüğü
        if (distanceToNextPointPosition < 0.1f) // eğer 0.1ften küçük olursa (en basitinden vardıysa)
        {
            _lastPointPosition = transform.position;
            return true;
        }

        return false;
    }

    private void UpdateCurrentPointIndex()
    {
        int lastWaypointIndex = Waypoint.Points.Length - 1;
        if (_currentWaypointIndex < lastWaypointIndex)
        {
            _currentWaypointIndex++;
        }
        else
        {
            EndPointReached();
        }
    }

    private void EndPointReached()
    {
        OnEndReached?.Invoke(this);
        _enemyHealth.ResetHealth();
        ObjectPooler.ReturnToPool(gameObject);
        ScoreKeeper.Instance.Life--;
    }

    public void ResetEnemy()
    {
        _currentWaypointIndex = 0;
    }
}

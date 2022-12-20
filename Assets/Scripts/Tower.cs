using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private Enemy _targetEnemy;
    private float _lookforTargetTimer;
    private float _lookforTargetTimerMax = 0.2f;
    private float _shootTimer;
    [SerializeField] private float _shootTimerMax = 1.0f;
    private Vector3 _spawnPos;

    private void Awake()
    {
        _spawnPos = transform.Find("projectileSpawnPos").position;
    }

    private void Update()
    {
        HandleTargeting();
        HandleShooting();

    }

    private void HandleShooting()
    {
        _shootTimer -= Time.deltaTime;
        if (_shootTimer <= 0f)
        {
            _shootTimer += _shootTimerMax;

            if (_targetEnemy != null)
            {
                ArrowProjectile.Create(_spawnPos, _targetEnemy);
            }
        }
    }

    private void HandleTargeting()
    {
        _lookforTargetTimer -= Time.deltaTime;
        if (_lookforTargetTimer <= 0f)
        {
            _lookforTargetTimer += _lookforTargetTimerMax;
            LookForTargets();
        }
    }
    private void LookForTargets()
    {
        float targetMaxRadius = 20f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach (Collider2D collider in collider2DArray)
        {
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                //Is an enemy
                if (_targetEnemy == null)
                {
                    _targetEnemy = enemy;
                }

                else
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, _targetEnemy.transform.position))
                    {
                        _targetEnemy = enemy;
                    }
                }

            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy Create (Vector3 position)
    {
        Transform enemyTransform = Instantiate(GameAssets.Instance.pfEnemy, position, Quaternion.identity);

        Enemy enemy = enemyTransform.GetComponent<Enemy>();
        return enemy;
    }

    private Transform _targetTransform;
    private Rigidbody2D _rb2D;
    private HealthSystem _healthSystem;
    private float _lookforTargetTimer;
    private float _lookforTargetTimerMax = 0.2f;

    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        if (BuildingManager.Instance.getHQBuilding() != null)
        {
            _targetTransform = BuildingManager.Instance.getHQBuilding().transform;
        }

        _healthSystem = GetComponent<HealthSystem>();

        _healthSystem.OnDied += _healthSystem_OnDied;
        _healthSystem.OnDamaged += _healthSystem_OnDamaged;

        //Add some randomness so each enemy is not searching at the same time in a given frame
        _lookforTargetTimer = Random.Range(0f, _lookforTargetTimerMax);
    }

    private void _healthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyHit);
        CinemachineShake.Instance.ShakeCamera(5f, 0.1f);
    }

    private void _healthSystem_OnDied(object sender, System.EventArgs e)
    {
        SoundManager.Instance.PlaySound(SoundManager.Sound.EnemyDie);
        CinemachineShake.Instance.ShakeCamera(7f, 0.15f);
        Instantiate(GameAssets.Instance.pfEnemyDieParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Update()
    {
        HandleMovement();
        HandleTargeting();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Building building = collision.gameObject.GetComponent<Building>();
        if (building != null)
        {
            //Collided with a building
            HealthSystem healthSystem = building.GetComponent<HealthSystem>();
            healthSystem.Damage(10);
            _healthSystem.Damage(999); // Kill enemy, if destory then particles will not play
        }
    }

    private void HandleMovement ()
    {
        if (_targetTransform != null)
        {
            Vector3 moveDirection = (_targetTransform.position - transform.position).normalized;
            float moveSpeed = 6f;
            _rb2D.velocity = moveDirection * moveSpeed;
        }
        else
        {
            _rb2D.velocity = Vector3.zero;
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
        float targetMaxRadius = 10f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(transform.position, targetMaxRadius);

        foreach (Collider2D collider in collider2DArray)
        {
            Building building = collider.gameObject.GetComponent<Building>();
            if (building != null)
            {
                //Is a building
                if (_targetTransform == null)
                {
                    _targetTransform = building.transform;
                }

                else
                {
                    if (Vector3.Distance(transform.position, building.transform.position) < Vector3.Distance(transform.position, _targetTransform.position))
                    {
                        _targetTransform = building.transform;
                    }
                }

            }
        }

        if (_targetTransform == null)
        {
            //Found no targets witih range
            if (BuildingManager.Instance.getHQBuilding() != null)  //HQ was not destroyed
            {
                _targetTransform = BuildingManager.Instance.getHQBuilding().transform;
            }
        }
    }
}

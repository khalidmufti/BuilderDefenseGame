using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    public static ArrowProjectile Create(Vector3 position, Enemy enemy)
    {
        Transform pfArrowProjectile = Resources.Load<Transform>(nameof(pfArrowProjectile));
        Transform arrowProjectileTransform = Instantiate(pfArrowProjectile, position, Quaternion.identity);

        ArrowProjectile arrowProjectile = arrowProjectileTransform.GetComponent<ArrowProjectile>();
        arrowProjectile.SetTarget(enemy);
        
        return arrowProjectile;
    }
    
    private Enemy _targetEnemy;
    private Vector3 _lastMoveDir;
    private float _timeToDie = 2f;


    private void Update()
    {
        Vector3 moveDir = _lastMoveDir;
        if (_targetEnemy != null)
        {
            moveDir = (_targetEnemy.transform.position - this.transform.position).normalized;
            _lastMoveDir = moveDir;
        }

        float moveSpeed = 20f;
        transform.position += moveDir * Time.deltaTime * moveSpeed;

        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(moveDir));

        _timeToDie -= Time.deltaTime;
        if (_timeToDie < 0f)
        {
            Destroy(gameObject);
        }
    }

    private void SetTarget (Enemy enemyTarget)
    {
        _targetEnemy = enemyTarget;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy !=null )
        {
            //Hit enemy
            int damageAmount = 10;
            enemy.GetComponent<HealthSystem>().Damage(damageAmount);

            Destroy(gameObject);
            _targetEnemy = null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyWaveUI : MonoBehaviour
{
    [SerializeField] private EnemyWaveManager _enemyWaveManager;

    private TextMeshProUGUI _waveNumberText;
    private TextMeshProUGUI _waveMessageText;
    private RectTransform _spawnIndicatorRectTransform;
    private RectTransform _closestEnemyIndicatorRectTransform;
    private Camera _camera;

    private void Awake()
    {
        _waveNumberText = transform.Find("waveNumberText").GetComponent<TextMeshProUGUI>();
        _waveMessageText = transform.Find("waveMessageText").GetComponent<TextMeshProUGUI>();
        _spawnIndicatorRectTransform = transform.Find("enemyWaveSpawnIndicator").GetComponent<RectTransform>();
        _closestEnemyIndicatorRectTransform = transform.Find("enemyClosestIndicator").GetComponent<RectTransform>();
    }

    private void Start()
    {
        _enemyWaveManager.OnWaveNumberChanged += _enemyWaveManager_OnWaveNumberChanged;
        SetWaveNumberText($"Wave {_enemyWaveManager.GetWaveNumber()}");
        _camera = Camera.main;
    }

    private void _enemyWaveManager_OnWaveNumberChanged(object sender, System.EventArgs e)
    {
        SetWaveNumberText($"Wave {_enemyWaveManager.GetWaveNumber()}");
    }

    private void Update()
    {
        HandleNextWaveMessage();
        HandleEnemyWaveSpawnPositionIndicator();
        HandleEnemyClosestPositionIndicator();
    }

    private void HandleNextWaveMessage()
    {
        float nextWaveSpawnTimer = _enemyWaveManager.GetTimeToNextWave();
        if (nextWaveSpawnTimer <= 0f)
        {
            //Wave is already spawned so hide message
            SetWaveMessageText("");
        }
        else
        {
            SetWaveMessageText($"Next Wave in {nextWaveSpawnTimer:F1}s");
        }
    }

    private void HandleEnemyWaveSpawnPositionIndicator()
    {
        //Indicator where spawn is coming from
        Vector3 spawnDir = (_enemyWaveManager.GetSpawnPosition() - _camera.transform.position).normalized;
        _spawnIndicatorRectTransform.anchoredPosition = spawnDir * 300f;
        _spawnIndicatorRectTransform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(spawnDir));

        //Only show arrow to next spawn if camera is not in spawn circle area
        float distanceToNextSpawnPos = Vector3.Distance(_enemyWaveManager.GetSpawnPosition(), _camera.transform.position);
        _spawnIndicatorRectTransform.gameObject.SetActive(distanceToNextSpawnPos > _camera.orthographicSize * 1.5f);
    }

    private void HandleEnemyClosestPositionIndicator ()
    {
        Enemy closestEnemy = LookForClosestEnemy();
        if (closestEnemy != null)
        {
            Vector3 closestEnemyDir = (closestEnemy.transform.position - _camera.transform.position).normalized;
            _closestEnemyIndicatorRectTransform.anchoredPosition = closestEnemyDir * 250f;
            _closestEnemyIndicatorRectTransform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVector(closestEnemyDir));

            //Only show arrow to next spawn if camera is not in spawn circle area
            float distanceToClosestEnemy = Vector3.Distance(closestEnemy.transform.position, _camera.transform.position);
            _closestEnemyIndicatorRectTransform.gameObject.SetActive(distanceToClosestEnemy > _camera.orthographicSize * 1.5f);
        }
        else
        {
            _closestEnemyIndicatorRectTransform.gameObject.SetActive(false);
        }
    }

    private void SetWaveMessageText (string message)
    {
        _waveMessageText.text = message; 
    }

    private void SetWaveNumberText (string text)
    {
        _waveNumberText.text = text;
    }

    private Enemy LookForClosestEnemy()
    {
        float targetMaxRadius = 9999f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(_camera.transform.position, targetMaxRadius);

        Enemy targetEnemy = null;
        foreach (Collider2D collider in collider2DArray)
        {
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                //Is an enemy
                if (targetEnemy == null)
                {
                    targetEnemy = enemy;
                }

                else
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) < Vector3.Distance(transform.position, targetEnemy.transform.position))
                    {
                        targetEnemy = enemy;
                    }
                }
            }
        }

        return targetEnemy;
    }
}

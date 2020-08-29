using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Player _player = null;
    [SerializeField] private Enemy _enemy = null;
    [SerializeField] private GameObject _enemyContainer = null;
    //private readonly float _spawnRate = 1.5f;
    //private float _timestampLastSpawn = 0.0f;
    private readonly WaitForSeconds _delay = new WaitForSeconds(1.5f);

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    //void Update()
    //{
    //    if (Time.time > _timestampLastSpawn + _spawnRate)
    //    {
    //        Instantiate(_enemy);
    //        _timestampLastSpawn = Time.time;
    //    }
    //}

    private IEnumerator SpawnEnemies()
    {
        while (null != _player && _player.IsAlive())
        {
            Enemy enemy = Instantiate(_enemy);
            enemy.transform.parent = _enemyContainer.transform;
            yield return _delay;
        }
    }

}

using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Player _player = null;
    [SerializeField] private Enemy _enemy = null;
    [SerializeField] private GameObject _enemyContainer = null;
    [SerializeField] private GameObject[] _powerUps = null;
    private readonly WaitForSeconds _delayEnemy = new WaitForSeconds(1.5f);
    private readonly WaitForSeconds _delayPowerUp = new WaitForSeconds(10.0f);

    public void StartSpawning()
    {
        _ = StartCoroutine(SpawnEnemies());
        _ = StartCoroutine(SpawnPowerUp());
    }

    private IEnumerator SpawnEnemies()
    {
        while (null != _player && _player.IsAlive())
        {
            Enemy enemy = Instantiate(_enemy);
            enemy.transform.parent = _enemyContainer.transform;
            yield return _delayEnemy;
        }
    }

    private IEnumerator SpawnPowerUp()
    {
        while (null != _player && _player.IsAlive())
        {
            yield return _delayPowerUp;
            int randomPowerUpId = Random.Range(0, 3);
            GameObject powerUp = _powerUps[randomPowerUpId];
            if (null != powerUp)
            {
                _ = Instantiate(powerUp);
            }
        }
    }

}

using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] Player player1 = default;
    [SerializeField] Player player2 = default;
    [SerializeField] Enemy _enemy = default;
    [SerializeField] GameObject _enemyContainer = default;
    [SerializeField] GameObject[] _powerUps = default;
    readonly WaitForSeconds _delayEnemy = new WaitForSeconds(3.5f);
    readonly WaitForSeconds _delayPowerUp = new WaitForSeconds(10.0f);

    public void StartSpawning()
    {
        _ = StartCoroutine(SpawnEnemies());
        _ = StartCoroutine(SpawnPowerUp());
    }

    IEnumerator SpawnEnemies()
    {
        while (null != player1 && player1.IsAlive() || null != player2 && player2.IsAlive())
        {
            Enemy enemy = Instantiate(_enemy);
            enemy.transform.parent = _enemyContainer.transform;
            yield return _delayEnemy;
        }
    }

    IEnumerator SpawnPowerUp()
    {
        while (null != player1 && player1.IsAlive() || null != player2 && player2.IsAlive())
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

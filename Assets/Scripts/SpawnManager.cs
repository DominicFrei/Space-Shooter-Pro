using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] Player player1 = null;
    [SerializeField] Player player2 = null;
    [SerializeField] Enemy _enemy = null;
    [SerializeField] GameObject _enemyContainer = null;
    [SerializeField] GameObject[] _powerUps = null;
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

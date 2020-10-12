using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Player _player = null;
    [SerializeField] private float _speed = 4.0f;
    private readonly float _boundsUpper = 6.2f;
    private readonly float _boundsLower = -6.4f;
    [SerializeField] private Animator _animator = null;
    [SerializeField] private AudioSource _explosionAudio = null;
    [SerializeField] private AudioClip _explosionAudioClip = null;

    private void Start()
    {
        float newX = Random.Range(-8.0f, 8.0f);
        transform.position = new Vector3(newX, _boundsUpper, 0);
        _animator = GetComponent<Animator>();
    }

    void Update()
    {

        if (null == _player || null == _player.gameObject)
        {
            Destroy(this.gameObject);
            return;
        }

        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < _boundsLower)
        {
            float newX = Random.Range(-8.0f, 8.0f);
            transform.position = new Vector3(newX, _boundsUpper, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.gameObject.transform.GetComponent<Player>();
            if (null != player)
            {
                player.Damage();
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0.0f;
            if (null != _explosionAudio)
            {
                _explosionAudio.clip = _explosionAudioClip;
                _explosionAudio.Play();
            }
            Destroy(gameObject, 2.8f);

        }
        else if (other.CompareTag("Laser"))
        {
            if (null != _player)
            {
                _player.EnemyKilled();
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0.0f;
            if (null != _explosionAudio)
            {
                _explosionAudio.clip = _explosionAudioClip;
                _explosionAudio.Play();
            }
            Destroy(other.gameObject);
            Destroy(gameObject, 2.8f);
        }
    }

}

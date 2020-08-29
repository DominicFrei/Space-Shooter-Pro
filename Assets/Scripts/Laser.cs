using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private readonly float _speed = 8.0f;
    private readonly float _boundsUpper = 8.0f;

    void Update()
    {
        transform.Translate(Vector3.up * Time.deltaTime * _speed);
        //transform.Translate(new Vector3(0, _speed * Time.deltaTime, 0));

        if (transform.position.y > _boundsUpper)
        {
            if (null != transform.parent)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}

using UnityEngine;

public class Laser : MonoBehaviour
{
    public bool ShotByPlayer
    {
        get
        {
            return shotByPlayer;
        }
    }

    Vector3 direction = default;
    bool shotByPlayer = default;
    readonly float speed = 8.0f;
    readonly float upperBounds = 8.0f;

    static internal Laser Init(Laser prefab, Vector3 position, Quaternion rotation, Vector3 direction, bool shotByPlayer)
    {
        Laser laser = Instantiate(prefab, position, rotation);
        laser.direction = direction;
        laser.shotByPlayer = shotByPlayer;
        return laser;
    }

    void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed);

        if (shotByPlayer)
        {
            if (transform.position.y > upperBounds)
            {
                DestroySelf();
            }
        }
        else
        {
            if (transform.position.y < -upperBounds)
            {
                DestroySelf();
            }
        }
    }

    void DestroySelf()
    {
        if (null != transform.parent)
        {
            // This happens in case we fire a triple shot where multiple laser objects are withing a parent object.
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

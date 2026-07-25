using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 100f;
    [SerializeField] private Vector3 minBounds = new(-100f, -100f, -100f);
    [SerializeField] private Vector3 maxBounds = new(100f, 100f, 100f);

    private void Update()
    {
        transform.position += speed * Time.deltaTime * transform.forward;

        // TODO use box collider
        if (transform.position.x < minBounds.x || transform.position.x > maxBounds.x ||
            transform.position.y < minBounds.y || transform.position.y > maxBounds.y ||
            transform.position.z < minBounds.z || transform.position.z > maxBounds.z)
        {
            Destroy(gameObject);
        }
    }
}

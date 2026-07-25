using UnityEngine;
using UnityEngine.Assertions;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 100f;
    [SerializeField] private string bulletBoundsLayer = "Bullet Bounds";

    private void Update()
    {
        transform.position += speed * Time.deltaTime * transform.forward;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(bulletBoundsLayer))
            Despawn();
    }

    private void Despawn()
    {
        // TODO animation
        // TODO sfx
        Destroy(gameObject);
    }
}

using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 100f;
    [SerializeField] private string bulletBoundsLayer = "Bullet Bounds";
    [SerializeField] private string playerBodyLayer = "Player Body";
    [SerializeField] private string playerHeadLayer = "Player Head";
    public PlayerController owner;

    public Vector3 direction;

    private void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player == null || player == owner)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer(playerBodyLayer))
        {
            player.GetHit();
            Despawn();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer(playerHeadLayer))
        {
            player.GetCritHit();
            Despawn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(bulletBoundsLayer))
        {
            // TODO miss sfx
            Despawn();
        }
    }

    public void Despawn()
    {
        // TODO vfx
        Destroy(gameObject);
    }
}

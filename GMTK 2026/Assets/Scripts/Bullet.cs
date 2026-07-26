using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Audio;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 100f;
    [SerializeField] private string bulletBoundsLayer = "Bullet Bounds";
    [SerializeField] private string playerBodyLayer = "Player Body";
    [SerializeField] private string playerHeadLayer = "Player Head";
    [SerializeField] private ClipGroup missSFX;

    public PlayerController owner;

    public Vector3 direction;

    private bool dead = false;

    private void Awake()
    {
        Assert.IsNotNull(missSFX);
    }

    private void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (dead)
            return;

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
            GameObject go = new("Miss SFX");
            AudioSource audioSource = go.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.clip = missSFX.Poll();
            audioSource.Play();
            go.AddComponent<TempObject>().lifetime = audioSource.clip.length;

            Despawn();
        }
    }

    public void Despawn()
    {
        // TODO vfx
        Destroy(gameObject);
        dead = true;
    }
}

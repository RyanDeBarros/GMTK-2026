using UnityEngine;

public class TempObject : MonoBehaviour
{
    public float lifetime;

    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
            Destroy(gameObject);
    }
}

using UnityEngine;

public class UILoop : MonoBehaviour
{
    public float loopSpeed = 5f;
    public float loopRadius = 15f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {
        float x = Mathf.Cos(Time.time * loopSpeed) * loopRadius;
        float y = Mathf.Sin(Time.time * loopSpeed) * loopRadius;
        transform.localPosition = startPosition + new Vector3(x, y, 0);
    }
}

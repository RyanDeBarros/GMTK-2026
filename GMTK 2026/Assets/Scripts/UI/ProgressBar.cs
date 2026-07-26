using UnityEngine;
using UnityEngine.Assertions;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private RectTransform fill;

    private void Awake()
    {
        Assert.IsNotNull(fill);
    }

    public void SetValue(float alpha)
    {
        fill.localScale = new(alpha, 1f);
    }
}

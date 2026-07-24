using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class CountdownDisplay : MonoBehaviour
{
    private static CountdownDisplay instance;
    public static CountdownDisplay Instance => instance;

    private TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        Assert.IsNotNull(textMesh);
    }

    private void OnEnable()
    {
        instance = this;
    }

    private void OnDisable()
    {
        instance = null;
    }

    public void SetCountdownValue(int value)
    {
        textMesh.text = value.ToString();
    }
}

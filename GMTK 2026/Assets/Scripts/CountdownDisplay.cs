using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class CountdownDisplay : MonoBehaviour
{
    private static CountdownDisplay instance;
    public static CountdownDisplay Instance => instance;

    private TextMeshProUGUI textMesh;

    private void Awake()
    {
        Assert.IsNull(instance);

        textMesh = GetComponent<TextMeshProUGUI>();
        Assert.IsNotNull(textMesh);
    }

    private void OnEnable()
    {
        Assert.IsNull(instance);
        instance = this;
    }

    private void OnDisable()
    {
        Assert.IsTrue(instance == this);
        instance = null;
    }

    public void SetCountdownValue(CountdownValue value)
    {
        textMesh.text = value switch {
            CountdownValue.Ten => "10",
            CountdownValue.Nine => "9",
            CountdownValue.Eight => "8",
            CountdownValue.Seven => "7",
            CountdownValue.Six => "6",
            CountdownValue.Five => "5",
            CountdownValue.Four => "4",
            CountdownValue.Three => "3",
            CountdownValue.Two => "2",
            CountdownValue.One => "1",
            CountdownValue.Zero => "Go!",

            CountdownValue.OneHalf => "1/2",
            CountdownValue.OneThird => "1/3",
            CountdownValue.OneFourth => "1/4",
            _ => ""
        };
    }
}

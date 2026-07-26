using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class CountdownDisplay : MonoBehaviour
{
    private static CountdownDisplay instance;
    public static CountdownDisplay Instance => instance;

    [SerializeField] private Image textBg;
    [SerializeField] private TextMeshProUGUI textMesh;

    private void Awake()
    {
        Assert.IsNotNull(textBg);
        Assert.IsNotNull(textMesh);
    }

    private void Start()
    {
        BaseCountdownTimer.Instance.tick.AddListener(OnTick);
    }

    private void Update()
    {
        bool showHUD = MatchManager.Instance.Phase == MatchPhase.Countdown || MatchManager.Instance.Phase == MatchPhase.ChooseAction;
        textBg.enabled = showHUD; // TODO animate textBg
        textMesh.enabled = showHUD;
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
    }

    private void OnTick()
    {
        textMesh.text = BaseCountdownTimer.Instance.GetCountdownValue() switch
        {
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

        textBg.GetComponent<RectTransform>().rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
    }
}

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CountDownUI : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private TMP_Text _label;
    [SerializeField]
    private Animator _textAnimator;

    public UnityEvent OnFinishCountDown { get; private set; }

    private void Awake()
    {
        OnFinishCountDown = new UnityEvent();
    }

    public void StartCountDown(int number)
    {
        StartCoroutine(StartCounting(number));
    }

    private IEnumerator StartCounting(int number)
    {
        int count = number;

        while (count > 0)
        {
            UpdateUI(count);
            yield return new WaitForSeconds(1);
            count--;
        }

        OnFinishCountDown.Invoke();
    }

    private void UpdateUI(int number)
    {
        _textAnimator.Play(TextAnimation.Pulse.ToString());
        _label.text = number.ToString().PadLeft(2, '0');
    }

    private enum TextAnimation
    {
        Pulse
    }
}

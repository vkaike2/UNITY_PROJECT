using System.Collections;
using TMPro;
using UnityEngine;

public class DpsMeterUI : MonoBehaviour
{
    [Header("COMPONENTS")]
    [SerializeField]
    private TMP_Text _label;

    [Header("CONFIGURATIONS")]
    [SerializeField]
    private float _cdwToReset;

    private float _curretDps;
    private float _internalCdw;
    private Coroutine _resetCounterCorroutine;

    public void AddDamage(float damage)
    {
        _curretDps += damage;
        UpdateLabel();

        _internalCdw = 0;

        if(_resetCounterCorroutine == null)
        {
            _resetCounterCorroutine = StartCoroutine(ResetCounter());
        }
    }

    private void UpdateLabel()
    {
        _label.text = _curretDps.ToString("F2");
    }

    private IEnumerator ResetCounter()
    {
        while (_internalCdw <= _cdwToReset)
        {
            _internalCdw += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        _curretDps = 0;
        UpdateLabel();
        _resetCounterCorroutine = null;
    }
}

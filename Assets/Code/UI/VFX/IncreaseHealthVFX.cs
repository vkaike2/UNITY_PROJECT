using TMPro;
using UnityEngine;

public class IncreaseHealthVFX : MonoBehaviour
{

    [Header("COMPONENTS")]
    [SerializeField]
    private TMP_Text _label;

    public void AddHealth(float number)
    {
        string textNumber = $"+{number.ToString()} HP";

        _label.text = textNumber;
    }
}

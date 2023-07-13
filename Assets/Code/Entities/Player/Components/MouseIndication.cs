using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseIndication : MonoBehaviour
{
    [Header("components")]
    [SerializeField]
    private Transform _mouseIndication;


    private void FixedUpdate()
    {
        Vector3 mousePosition = Input.mousePosition;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePosition.x -= objectPos.x;
        mousePosition.y -= objectPos.y;

        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        Quaternion mouseRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        _mouseIndication.rotation = mouseRotation;

        _mouseIndication.localScale = new Vector3(transform.localScale.x * -1f, 1, 1);
    }
}

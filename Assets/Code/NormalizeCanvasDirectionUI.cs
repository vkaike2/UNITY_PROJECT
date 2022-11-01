using System.Collections;
using UnityEngine;

namespace Assets.Code
{
    public class NormalizeCanvasDirectionUI : MonoBehaviour
    {
        [Header("components")]
        [SerializeField]
        private Transform _reference;


        private void FixedUpdate()
        {
            this.transform.localScale = _reference.localScale;
        }
    }
}
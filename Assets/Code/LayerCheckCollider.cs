using System.Collections;
using System.Drawing;
using UnityEngine;

namespace Assets.Code.LOGIC
{
    public class LayerCheckCollider : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _layerMask;

        [Space]
        [SerializeField]
        private float _horizontalRaycastSize = 0.5f;

        public bool IsCollidingWithLayer => CheckCollision();

        public bool test;

        private void OnDrawGizmos()
        {
            Gizmos.color = UnityEngine.Color.red;
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - _horizontalRaycastSize));
        }

        private bool CheckCollision()
        {
            RaycastHit2D col = Physics2D.Linecast(transform.position, new Vector2(transform.position.x, transform.position.y - _horizontalRaycastSize), _layerMask);

            return col.collider != null;
        }
    }
}
using UnityEngine;

namespace RaycastControllerCore
{
    public struct RaycastOrigins
    {
        public Vector2 topLeft;
        public Vector2 topRight;
        public Vector2 bottomLeft;
        public Vector2 bottomRight;
    }
    
    [RequireComponent(typeof(BoxCollider2D))]
    public class Player : MonoBehaviour
    {
        private const float SkinWidth = 0.015f;

        public int horizontalRayCount = 4;
        public int verticalRayCount = 4;

        private float horizontalRaySpacing;
        private float verticalRaySpacing;
        
        private BoxCollider2D _collider2D;
        private RaycastOrigins _raycastOrigins;


        private void CalculateRaySpacing()
        {
            var bounds = _collider2D.bounds;
            bounds.Expand(SkinWidth * -2);

            horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
            verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

            horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
            verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        }

        private void UpdateRaycastOrigins()
        {
            var bounds = _collider2D.bounds;
            bounds.Expand(SkinWidth * -2);

            _raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
            _raycastOrigins.bottomRight = new Vector2(bounds.max.y, bounds.min.y);
            _raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
            _raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        }
        
        private void Start()
        {
            _collider2D = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            UpdateRaycastOrigins();
            CalculateRaySpacing();

            for (var i = 0; i < verticalRayCount; i++)
            {
               Debug.DrawRay(
                   _raycastOrigins.bottomLeft + Vector2.right * (verticalRaySpacing * i),
                   Vector2.up * -2, Color.red); 
            }
        }
    }
}
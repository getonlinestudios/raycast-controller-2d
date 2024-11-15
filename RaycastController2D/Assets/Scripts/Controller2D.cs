using UnityEngine;

namespace RaycastControllerCore
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Controller2D : MonoBehaviour
    {
        public LayerMask collisionMask;
        
        private const float SkinWidth = 0.015f;

        public int horizontalRayCount = 4;
        public int verticalRayCount = 4;

        private float horizontalRaySpacing;
        private float verticalRaySpacing;
        
        private BoxCollider2D _collider2D;
        private RaycastOrigins _raycastOrigins;

        public void Move(Vector2 targetVelocity)
        {
            UpdateRaycastOrigins();

            if (targetVelocity.x != 0)
            {
                HandleHorizontalCollisions(ref targetVelocity);
            }

            if (targetVelocity.y != 0)
            {
                HandleVerticalCollisions(ref targetVelocity);
            }

            transform.Translate(targetVelocity);
        }

        private void HandleHorizontalCollisions(ref Vector2 velocity)
        {
            var directionX = (int) Mathf.Sign(velocity.x);
            var rayLength = Mathf.Abs(velocity.x) + SkinWidth;
            
            for (var i = 0; i < horizontalRayCount; i++)
            { 
                var rayOrigin = (directionX == -1) ? _raycastOrigins.bottomLeft : _raycastOrigins.bottomRight;
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);

                var hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
                Debug.DrawRay(rayOrigin, Vector2.right * (directionX * rayLength), Color.red); 
                
                if (hit)
                {
                    print($"Hit {hit.transform.name}.");
                    velocity.x = (hit.distance - SkinWidth) * directionX;
                    rayLength = hit.distance;
                }
            }
        }

        private void HandleVerticalCollisions(ref Vector2 velocity)
        {
            var directionY = (int) Mathf.Sign(velocity.y);
            var rayLength = Mathf.Abs(velocity.y) + SkinWidth;
            
            for (var i = 0; i < verticalRayCount; i++)
            { 
                var rayOrigin = (directionY == -1) ? _raycastOrigins.bottomLeft : _raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);

                var hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
                Debug.DrawRay(rayOrigin, Vector2.up * (directionY * rayLength), Color.red); 
                
                if (hit)
                {
                    velocity.y = (hit.distance - SkinWidth) * directionY;
                    rayLength = hit.distance;
                }
            }
        }

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
            _raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
            _raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
            _raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
        }
        
        private void Start()
        {
            if (!Physics2D.autoSyncTransforms)
            {
                Physics2D.autoSyncTransforms = true;
            }
            _collider2D = GetComponent<BoxCollider2D>();
            CalculateRaySpacing();
        }
    }
}
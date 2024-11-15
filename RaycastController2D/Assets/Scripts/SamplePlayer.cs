using UnityEngine;

namespace RaycastControllerCore
{
    public class SamplePlayer : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10f;
        public float gravity = -20f;
        
        private Vector2 velocity;
        private Controller2D _controller2D;

        private void Start()
        {
            _controller2D = GetComponent<Controller2D>();

            if (_controller2D == null)
            {
                Debug.LogError($"No controller was found on {name}. Ensure one is attached to this game object.");
            }
        }

        private void Update()
        {
            var input = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

            velocity.x = input.x * moveSpeed;
            velocity.y += gravity * Time.deltaTime;
            _controller2D.Move(velocity * Time.deltaTime);
        }
    }
}
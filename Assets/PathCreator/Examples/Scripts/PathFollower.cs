using UnityEngine;

namespace PathCreation.Examples
{
    public class PathFollower : MonoBehaviour
    {
        [HideInInspector] public PathCreator pathCreator;
        [HideInInspector] public EndOfPathInstruction endOfPathInstruction = EndOfPathInstruction.Stop;
        [HideInInspector] public float speed = 5f;
        private float distanceTravelled;
        private bool isInitialized = false;              // Check if PathFollower has been initialized
        private bool shouldMove = true;                  // Flag to control movement

        void Update()
        {
            if (isInitialized && pathCreator != null && shouldMove)
            {
                // Move the object along the path if it should move
                distanceTravelled += speed * Time.deltaTime;
                transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);

                //Debug.Log("Moving along the path. Distance travelled: " + distanceTravelled);
            }
        }

        // Method to initialize the PathFollower with required data
        public void Initialize(PathCreator creator, EndOfPathInstruction instruction, float moveSpeed)
        {
            pathCreator = creator;
            endOfPathInstruction = instruction;
            speed = moveSpeed;
            isInitialized = true;  // Mark as initialized
        }

        // Method to stop movement (called by EnemyScript)
        public void StopMoving()
        {
            Debug.Log("Moving stopped.");
            shouldMove = false;  // Stop the movement of the enemy
        }

        // Handle path changes
        void OnPathChanged()
        {
            if (pathCreator != null)
            {
                distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
            }
        }
    }
}

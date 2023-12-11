using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class MovementUtils
{
    public static Vector2 CalculateJumpVelocity(Vector3 target, Vector3 myTransform, float gravityScale = 3, List<float> customAngles = null)
    {

        List<float> anglesToBeTested = new List<float>()
            {
                70f,
                60f,
                45f
            };

        if (customAngles != null)
        {
            anglesToBeTested = customAngles;
        }


        List<JumpAngleDto> jumpAngles = new List<JumpAngleDto>();

        foreach (var testingAngle in anglesToBeTested)
        {
            target = new Vector3(target.x, target.y + 0.1f, target.z);

            float gravity = Physics.gravity.magnitude * gravityScale;
            float angle = testingAngle * Mathf.Deg2Rad;

            // Positions of this object and the target on the same plane
            Vector3 planarTarget = new Vector3(target.x, 0, target.z);
            Vector3 planarPosition = new Vector3(myTransform.x, 0, myTransform.z);

            // Planar distance between objects
            float distance = Vector3.Distance(planarTarget, planarPosition);
            // Distance along the y axis between objects
            float yOffset = myTransform.y - target.y;

            float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));
            Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));
            // Rotate our velocity to match the direction between the two objects
            float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPosition);
            Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

            jumpAngles.Add(new JumpAngleDto()
            {
                Angle = testingAngle,
                JumpForce = finalVelocity,
                Power = finalVelocity.x + finalVelocity.y
            });
        }

        JumpAngleDto mostEfficientAngle = jumpAngles.OrderBy(e => e.Power).FirstOrDefault();

        Vector2 jumpForce = new Vector2(target.x > myTransform.x ? mostEfficientAngle.JumpForce.x : -mostEfficientAngle.JumpForce.x, mostEfficientAngle.JumpForce.y);

        return jumpForce;
    }

    public struct JumpAngleDto
    {
        public float Angle { get; set; }
        public float Power { get; set; }
        public Vector2 JumpForce { get; set; }
    }
}

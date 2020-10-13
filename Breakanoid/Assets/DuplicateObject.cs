using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DuplicateObject : BonusObjectBase
{
    [SerializeField] int numberOfDuplications = 1;

    private void DoubleObject()
    {
        var existingBalls = FindObjectsOfType<BallStart>().Where(s => s.name.StartsWith("Ball"));
        if (existingBalls.Count() > 7)
        {
            return;
        }

        foreach (var ball in existingBalls)
        {
            var transform = ball.transform;
            var newBall = Instantiate(ball, transform.position, transform.rotation);
            var rigidBody = ball.GetComponent<Rigidbody2D>();
            var newRigidBody = newBall.GetComponent<Rigidbody2D>();
            if (rigidBody != null)
            {
                var newVelocity = rigidBody.velocity;
                newVelocity.x *= -1.4f;
                newVelocity.y = Mathf.Abs(newVelocity.y) * 1.4f;

                newRigidBody.velocity = newVelocity;
            }
        }

        Destroy(this.gameObject);
    }

    public override void OnCollided()
    {
        this.DoubleObject();
    }
}

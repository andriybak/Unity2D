using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoubleObjectScaleXY : BonusObjectBase
{
    [SerializeField] float BonusDuration = 3f;

    private IEnumerator IncreseXYScale(BallStart ball)
    {
        var newScale = ball.transform.localScale;
        newScale.x *= 2;
        newScale.y *= 2;

        ball.transform.localScale = newScale;

        yield return new WaitForSeconds(BonusDuration);

        ball.transform.localScale = new Vector3(1f, 1f, 0f);
    }

    public override void OnCollided()
    {
        var existingBalls = FindObjectsOfType<BallStart>().Where(s => s.name.StartsWith("Ball"));
        foreach (var ball in existingBalls)
        {
            StartCoroutine(IncreseXYScale(ball));
        }
    }
}

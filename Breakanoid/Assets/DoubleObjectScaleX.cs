using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleObjectScaleX : BonusObjectBase
{
    [SerializeField] int BonusDuration = 3;

    private IEnumerator ScaleObjectForTime()
    {
        var paddleObject = this.paddleCollider.gameObject;
        var currentScale = paddleObject.transform.localScale;
        currentScale.x = currentScale.x * 2;
        paddleObject.transform.localScale = currentScale;

        yield return new WaitForSeconds(this.BonusDuration);

        paddleObject.transform.localScale = Vector3.one;
    }

    public override void OnCollided()
    {
        StartCoroutine(ScaleObjectForTime());
    }
}

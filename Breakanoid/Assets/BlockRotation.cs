using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRotation : MonoBehaviour
{
    [SerializeField] float rotationPerSecondDegrees = 0.5f;
    [SerializeField] float startDelay = 1f;

    [SerializeField] bool customRotationPoint = false;
    [SerializeField] Vector2 rotateAroundPoint2D;

    private bool isStarted = false;
    private float creationTime = 0f;

    private void RotateBlock()
    {
        this.gameObject.transform.RotateAround(this.rotateAroundPoint2D, Vector3.forward, rotationPerSecondDegrees);
    }

    // Start is called before the first frame update
    void Start()
    {
        creationTime = Time.time;
        if (!this.customRotationPoint || this.rotateAroundPoint2D == null)
        {
            this.rotateAroundPoint2D = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStarted && Time.time - this.creationTime > this.startDelay)
        {
            isStarted = true;
            this.RotateBlock();
        }

        if (isStarted)
        {
            this.RotateBlock();
        }
    }
}

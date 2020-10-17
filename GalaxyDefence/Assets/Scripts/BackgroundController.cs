using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] public float backgroundSpeed = 0.001f;

    private Renderer backgroundRender;

    private void Start()
    {
        backgroundRender = this.gameObject.GetComponent<Renderer>();

    }

    private void Update()
    {
        this.backgroundRender.material.mainTextureOffset += new Vector2(0f, backgroundSpeed);
    }
}

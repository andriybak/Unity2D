using UnityEngine;
using System.Collections;
public class BonusObjectBase : MonoBehaviour
{
    public Collider2D objectCollider = null;
    public Collider2D paddleCollider = null;

    public bool didCollide = false;

    // Use this for initialization
    void Start()
    {
        this.objectCollider = this.gameObject.GetComponent<Collider2D>();
        this.paddleCollider = FindObjectOfType<Paddle>().GetComponent<Collider2D>();
    }

    public virtual void OnCollided()
    { }

    void Update()
    {
        if (this.objectCollider.IsTouching(this.paddleCollider) && !didCollide)
        {
            this.didCollide = true;
            this.OnCollided();
        }
    }
}
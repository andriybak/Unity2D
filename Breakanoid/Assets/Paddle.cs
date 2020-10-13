using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField] float screenWidthInUnits = 16.0f;

    public void ResetScale()
    {
        this.gameObject.transform.localScale = new Vector3(1f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        var mousePosX = Input.mousePosition.x / Screen.width * screenWidthInUnits;
        var newPosX = Mathf.Clamp(mousePosX, 1, 15);
        transform.position = new Vector3(newPosX, transform.position.y, transform.position.z);
    }
}

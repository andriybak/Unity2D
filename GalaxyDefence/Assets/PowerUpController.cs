using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    HealFull = 0,
    SpeedUpFire,
    LvlUpGuns,
    Shield
}

public class PowerUpController : MonoBehaviour
{
    [SerializeField] public PowerUpType Type;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            this.ApplyBonusByType(collision.gameObject);
        }
    }

    private void ApplyBonusByType(GameObject player)
    {
        var playerController = player.GetComponent<PlayerController>();
        if (playerController == null)
        {
            return;
        }

        switch (this.Type)
        {
            case PowerUpType.HealFull:
                playerController.SetFullHp();
                break;

            case PowerUpType.LvlUpGuns:
                playerController.LvlUpGuns();
                break;

            case PowerUpType.SpeedUpFire:
                playerController.SpeedUpFire();
                break;

            case PowerUpType.Shield:
                playerController.SetUpShield();
                break;

            default:
                break;
        }

        Destroy(this.gameObject);
    }


}

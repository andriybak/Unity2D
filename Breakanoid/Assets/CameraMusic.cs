using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMusic : MonoBehaviour
{
    [SerializeField] List<AudioClip> gameMusicList;

    // Start is called before the first frame update
    void Start()
    {
        if (gameMusicList.Count > 0)
        {
            var songIndex = Random.Range(0, gameMusicList.Count);
            var musicSource = GetComponent<AudioSource>();
            musicSource.clip = gameMusicList[songIndex];
            musicSource.Play();
        }
    }
}

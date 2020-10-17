using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MusicPlayerController : MonoBehaviour
{
    [SerializeField] List<AudioClip> songs;

    private AudioSource player;

    // Start is called before the first frame update
    void Awake()
    {
        if (FindObjectsOfType(this.GetType()).Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        this.player = this.GetComponent<AudioSource>();
        if (this.songs.Count > 0)
        {
            var songToPlay = Random.Range(0, this.songs.Count);
            this.player.clip = this.songs[songToPlay];
        }
    }

    private void PlayNewSong()
    {
        var newSongs = this.songs.Where(song => song.name != this.player.clip.name).ToList();
        if (newSongs.Any())
        {
            var songToPlay = Random.Range(0, newSongs.Count);
            this.player.clip = newSongs[songToPlay];
        }

        this.player.Play();
    }

    private void Update()
    {
        if (!this.player.isPlaying)
        {
            this.PlayNewSong();
        }
    }
}

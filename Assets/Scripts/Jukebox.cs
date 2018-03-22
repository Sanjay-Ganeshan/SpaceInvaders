using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Jukebox will play music, and control game options regarding sound
/// </summary>
public class Jukebox : MonoBehaviour {

    /// <summary>
    /// The Input button to listen for that will mute/unmute SFX
    /// </summary>
    const string INPUT_MUTE_SFX = "MuteSFX";

    /// <summary>
    /// The Input button to listen for that will mute/unmute Music
    /// </summary>
    const string INPUT_MUTE_MUSIC = "MuteMusic";

    /// <summary>
    /// All the songs that the jukebox might play
    /// </summary>
    public AudioClip[] Songs;

    /// <summary>
    /// Whether or not playback of music is paused
    /// </summary>
    public bool IsPaused = false;

    /// <summary>
    /// The AudioSource to play music from
    /// </summary>
    private AudioSource player;

    /// <summary>
    /// The currently playing track number (index in Songs[])
    /// </summary>
    private int trackNumber = -1;

    /// <summary>
    /// Whether or not the tracks will repeat after all songs have been played.
    /// </summary>
    public bool Repeat;

    /// <summary>
    /// Whether or not the currently playing track will loop endlessly.
    /// </summary>
    public bool Loop;

    /// <summary>
    /// Whether or not to randomize the order in which songs are played.
    /// </summary>
    public bool Shuffle;

    /// <summary>
    /// The playlist. The 0th element is the next song that will play.
    /// </summary>
    private List<int> Playlist;

    /// <summary>
    /// Random Number Generator that can generate ints.
    /// </summary>
    System.Random randGen;

	// Use this for initialization
	void Start () {
        // Get the audio source you'll play from
        player = GetComponent<AudioSource>();

        // Initialize the playlist. This reference will never change.
        Playlist = new List<int>();

        // Create a new RNG.
        randGen = new System.Random();

        // Add all tracks from the known songs
        AddAllTracks();
	}
	
	// Update is called once per frame
	void Update () {

        // Listen for input to mute/unmute SFX
		if(Input.GetButtonDown(INPUT_MUTE_SFX))
        {
            GameConfig.SFXEnabled = !GameConfig.SFXEnabled;
        }

        // Listen for input to mute/unmute Music
        if(Input.GetButtonDown(INPUT_MUTE_MUSIC))
        {
            GameConfig.MusicEnabled = !GameConfig.MusicEnabled;

            // Stop currently playing music if needed.
            if(!GameConfig.MusicEnabled)
            {
                Stop();
            }
            else
            {
                this.IsPaused = false;
            }
        }

        // Check if we have reached the end of a clip, 
        // and need to move onto the next song
        if (GameConfig.MusicEnabled && !this.IsPaused && !player.isPlaying)
        {
            // We must have reached the end of a clip.
            NextSong();
        }
	}

    /// <summary>
    /// Adds all tracks in Songs[] to Playlist, shuffling them if needed
    /// </summary>
    void AddAllTracks()
    {
        Playlist.Clear();
        for(int i = 0; i < this.Songs.Length; i++)
        {
            Playlist.Add(i);
        }
        if (Shuffle)
        {
            List<int> shuffled = new List<int>();
            int nextIndex;
            while(Playlist.Count > 0)
            {
                nextIndex = randGen.Next(0, Playlist.Count);
                shuffled.Add(Playlist[nextIndex]);
                Playlist.RemoveAt(nextIndex);
            }
            foreach(int i in shuffled)
            {
                Playlist.Add(i);
            }
        }
    }

    /// <summary>
    /// Pauses playback of music, but keeps the position
    /// in the current song and playlist.
    /// </summary>
    public void Pause()
    {
        // Affect the Audio source
        if(this.IsPaused)
        {
            player.UnPause();
        }
        else
        {
            player.Pause();
        }

        // Store state internally that will carry through songs ending
        this.IsPaused = !this.IsPaused;
    }

    /// <summary>
    /// Stop all playback of music, maintaining the playlist but losing
    /// position in the currently playing track.
    /// </summary>
    public void Stop()
    {
        this.IsPaused = true;
        this.player.Stop();
        this.trackNumber = -1;

    }

    /// <summary>
    /// Advances playback to the next song
    /// </summary>
    public void NextSong()
    {
        // If we don't have any songs left
        if(this.Playlist.Count == 0)
        {
            // But we're on Repeat mode, repopulate them
            if(this.Repeat)
            {
                AddAllTracks();
            }

            // Otherwise, we're out of songs. Stop music playback.
            else
            {
                Stop();
            }
        }

        // If, after refreshing songs as needed / allowed, we 
        // have a track to play
        if(this.Playlist.Count > 0 || (trackNumber >= 0 && trackNumber <= Songs.Length && Loop))
        {
            int nextIndex;
            // Choose the same song we're playing to be played next
            // when in Loop mode
            if (Loop)
            {
                nextIndex = trackNumber;
            }
            // Or pick the next song in the playlist if not in Loop
            else
            {
                nextIndex = this.Playlist[0];
                this.Playlist.RemoveAt(0);
            }

            // Stop any playback
            this.player.Stop();
            
            // Change the clip in the AudioSource to the right one
            this.player.clip = this.Songs[nextIndex];

            // And start playback
            this.player.Play();

            // Store the playing track number
            trackNumber = nextIndex;

            // And assert that IsPaused is false,
            // Because we know we have playback
            this.IsPaused = false;
        }
    }
}

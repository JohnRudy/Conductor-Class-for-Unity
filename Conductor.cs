///<summary>
///MIT License
///
///Copyright(c) 2021 John Rudy
///
///Permission is hereby granted, free of charge, to any person obtaining a copy
///of this software and associated documentation files (the "Software"), to deal
///in the Software without restriction, including without limitation the rights
///to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
///copies of the Software, and to permit persons to whom the Software is
///furnished to do so, subject to the following conditions:
///
///The above copyright notice and this permission notice shall be included in all
///copies or substantial portions of the Software.
///
///THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
///IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
///FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
///AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
///LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
///OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
///SOFTWARE.
/// </summary>

//NOTE: This is a super old class I did on a forum post found on Gamasutra on rythm games and how to 
//Get a beat and bar position from music

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour {
    [Header("Conductor Class")]
    [Tooltip("User set")]
    [SerializeField] float songBpm;
    [SerializeField] float secPerBeat;
    [SerializeField] float songPos;
    [SerializeField] float songPosInBeats;
    [SerializeField] float dspSongTime;
    [Tooltip("User set")] public float firstBarOffset;
    [SerializeField] AudioSource audioSource;
    
    [Header("Bar information")]
    [Tooltip("User set")] public float beatsPerBar;
    [SerializeField] int completedBar = 0;
    [SerializeField] float BarPosInBeats;
    
    [Header("Analog position in loop")]
    [SerializeField] float barPosInAnalog;
    
    [Header("For other classes")]
    [SerializeField] int lastBeat = -1;
    [SerializeField] int lastBar = -1;

    public event Action Beat;
    public event Action Bar;

    //////////////////////////////
    //----|_0_|_1_|_2_|_3_|-----//
    //--Each is the first beat--//
    //-------Bar is 0-3---------//
    //////////////////////////////

    private void Start() {
        //60 FPS
        secPerBeat = 60f / songBpm;
        //Song time from samples
        dspSongTime = (float)AudioSettings.dspTime;

        PlaySong();
    }

    private void Update() {
        //Current song position from samples
        songPos = (float)AudioSettings.dspTime - dspSongTime;
        
        //Current position in the song in seconds
        songPosInBeats = songPos / secPerBeat;
        
        //Each 4/4 Bar count
        if (songPosInBeats >= (completedBar +1) * beatsPerBar) { completedBar++; }
        
        //In what position the Bar is in the song
        BarPosInBeats = songPosInBeats - completedBar * beatsPerBar;
        
        // Beat position in bars
        barPosInAnalog = BarPosInBeats / beatsPerBar;
        
        //Each beat called 4/4
        if (lastBeat != (int)songPosInBeats) {
            lastBeat++;
            Beat();
        }
        //Called each Bar 
        if (lastBar != (int)completedBar) {
            lastBar++;
            Bar();
        }
    }

    private void PlaySong () {
        audioSource.Play();
    }
}
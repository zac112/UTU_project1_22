using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAudioHandler : MonoBehaviour
{
    [SerializeField] AudioClip[] ambience;
    [SerializeField] AudioClip[] battle;
    AudioSource source;
    DayNight daynight;

    // Start is called before the first frame update
    void Start(){
        daynight = GameObject.FindObjectOfType<DayNight>();
        source = GetComponent<AudioSource>();
        OnEnable();
    }

    void OnEnable(){
        GameEvents.current.Tick += Tick;
    }

    void OnDisable(){
        GameEvents.current.Tick -= Tick;
    }

    void Tick(int tick){
        //float pitch = (0f+tick%daynight.getDayLength())/daynight.getDayLength();
        float pitch = Mathf.Cos(tick*Mathf.PI/daynight.getDayLength())/4+0.5f;
        source.pitch = pitch;
    }
}

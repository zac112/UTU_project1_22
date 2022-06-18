using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAudioHandler : MonoBehaviour
{
    [SerializeField] AudioClip[] ambience;
    [SerializeField] AudioClip[] battle;
    AudioSource[] source;
    DayNight daynight;

    // Start is called before the first frame update
    void Start(){
        daynight = GameObject.FindObjectOfType<DayNight>();
        source = GetComponents<AudioSource>();
    }

    void OnEnable(){
        GameEvents.current.Tick += Tick;
    }

    void OnDisable(){
        GameEvents.current.Tick -= Tick;
    }

    void Tick(int tick){
        float d = daynight.getDayLength();

        float daypitch = Mathf.Clamp01(Mathf.Cos(tick*2*Mathf.PI/d) + 0.1f);
        float nightpitch = Mathf.Clamp01(Mathf.Cos(tick*2*Mathf.PI/d + Mathf.PI) + 0.1f);

        source[0].volume = daypitch;
        source[1].volume = nightpitch;
    }
}

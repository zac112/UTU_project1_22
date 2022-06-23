using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
    [Serializable]
    private class AudioInstance{
        [SerializeField] public AudioType category;
        [SerializeField] public AudioClip clip;
    }
    [SerializeField] List<AudioInstance> clips;
    
    private AudioSource source;

    void Awake(){
        source = GetComponent<AudioSource>();        
    }

    //Plays the first clip associated with the player
    public void Play(AudioType category){
        Play(category, 0);
    }

    //Plays the clip at the given index or nothing
    public void Play(AudioType category, int clip)
    {
        if (GetCategory(category).Count <= clip) return;
        source.PlayOneShot(clips[clip].clip);
    }

    public void PlayRandom(AudioType category){
        Play(category, UnityEngine.Random.Range(0,GetCategory(category).Count));
    }

    private List<AudioInstance> GetCategory(AudioType category){
        return clips.FindAll((c) => c.category.Equals(category));
    }
}


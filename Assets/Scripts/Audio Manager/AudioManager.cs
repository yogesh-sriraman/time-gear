using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    public static AudioManager instance;
    public Dictionary<string, Sound> soundDictionary=new Dictionary<string, Sound>();
    public bool isMuted;

    // Use this for initialization
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.removeAfterUse = false;
          if(!soundDictionary.ContainsKey(s.name))
          {
             soundDictionary.Add(s.name, s);
          }
        }
       
    }
    private void OnEnable()
    {
        PlaySound("Theme Music");
    }
    public void PlaySound(string soundName)
    {
        if (isMuted)
            return;

        Sound s = soundDictionary[soundName];
        if(s==null)
        {
            Debug.Log("Sound : "+s.name+" not found");
            return;
        }
        s.source.Play();
    }

    public void PlaySound(AudioClip audioClip)
    {
        Sound s = new Sound
        {
            source = gameObject.AddComponent<AudioSource>()
        };
        s.source.clip = audioClip;
        s.source.volume = 0.8f;
        s.source.pitch = 0.5f;
        s.source.loop = false;
        s.removeAfterUse = true;
        s.source.Play();
    }
    public void StopSound(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.Log("Sound : " + s.name + " not found");
            return;
        }
        s.source.Stop();
    }

    

    public void RemoveAudioFiles()
    {
        List<string> selecetdAudioFiles = GetRemovableFileNames();
        foreach (var item in selecetdAudioFiles)
        {
            soundDictionary.Remove(item);
        }
    }
    public List<string> GetRemovableFileNames()
    {
        List<string> selecetdAudioFiles = new List<string>();
        foreach (var item in soundDictionary)
        {
            if(item.Value.removeAfterUse)
            {
                selecetdAudioFiles.Add(item.Key);
            }
        }
        return selecetdAudioFiles;
    }
    public bool IsSoundPlaying(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.Log("Sound : " + s.name + " not found");
            return false;
        }
        return (s.source.isPlaying);
    }
    public void PauseAudio(string soundName, bool value)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
        {
            Debug.Log("Sound : " + s.name + " not found");
        }
        if (value)
        {
            s.source.Pause();
        }
        else
        {
            s.source.Play();
        }
    }
    public void setVolume(string soundName, float value)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s != null)
            s.source.volume = value;
    }

    public float getVolume(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if (s == null)
            return 0;

        return s.source.volume;
    }

    public void setSoundVolume(float value)
    {
        foreach(Sound s in sounds)
        {
            if(s.name != "Theme Music")
            {
                s.source.volume = value;
            }
        }
    }

    public float getSoundVolume()
    {
        float vol = 0;
        foreach (Sound s in sounds)
        {
            if (s.name != "Theme Music")
            {
                vol = s.source.volume;
                break;
            }
        }
        return vol;
    }
}

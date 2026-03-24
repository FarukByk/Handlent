using System;
using UnityEngine;

public class soundSystem : MonoBehaviour
{	
    public sounds[] sound;
    public GameObject pref;

    public static void play(string name)
    {
        soundSystem sa = FindAnyObjectByType<soundSystem>();
        foreach (sounds s in sa.sound)
        {
            if (s.soundName == name)
            {
                GameObject go = Instantiate(sa.pref);
                go.GetComponent<AudioSource>().clip = s.sound;
                go.GetComponent<AudioSource>().volume = s.volume;
                go.GetComponent<AudioSource>().Play();
                Destroy(go, s.second);
            }
        }
    }
}
[Serializable]
public class sounds
{
    public string soundName;
    public AudioClip sound;
    public float volume;
    public float second;
}
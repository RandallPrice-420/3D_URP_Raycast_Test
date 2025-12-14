using UnityEngine;


public class SoundManager : Singleton<SoundManager>
{
    // -------------------------------------------------------------------------
    // Public Enumerations:
    // --------------------
    //   Sounds
    // -------------------------------------------------------------------------

    #region .  Public Enumerations  .

    public enum Sounds
    {
        Explosion_1   = 1,
        Glass_Shatter = 2,
        Pop_1         = 3,
        Pop_2         = 4,
        Pop_3         = 5,
        Pop_4         = 6,
        Pop_5         = 7,
        Pop_6         = 8,
        Pop_7         = 9,
        Pop_8         = 10,
        Gun_Shot      = 11,
        Laser_Gun     = 12
    }

    #endregion


    // -------------------------------------------------------------------------
    // Public Properties:
    // ------------------
    //   Audio
    //   SoundAudioClips
    // -------------------------------------------------------------------------

    #region .  Public Properties  .

    public AudioSource      Audio;
    public SoundAudioClip[] SoundAudioClips;

    #endregion



    // -------------------------------------------------------------------------
    // Public Methods:
    // ---------------
    //   PlaySound() - name
    //   PlaySound() - sound
    // -------------------------------------------------------------------------

    #region .  PlaySound() - name  .
    // -------------------------------------------------------------------------
    //   Method.......:  PlaySound()
    //   Description..:  
    //   Parameters...:  None
    //   Returns......:  Nothing
    // -------------------------------------------------------------------------
    public void PlaySound(string name)
    {
        try
        {
            this.Audio.PlayOneShot(this.GetAudioClip(name));
        }
        catch
        {
            Debug.Log($"SoundManager.PlaySound(name) called with a null name.");
        }

    }   // PlaySound()
    #endregion


    #region .  PlaySound() - sound  .
    // -------------------------------------------------------------------------
    //   Method.......:  PlaySound()
    //   Description..:  
    //   Parameters...:  None
    //   Returns......:  Nothing
    // -------------------------------------------------------------------------
    public void PlaySound(Sounds sound)
    {
        try
        {
            this.Audio.PlayOneShot(this.GetAudioClip(sound));
        }
        catch
        {
            Debug.Log($"SoundManager.PlaySound(sound) called with a null sound.");
        }

    }   // PlaySound()
    #endregion



    // -------------------------------------------------------------------------
    // Private Methods:
    // ----------------
    //   GetAudioClip() - name
    //   GetAudioClip() - sound
    // -------------------------------------------------------------------------

    #region .  GetAudioClip() - name  .
    // -------------------------------------------------------------------------
    //   Method.......:  GetAudioClip()
    //   Description..:  
    //   Parameters...:  None
    //   Returns......:  AudioClip
    // -------------------------------------------------------------------------
    private AudioClip GetAudioClip(string name)
    {
        foreach (SoundAudioClip soundAudioClip in SoundAudioClips)
        {
            if (soundAudioClip.sound.ToString() == name)
            {
                return soundAudioClip.audioClip;
            }
        }

        Debug.Log($"SoundManager.GetAudioClip({name}) not found or is null.");
        return null;

    }   // GetAudioClip()
    #endregion


    #region .  GetAudioClip() - sound  .
    // -------------------------------------------------------------------------
    //   Method.......:  GetAudioClip()
    //   Description..:  
    //   Parameters...:  None
    //   Returns......:  Nothing
    // -------------------------------------------------------------------------
    private AudioClip GetAudioClip(Sounds sound)
    {
        foreach (SoundAudioClip soundAudioClip in SoundAudioClips)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }

        Debug.Log($"SoundManager.GetAudioClip({sound}) not found or is null.");
        return null;

    }   // GetAudioClip()
    #endregion



    // --------------------------------------------------------------
    // Public Classes:
    // ---------------
    //   SoundAudioClip
    // --------------------------------------------------------------

    #region .  Public Classes  .

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sounds sound;
        public AudioClip           audioClip;

    }   // class SoundAudioClip

    #endregion


}   // class SoundManager

using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private float sfxMinimumDistance;
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;
    [SerializeField] private AudioSource[] uiSfx;
    private AudioSource currentBGM;
    private bool canPlaySFX;

    private void Awake() 
    {

        if (instance != null && instance != this) 
            Destroy(instance.gameObject);
        else
            instance = this;
        
        Invoke(nameof(AllowSFX), .1f);

        foreach(var source in uiSfx)
        {
            source.ignoreListenerPause = true;
        }
        
        foreach(var source in bgm)
        {
            source.ignoreListenerPause = true;
        }
    }

    #region SFX region
    public void PlaySFX(int _sfxIndex, Transform _source)
    {
        // if(sfx[_sfxIndex].isPlaying)
        //     return;
    
        if(!canPlaySFX)
            return;
        
        //sound distance logic
        if(_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > sfxMinimumDistance)
            return;

        if(_sfxIndex < sfx.Length)
        {
            sfx[_sfxIndex].pitch = Random.Range(0.85f, 1.2f);
            sfx[_sfxIndex].Play();
        }
    }

    public void PlaySFXNoPitch(int _sfxIndex, Transform _source)
    {
        if(!canPlaySFX)
            return;
        
        //sound distance logic
        if(_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > sfxMinimumDistance)
            return;

        if(_sfxIndex < sfx.Length)
            sfx[_sfxIndex].Play();
    }

    public void PlayUISFX(int _sfxIndex)
    {
        if(!canPlaySFX)
            return;
            
        if(_sfxIndex >= 0 && _sfxIndex < uiSfx.Length)
        {
            uiSfx[_sfxIndex].pitch = Random.Range(0.85f, 1.2f);
            uiSfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _index) => sfx[_index].Stop();
    
    private void AllowSFX() => canPlaySFX = true;

    public void StopFSXWithTime(int _index) => StartCoroutine(DecreaseVolume(sfx[_index]));
    #endregion

    #region BGM music region
    public void PlayBGM(int _bgmIndex)
    {
        if (currentBGM == bgm[_bgmIndex]) 
            return;

        StopBGM();

        currentBGM = bgm[_bgmIndex];
        currentBGM.loop = true;
        currentBGM.Play();
    }

    public void StopBGM()
    {
        if (currentBGM != null)
        {
            currentBGM.Stop();
            currentBGM = null;
        }
    }

    #endregion

    private IEnumerator DecreaseVolume(AudioSource _audio)
    {
        float defaultVolume = _audio.volume;

        while(_audio.volume > .1f)
        {
            _audio.volume -= _audio.volume * .2f;
            yield return new WaitForSeconds(.25f);

            if(_audio.volume <= .1f)
            {
                _audio.Stop();
                _audio.volume = defaultVolume;
                break;
            }
        }
    }
}

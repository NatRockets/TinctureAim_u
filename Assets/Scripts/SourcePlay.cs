using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SourcePlay : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup mixerMasterGroup;

    [SerializeField] private AudioSource backS;
    [SerializeField] private AudioSource correctS;
    [SerializeField] private AudioSource wrongS;
    
    [SerializeField] private Toggle audioToggle;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;
    
    private Image toggleImage;

    private void OnEnable()
    {
        backS.Play();
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("GVolume"))
        {
            PlayerPrefs.SetInt("GVolume", 1);
        }
        
        toggleImage = audioToggle.GetComponent<Image>();
        
        audioToggle.onValueChanged.AddListener(SwitchAudio);
        audioToggle.isOn = (PlayerPrefs.GetInt("GVolume") == 1);
        
        toggleImage.sprite = audioToggle.isOn ? onSprite : offSprite;
    }

    private void OnDisable()
    {
        audioToggle.onValueChanged.RemoveListener(SwitchAudio);
    }
    
    private void SwitchAudio(bool isOn)
    {
        mixerMasterGroup.audioMixer.SetFloat("_MasterVolume", isOn ? 0 : -80);
        toggleImage.sprite = audioToggle.isOn ? onSprite : offSprite;
    }

    public void Play(int type)
    {
        AudioSource target = type switch
        {
            1 => correctS,
            2 => wrongS,
            _ => throw new System.NotSupportedException()
        };
        target.Play();
    }
}
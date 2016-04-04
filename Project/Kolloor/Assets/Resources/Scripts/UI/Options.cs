using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Options : MonoBehaviour
{
    public static Options instance;

    public GameObject optionsObject;
    public GameObject audioObject;
    public GameObject controlsObject;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Image musicImage;
    public Image sfxImage;
    Sprite audioOffSprite;
    Sprite audioOn1Sprite;
    Sprite audioOn2Sprite;
    Sprite audioOn3Sprite;


    void Start()
    {
        instance = this;

        audioOffSprite = Resources.Load<Sprite>("UI/Audio/audioOff");
        audioOn1Sprite = Resources.Load<Sprite>("UI/Audio/audioOn1");
        audioOn2Sprite = Resources.Load<Sprite>("UI/Audio/audioOn2");
        audioOn3Sprite = Resources.Load<Sprite>("UI/Audio/audioOn3");

        musicSlider.value = AudioManager.instance.musicVolumePercent * 100;
        sfxSlider.value = AudioManager.instance.sfxVolumePercent * 100;

        SetUI(musicSlider, musicImage, "Music Volume: ", AudioManager.instance.musicVolumePercent * 100);
        SetUI(sfxSlider, sfxImage, "Sfx Volume: ", AudioManager.instance.sfxVolumePercent * 100);
    }

    public void Controls()
    {
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        optionsObject.SetActive(false);
        audioObject.SetActive(false);
        controlsObject.SetActive(true);
    }

    public void Audio()
    {
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        optionsObject.SetActive(false);
        audioObject.SetActive(true);
        controlsObject.SetActive(false);
    }

    public void Back()
    {
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        optionsObject.SetActive(true);
        audioObject.SetActive(false);
        controlsObject.SetActive(false);
    }

    public void MusicSlider()
    {
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        AudioManager.instance.SetVolume(musicSlider.value, AudioChannel.Music);
        SetUI(musicSlider, musicImage, "Music Volume: ", AudioManager.instance.musicVolumePercent * 100);
    }

    public void SfxSlider()
    {
        AudioManager.instance.PlaySound(AudioCategory.UI, false, true);
        AudioManager.instance.SetVolume(sfxSlider.value, AudioChannel.Sfx);
        SetUI(sfxSlider, sfxImage, "Sfx Volume: ", AudioManager.instance.sfxVolumePercent * 100);
    }

    void SetUI(Slider slider, Image image, string text, float percent)
    {
        if (slider.value > 0 && slider.value < 33)
            image.sprite = audioOn1Sprite;
        else if (slider.value > 33 && slider.value < 66)
            image.sprite = audioOn2Sprite;
        else if (slider.value > 66)
            image.sprite = audioOn3Sprite;
        else
            image.sprite = audioOffSprite;

        slider.GetComponentInChildren<Text>().text = text + percent + "%";
    }
}
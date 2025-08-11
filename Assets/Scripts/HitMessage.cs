using UnityEngine;

public class HitMessage : MonoBehaviour
{
    [SerializeField] private Sprite hitSprite;
    [SerializeField] private Sprite missSprite;
    [SerializeField] private CutScreen animatedImage;

    private void Play()
    {
        animatedImage.SetPos(100f);
        animatedImage.Upscale(true, () =>
        {
            animatedImage.Descale(true, 0.4f);
        });
    }
    
    public void InitMessage()
    {
        animatedImage.Setup();
    }

    public void PlayHit()
    {
        animatedImage.SetView(hitSprite);
        Play();
    }

    public void PlayMiss()
    {
        animatedImage.SetView(missSprite);
        Play();
    }

    public void ResetMessage()
    {
        animatedImage.Descale();
    }
}

using UnityEngine;
using UnityEngine.UI;

public class PlayAnimatedUI : MonoBehaviour
{
    [SerializeField] private Image animatedImage;
    [SerializeField] private Sprite[] frames;
    [SerializeField] private float frameRate = 10f;

    private int currentFrame = 0;
    private float timer = 0f;

    void Start()
    {
        if (frames.Length > 0 && animatedImage != null)
        {
            animatedImage.sprite = frames[0];
        }
    }
    void Update()
    {
        if (currentFrame >= frames.Length) return;
        
        timer += Time.deltaTime;
        if(timer >= 1f/frameRate)
        {
            currentFrame++;
            if(currentFrame >= frames.Length)
            {
                Destroy(gameObject);
                return;
            }

            animatedImage.sprite = frames[currentFrame];
            timer = 0f;
        }

    }
}

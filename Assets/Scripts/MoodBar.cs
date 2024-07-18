
using UnityEngine;
using UnityEngine.UI;

public class MoodBar : BarUI
{
    public Image itemImage;
    public Image moodImage;
    public override void UpdateBar()
    {
        float fillAmount = CurrentValue / MaxValue;
        progressForeBar.fillAmount = fillAmount;

        if (CurrentValue <= MaxValue * 0.25)
        {
            progressForeBar.color = new Color(241f / 255f, 60f / 255f, 15f / 255f, 1);
        }
        else if (CurrentValue <= MaxValue * 0.5)
        {
            progressForeBar.color = new Color(241f / 255f, 176f / 255f, 15f / 255f, 1); 
        }
        else progressForeBar.color = new Color(110f / 255f, 188f / 255f, 95f / 255f, 1);
    }

    public void ShowMood(Sprite moodSprite)
    {
        moodImage.sprite = moodSprite;
    }
}
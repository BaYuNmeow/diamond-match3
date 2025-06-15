using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoalPanel : MonoBehaviour
{
    public Image thisImage;
    public Sprite thisSprite;
    public TMP_Text thisText;
    public string thisString;

    public void Setup()
    {
        thisImage.sprite = thisSprite;
        thisText.text = thisString;
    }
}

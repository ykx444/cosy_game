using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Tutorial
{
    public string header;
    public Sprite image;
    public string content;

    // Constructor to initialize the tutorial object
    public Tutorial(string header, Sprite image, string content)
    {
        this.header = header;
        this.image = image;
        this.content = content;
    }
}
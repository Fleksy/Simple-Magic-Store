using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MessageScript : MonoBehaviour
{
    private Text text;
    private Image image;

    public void Show()
    {
        text = transform.GetChild(0).GetComponent<Text>();
        image = GetComponent<Image>();
        StartCoroutine("ShowMessage");
    }

    IEnumerator ShowMessage()
    {
        for (float f = 1f; f >= 0; f -= 0.005f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, f);
            image.color = new Color(image.color.r, image.color.g, image.color.b, f);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    [SerializeField] string[] gameOverMessages;
    [SerializeField] Text labelMessage;
    [SerializeField] PlayableDirector fadeOutDirector;
    [SerializeField] PlayableDirector fadeInDirector;

    public void SetMessage(string textMessage)
    {
        labelMessage.text = textMessage;
    }

    public void SetRandomMessage()
    {
        labelMessage.text = gameOverMessages[Random.Range(0, gameOverMessages.Length)];
    }

    public void FadeIn(bool showMessage)
    {
        labelMessage.gameObject.SetActive(showMessage);
        fadeOutDirector.Stop();
        fadeInDirector.Stop();
        fadeInDirector.Play();
    }

    public void FadeOut(bool showMessage)
    {
        labelMessage.gameObject.SetActive(showMessage);
        fadeOutDirector.Stop();
        fadeInDirector.Stop();
        fadeOutDirector.Play();
    }
}

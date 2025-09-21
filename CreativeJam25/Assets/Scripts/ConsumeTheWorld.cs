using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConsumeTheWorld : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ConsumeTheWorldPrompt;

    [SerializeField] private TextMeshProUGUI EdgyQuote;

    Consume inputAction;

    Image panelImage;

    public void Start()
    {
        panelImage = GetComponent<Image>();
        panelImage.color = new Color(0f, 0f, 0f, 0.0f);
        EdgyQuote.color = new Color(1f, 1f, 1f, 0.0f);
        inputAction = new Consume();
        inputAction.Action.Consume.started += ctx => Consume();
        ConsumeTheWorldPrompt.gameObject.SetActive(false);
        gameObject.SetActive(false);
        PlayerStats.instance.ConsumeTheWorld.AddListener(ShowPrompt);
    }

    public void ShowPrompt()
    {
        gameObject.SetActive(true);
        ConsumeTheWorldPrompt.gameObject.SetActive(true);
        inputAction.Enable();
    }

    public void Consume()
    {
        ConsumeTheWorldPrompt.gameObject.SetActive(false);
        inputAction.Disable();
        PlayerState.instance.currentState = PlayerState.PlayerStates.Win;
        //Player and enemies are frozen as the screen fades to black
        //After the fade, it will show an edgy quote about starting with nothing but consuming everything.
        //Main Menu.
        StartCoroutine(FadeOutPanel());
    }

    public IEnumerator FadeOutPanel()
    {
        float duration = 5.0f; // Duration of the fade
        float elapsedTime = 0f;

        Color startColor = new Color(0f, 0f, 0f, 0.0f); // Starting color (black with zero opacity)
        Color endColor = new Color(0f, 0f, 0f, 1.0f); // Ending color (black with full opacity)

        Color edgyQuoteStartColor = new Color(1f, 1f, 1f, 0.0f);
        Color edgyQuoteEndColor = new Color(1f, 1f, 1f, 1.0f);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            panelImage.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            yield return null;
        }
        elapsedTime = 0f;
        duration = 10f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            EdgyQuote.color = Color.Lerp(edgyQuoteStartColor, edgyQuoteEndColor, elapsedTime / duration);
            yield return null;
        }
        elapsedTime = 0f;
        duration = 5f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            EdgyQuote.color = Color.Lerp(edgyQuoteEndColor, edgyQuoteStartColor, elapsedTime / duration);
            yield return null;
        }
        SceneManager.LoadScene("Main Menu");
        }
}

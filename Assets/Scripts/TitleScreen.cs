using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text playText;
    [SerializeField] private TMP_Text loreText;
    [SerializeField] private Image logo;
    [SerializeField] private Image bg;
    private float t = 0;
    private float factor = 1;
    private int turn = 1;
    private bool bounceable = true;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject tutorialHelp;
    private MusicPlayer theme;


    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<PlayerActions>().moveable = false;
        player.GetComponent<PlayerActions>().isActive = true;
        theme = GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && bounceable)
        {
            bounceable = false;
            playText.rectTransform.localScale = new Vector3(1.6f, 1.6f, 1);
            playText.rectTransform.localPosition = new Vector3(39, -338, 1);
            StartCoroutine(fade());
            theme.startGame(0.5f);
            Invoke("startGame", 0.5f);
        }
    }

    private void FixedUpdate()
    {
        if (bounceable)
        {
            bounce();
        }
    }

    private void bounce()
    {
        t = t + (0.04f * turn) * factor;
        factor = 1 - Mathf.Abs( 0.5f - t);

        if (t > 1)
        {
            turn = -turn;
        }
        else if (t < 0)
        {
            turn = -turn;
            t = 0;
        }
        playText.rectTransform.localScale = ((1 - t) * new Vector3(1.3f, 1.3f, 1)) + (t * new Vector3(1.4f, 1.4f, 1));
        playText.rectTransform.localPosition = ((1 - t) * new Vector3(39, -345, 1)) + (t * new Vector3(39, -341, 1));
    }

    private void startGame()
    {
        player.GetComponent<PlayerActions>().moveable = true;
        player.GetComponent<PlayerActions>().isActive = false;
        tutorialHelp.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    IEnumerator fade()
    {
        Color logoColor = logo.color;
        Color bgColor = bg.color;
        Color textColor = playText.color;
        Color loreTextColor = loreText.color;
        for (float i = 1; i < 11; i++)
        {
            playText.color = textColor - new Color(0, 0, 0, i / 5f);
            loreText.color = loreTextColor - new Color(0, 0, 0, i / 5f);
            logo.color = logoColor - new Color(0, 0, 0, i/10f);
            bg.color = bgColor - new Color(0, 0, 0, i/10f);
            yield return new WaitForSeconds(0.05f);
        }
        yield break;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{


    [SerializeField] private TMP_Text playText;
    private float t = 0;
    private float factor = 1;
    private int turn = 1;
    private bool bounceable = true;
    private bool gameStarted;
    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player.GetComponent<PlayerActions>().moveable = false;
        player.GetComponent<PlayerActions>().isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            bounceable = false;
            playText.rectTransform.localScale = new Vector3(1.6f, 1.6f, 1);
            playText.rectTransform.localPosition = new Vector3(39, -220, 1);
            Invoke("startGame", 0.5f);
        }
        else if(bounceable)
        {
            bounce();
        }
    }

    private void bounce()
    {
        t = t + (0.004f * turn) * factor;
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
        playText.rectTransform.localPosition = ((1 - t) * new Vector3(39, -228, 1)) + (t * new Vector3(39, -223, 1));
    }

    private void startGame()
    {
        player.GetComponent<PlayerActions>().moveable = true;
        player.GetComponent<PlayerActions>().isActive = false;
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class bossNameUI : MonoBehaviour
{
    public TMP_Text text;
    [SerializeField] private Image frame;
    [SerializeField] private Image frame2;
    [SerializeField] private Image bg;
    [HideInInspector]
    private int timeToGo = 0;
    [HideInInspector] public bool wait = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(fade(1));
    }

    // Update is called once per frame
    void Update()
    {
        if(timeToGo == 0)
        {
            StartCoroutine(fade(1));
        }
        if (timeToGo == 2)
        {
            timeToGo++;
            StartCoroutine( fade(-1));
        }
        else if(timeToGo > 4)
        {
            timeToGo = 0;
            gameObject.SetActive(false);
        }
    }
    IEnumerator fade(int turn)
    {
        timeToGo++;
        Color frameColor = frame.color;
        Color bgColor = bg.color;
        Color textColor = text.color;
        if(timeToGo == 1 && wait) yield return new WaitForSeconds(0.8f);
        for (float i = 1; i < 31; i++)
        {
            text.color = textColor + new Color(0, 0, 0, i / 30f) * turn;
            frame.color = frameColor + new Color(0, 0, 0, i / 30f) * turn;
            frame2.color = frameColor + new Color(0, 0, 0, i / 30f) * turn;
            bg.color = bgColor + new Color(0, 0, 0, i / 238f) * turn;
            yield return new WaitForSeconds(0.015f);
        }
        yield return new WaitForSeconds(1.3f);
        timeToGo++;
    }
}

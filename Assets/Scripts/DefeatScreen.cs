using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class DefeatScreen : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text defeatText;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(fade());
    }

    IEnumerator fade()
    {
        Color bgColor = background.color;
        Color textColor = defeatText.color;
        yield return new WaitForSeconds(2.0f);
        for (float i = 1; i < 41; i++)
        {

            background.color = bgColor + new Color(0, 0, 0, i / 40f);
            yield return new WaitForSeconds(0.02f);
        }
        for (float i = 1; i < 21; i++)
        {
            defeatText.color = textColor + new Color(0, 0, 0, i / 20f);
            yield return new WaitForSeconds(0.05f);
        }
        yield break;
    }
}

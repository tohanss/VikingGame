using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialText : MonoBehaviour
{
    [SerializeField] private TMP_Text tutorialText;
    [SerializeField] private GameObject tutorialRoom;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(changeText());
    }

    private void Update()
    {
        if (!tutorialRoom.gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator changeText()
    {
        yield return new WaitForSeconds(6.0f);
        tutorialText.text = "";
        yield return new WaitForSeconds(1.0f);
        tutorialText.text = "Use WASD to move";
        yield return new WaitForSeconds(4.0f);
        tutorialText.text = "";
        yield return new WaitForSeconds(1.0f);
        tutorialText.text = "Press left mouse button to shoot";
        yield return new WaitForSeconds(4.0f);
        tutorialText.text = "Try it out on the training dummies!";
        yield return new WaitForSeconds(3.0f);
        tutorialText.text = "";
        yield return new WaitForSeconds(1.0f);
        tutorialText.text = "Press right mouse button to use Shadowstep";
        yield return new WaitForSeconds(5.0f);
        tutorialText.text = "";
        yield return new WaitForSeconds(1.0f);
        tutorialText.text = "Press space button to dash";
        yield return new WaitForSeconds(5.0f);
        tutorialText.text = "Beware, abilities have cooldowns";
        yield return new WaitForSeconds(3.0f);
        tutorialText.text = "";
        yield return new WaitForSeconds(1.0f);
        tutorialText.text = "Active portals are blue, use them to proceed";
    }
}

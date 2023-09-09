using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEditor.UIElements;

public class SceneFader : MonoBehaviour
{
    public Image img;
    public AnimationCurve aCurve;

    void Start ()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeOutTo (string scene)
    {
        StartCoroutine(FadeOut(scene));
    }

    IEnumerator FadeIn ()
    {
        float opacity = 1f;

        while(opacity > 0f)
        {
            opacity -= Time.deltaTime;
            float a = aCurve.Evaluate(opacity);
            img.color = new Color (0f, 0f, 0f, a);

            //skip to the next frame
            yield return 0;
        }
    }

    IEnumerator FadeOut (string scene)
    {
        float opacity = 0f;

        while(opacity < 1f)
        {
            opacity += Time.deltaTime;
            float a = aCurve.Evaluate(opacity);
            img.color = new Color (0f, 0f, 0f, a);

            //skip to the next frame
            yield return 0;
        }

        SceneManager.LoadScene(scene);
    }

}

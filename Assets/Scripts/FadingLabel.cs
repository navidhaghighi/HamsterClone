using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FadingLabel : MonoBehaviour
{
    private const float speed = 50;
    [SerializeField]
    private MoveDirection direction;
    private CanvasGroup _canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeOut());
        StartCoroutine(LerpMove());
    }

    public void Init(int coinAmount)
    {
        GetComponent<TextMeshProUGUI>().text = coinAmount.ToString()+ "+";
    }


    private IEnumerator LerpMove()
    {
        while (true)
        {
            transform.position = new Vector3(transform.position.x + (direction.x*speed * Time.deltaTime), transform.position.y + (direction.y * speed * Time.deltaTime), transform.position.z);
            yield return null;
        }
    }

    /// <summary>
    /// fadeout canvas group
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeOut()
    {
        float alpha=1f;
        while (true)
        {
            alpha-= Time.deltaTime;
            GetComponent<TextMeshProUGUI>().color = new Color(GetComponent<TextMeshProUGUI>().color.r, GetComponent<TextMeshProUGUI>().color.g, GetComponent<TextMeshProUGUI>().color.b,alpha);
            yield return null;
        }
    }

}
[System.Serializable]
public class MoveDirection
{
    public float x;
    public float y;
}
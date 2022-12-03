using UnityEngine;
using System.Collections;

public class Msg : Singleton<Msg>
{
    [SerializeField] TMPro.TMP_Text txtMsg;

    private void Start()
    {
        txtMsg.CrossFadeAlpha(0, 0, false); // Text fade (Gone)
    }

    public void DisplayMsg(string msg, Color color)
    {
        txtMsg.color = color;
        txtMsg.text = msg;
        StartCoroutine(IEFadeTxt());
    }

    IEnumerator IEFadeTxt()
    {
        txtMsg.CrossFadeAlpha(1, 0.5f, false); // Text come 
        yield return new WaitForSeconds(3);
        txtMsg.CrossFadeAlpha(0, 0.5f, false); // Text fade (Gone)
    }
}

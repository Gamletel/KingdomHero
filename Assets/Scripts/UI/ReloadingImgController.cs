using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadingImgController: MonoBehaviour
{
    private Image _img;

    private void Awake()
    {
        _img = GetComponent<Image>();
        _img.fillAmount = 0;
    }

    public void OnFired(float reloadingTime)
    {
        StartCoroutine(ReloadingImgTimer(reloadingTime));
    }

    private IEnumerator ReloadingImgTimer(float reloadingTime)
    {
        float curReloadingTime = reloadingTime;
        while(curReloadingTime >= 0)
        {
            yield return new WaitForSeconds(0.1f);
            curReloadingTime -= .1f;
            _img.fillAmount = curReloadingTime / reloadingTime;
        }
    }
}

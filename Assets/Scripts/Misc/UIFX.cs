using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIFX : MonoBehaviour
{
    public Vector3 targetScale = new Vector3(1.1f, 1.1f, 1);
    public Vector3 oldScale = new Vector3(1, 1, 1);

    // Change these in INSPECTOR if needed

    public void MakeBigger()
    {
        if (transform != null)
        {
            transform.DOKill();
            float rotationNum = Random.Range(-3f, 3f);
            transform.DORotate(new Vector3(0f, 0f, rotationNum), .5f).SetUpdate(true);
            transform.DOScale(targetScale, .5f).SetUpdate(true);
        }
    }
    public void MakeSmaller()
    {
        if (transform != null)
        {
            transform.DOKill();
            transform.DORotate(new Vector3(0f, 0f, 0f), .5f).SetUpdate(true);
            transform.DOScale(oldScale, .5f).SetUpdate(true);
        }

    }

    void OnDisable()
    {
        transform.DOKill();
        transform.localScale = oldScale;
        transform.localRotation = Quaternion.identity;
    }

}

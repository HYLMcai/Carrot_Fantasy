using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bird : MonoBehaviour
{
    public float offSetY = 20f;
    public float duration = 2f;

    private Tweener tweener;
    // Start is called before the first frame update
    void Start()
    {
        tweener = this.transform.DOLocalMoveY(this.transform.localPosition.y + offSetY, duration);
        tweener.SetEase(Ease.InOutSine);
        tweener.SetLoops(-1, LoopType.Yoyo);
    }
    private void OnDestroy()
    {
        if (tweener != null) tweener.Kill();
    }
}

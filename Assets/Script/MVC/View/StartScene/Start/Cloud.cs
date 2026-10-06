using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Cloud : MonoBehaviour
{
    public float offSet = 1000f;
    public float duration = 10f;

    private Tweener tweener;
    // Start is called before the first frame update
    void Start()
    {
        tweener = this.transform.DOLocalMoveX(this.transform.position.x + offSet, duration).SetEase(Ease.OutSine).SetLoops(-1, LoopType.Restart);
    }
    private void OnDestroy()
    {
        if (tweener != null) tweener.Kill();
    }
}

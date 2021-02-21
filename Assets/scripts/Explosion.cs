using System;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public Action OnExplosionComplete;

    public void Explode()
    {
        transform.DOScale(2, .5f);
        GetComponent<SpriteRenderer>().DOColor(Color.red, .5f).OnComplete(() =>
        {
            Destroy(gameObject);
            OnExplosionComplete?.Invoke();
        });
    }
}
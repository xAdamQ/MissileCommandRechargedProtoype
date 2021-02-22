using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class MyMissile : Missile
{
    private GameObject ExpMark;
    [SerializeField] private GameObject ExpMarkPrefab;

    private bool Exploded;

    public void Shoot(Vector2 targetPos)
    {
        ExpMark = Instantiate(ExpMarkPrefab, targetPos, Quaternion.identity);

        var dir = (targetPos - (Vector2) transform.position).normalized;

        // Debug.Log(dir);

        GetComponent<Rigidbody2D>().AddForce(dir * GameData.I.Speed);

        StartCoroutine(checkPos());

        IEnumerator checkPos()
        {
            while (Vector2.Distance(transform.position, targetPos) > .1f)
            {
                yield return new WaitForFixedUpdate();
            }

            if (!Exploded) Explode();
        }
    }

    public override void Destroy()
    {
        Destroy(gameObject);
    }

    private void Explode()
    {
        Exploded = true;
        Destroy(ExpMark);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.DOScale(GameData.I.Power, .5f).OnComplete(() => { Destroy(); });
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<EvilMissile>() || collision.collider.GetComponent<Spaceship>())
        {
            if (!Exploded) Explode();

            collision.collider.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            collision.collider.GetComponent<Destroyable>().Destroy();
        }
    }
}
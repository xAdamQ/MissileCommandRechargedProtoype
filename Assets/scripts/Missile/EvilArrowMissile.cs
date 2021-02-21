using System.Collections;
using System.Collections.Generic;
using MiscUtil.Xml.Linq.Extensions;
using UnityEngine;

public class EvilArrowMissile : EvilMissile
{
    //these should be increasing rate
    private const float EMissilesRate = 1;
    private const float EMissilesForce = 250;

    public void Shoot()
    {
        var startPoz = new Vector2(Random.Range(-Game.CameraSize, Game.CameraSize), 6);
        var endPoz = Game.I.AllBuildingsPostions[Random.Range(0, Game.I.AllBuildingsPostions.Length - 1)];
        endPoz.x += Random.Range(-.5f, .5f);

        var angle = (endPoz - startPoz).normalized;

        transform.position = startPoz;
        //this peace of code is important to gameplay and can be improved much more

        GetComponent<Rigidbody2D>().AddForce(angle.normalized * EMissilesForce);

        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(6);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Building>())
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.collider.GetComponent<Destroyable>().Destroy();
            Destroy();
        }
    }
}
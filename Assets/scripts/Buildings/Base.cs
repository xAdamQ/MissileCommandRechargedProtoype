using UnityEngine;
using DG.Tweening;

public class Base : Building
{
    [HideInInspector] public bool Ready;
    [SerializeField] private GameObject ReloadIndicator;
    [SerializeField] private GameObject MyMissilePrefab;

    private bool Built;

    private void Start()
    {
        Ready = true;
        Built = true;
    }

    public override void Destroy()
    {
        var exp = Instantiate(ExplosionPrefab, transform).GetComponent<Explosion>();
        // exp.OnExplosionComplete += () =>
        Rebuild();
        exp.Explode();
    }

    private void Rebuild()
    {
        Built = false;
        GetComponent<SpriteRenderer>().color = Color.white;

        // await UniTask.Delay(GameData.I.BaseRebuildTime);

        transform.DOScaleY(0, GameData.I.BaseRebuildTime).OnComplete(() =>
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
            Built = true;
        }).From();
    }

    private void Reload()
    {
        Ready = false;
        ReloadIndicator.GetComponent<SpriteRenderer>().color = Color.white;

        ReloadIndicator.transform.DOScaleY(0, GameData.I.BaseRebuildTime).OnComplete(() =>
        {
            ReloadIndicator.GetComponent<SpriteRenderer>().color = Color.yellow;
            Ready = true;
        }).From();

        // var timeStep = .05f;
        // var frames = GameData.I.BaseReloadTime / timeStep;
        // var incUnit = 1 / frames;
        // for (int i = 0; i < frames; i++)
        // {
        //     ReloadIndicator.transform.localScale += Vector3.up * incUnit;
        //     yield return new WaitForSeconds(timeStep);
        // }
    }

    public void Shoot(Vector2 targetPos)
    {
        if (!Ready || !Built) return;

        Reload();

        var missile = Instantiate(MyMissilePrefab, transform.position, Quaternion.identity).GetComponent<MyMissile>();
        missile.Shoot(targetPos);
    }
}
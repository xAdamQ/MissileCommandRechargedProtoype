using UnityEngine;

public abstract class Destroyable : MonoBehaviour
{
    [SerializeField] protected GameObject ExplosionPrefab;

    public virtual void Destroy()
    {
        var exp = Instantiate(ExplosionPrefab, transform).GetComponent<Explosion>();
        exp.OnExplosionComplete += () => Destroy(gameObject);
        exp.Explode();
    }
}
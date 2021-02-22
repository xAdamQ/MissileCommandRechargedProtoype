using System.Collections;
using UnityEngine;
using System.Linq;

public class Game : MonoBehaviour
{
    public static Game I;

    public Vector2[] AllBuildingsPostions;

    private void Awake()
    {
        I = this;

        AllBuildingsPostions = FindObjectsOfType<Building>().Select(b => (Vector2) b.transform.position).ToArray();
    }

    private void Start()
    {
        GameData.I = new GameData
        {
            Speed = 300,
            BaseRebuildTime = 2,
            BaseReloadTime = 1.5f,
            Power = 1
        };

        StartCoroutine(EvilMissilesFactory());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BaseShoot();
        }
    }

    [SerializeField] private Base[] Bases;

    private void BaseShoot()
    {
        var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Bases = Bases.OrderBy(b => Vector2.Distance(targetPos, b.transform.position)).ToArray();

        for (int i = 0; i < Bases.Length; i++)
        {
            if (Bases[i].Ready)
            {
                Bases[i].Shoot(targetPos);
                break;
            }
        }
    }

    public const float CameraSize = 5;

    //these should be increasing rate
    private const float EMissilesRate = 2;
    private const float EMissilesForce = 250;
    [SerializeField] private GameObject ArrowEvilMissilePrefab;

    private IEnumerator EvilMissilesFactory()
    {
        while (true)
        {
            var missile = Instantiate(ArrowEvilMissilePrefab).GetComponent<EvilArrowMissile>();
            missile.Shoot();

            yield return new WaitForSeconds(EMissilesRate);
        }
    }
}
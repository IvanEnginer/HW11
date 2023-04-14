using UnityEngine;

public class Barrel : PassiveItems
{
    [SerializeField] private GameObject _barrelExplosion;

    private int _effectCorner = -90;

    public override void OnAffect()
    {
        base.OnAffect();

        Die();
    }

    [ContextMenu("Die")]
    private void Die()
    {
        Instantiate(_barrelExplosion, transform.position, Quaternion.Euler(_effectCorner, 0, 0));
        Destroy(gameObject);
    }    
}

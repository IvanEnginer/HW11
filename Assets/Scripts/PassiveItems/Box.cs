using UnityEngine;

public class Box : PassiveItems
{
    [Range(0, 2)]
    public int Health = 1;

    [SerializeField] private GameObject[] _levels;
    [SerializeField] private GameObject _breakEffectPrefabs;
    [SerializeField] private Animator _animator;

    private int _effectCorner = -90;

    private void Start()
    {
        SetHealth(Health);
    }

    [ContextMenu("OnAffect")]
    public override void OnAffect()
    {
        base.OnAffect();

        Health -= 1;
        Instantiate(_breakEffectPrefabs, transform.position, Quaternion.Euler(_effectCorner, 0f, 0f));
        _animator.SetTrigger("Shake");

        if(Health < 0)
        {
            Die();
        }
        else
        {
            SetHealth(Health);
        }
    }

    private void SetHealth(int value)
    {
        for(int i = 0; i < _levels.Length; i++)
        {
            _levels[i].SetActive(1 <= value);
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}

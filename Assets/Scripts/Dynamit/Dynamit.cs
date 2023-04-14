using System.Collections;
using UnityEngine;

public class Dynamit : ActiveItem
{
    [Header("Dynamit")]
    [SerializeField] private float _affectRadius = 1.5f;
    [SerializeField] private float _forceValue = 1000f;

    [SerializeField] private GameObject _affectArea;
    [SerializeField] private GameObject _effectPrefabs;

    private float _durationAffect = 1f;

    private float _extentionRadius = 2f;

    private float _extentionForse = 0.5f;

    protected override void Start()
    {
        base.Start();

        _affectArea.SetActive(false);
    }

    public override void DoEffect()
    {
        base.DoEffect();

        StartCoroutine(AffectProcess());
    }

    private IEnumerator AffectProcess()
    {
        _affectArea.SetActive(true);
        _animator.enabled = true;
        yield return new WaitForSeconds(_durationAffect);

        Collider[] colliders = Physics.OverlapSphere(transform.position, _affectRadius);
        for (int i = 0;i < colliders.Length; i++)
        {
            Rigidbody rigidbody = colliders[i].attachedRigidbody;

            if(rigidbody)
            {
                Vector3 fromTo = (rigidbody.transform.position - transform.position).normalized;
                rigidbody.AddForce(fromTo * _forceValue + Vector3.up * _forceValue * _extentionForse);

                PassiveItems passiveItems = rigidbody.GetComponent<PassiveItems>();

                if (passiveItems)
                {
                    passiveItems.OnAffect();
                }
            }
        }

        Instantiate(_effectPrefabs, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnValidate()
    {
        _affectArea.transform.localScale = Vector3.one * _affectRadius * _extentionRadius;
    }
}

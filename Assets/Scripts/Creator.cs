using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Creator : MonoBehaviour
{
    [SerializeField] private Transform _tube;
    [SerializeField] private Transform _spawner;
    [SerializeField] private ActiveItem _ballPrefab;

    [SerializeField] private Transform _rayTransform;
    [SerializeField] private LayerMask _layerMask;

    private ActiveItem _itemInTube;
    private ActiveItem _itemInSpawner;

    private int _minRange = 0;
    private int _maxRange = 5;

    private float _boardMoveTime = 1f;
    private float _moveTime = 0.45f;

    private float _xScaleRay = 2f;
    private float _zScaleRay = 1f;
    private int _maxDistanseRay = 100;

    private void Start()
    {
        CreateItemInTube();
        StartCoroutine(MoveToSpawner());
    }

    private void LateUpdate()
    {
        if (_itemInSpawner)
        {
            Ray ray = new Ray(_spawner.position, Vector3.down);
            RaycastHit hit;

            if(Physics.SphereCast(ray, _itemInSpawner.Radius, out hit, _maxDistanseRay, _layerMask, QueryTriggerInteraction.Ignore))
            {
                _rayTransform.localScale = new Vector3(_itemInSpawner.Radius * _xScaleRay, hit.distance, _zScaleRay);
                _itemInSpawner.Projection.SetPosition(_spawner.position + Vector3.down * hit.distance);
            }

            if (Input.GetMouseButtonUp(0))
            {
                Drop();
            }
        }
    }

    void CreateItemInTube()
    {
        int itemLevel = Random.Range(_minRange, _maxRange);
        _itemInTube = Instantiate(_ballPrefab, _tube.position, Quaternion.identity);
        _itemInTube.SetLevel(itemLevel);
        _itemInTube.SetupToTube();
    }

    private IEnumerator MoveToSpawner()
    {
        _itemInTube.transform.parent = _spawner;

        for(float t = 0; t < _boardMoveTime; t += Time.deltaTime / _moveTime)
        {
            _itemInTube.transform.position = Vector3.Lerp(_tube.position, _spawner.position, t);
            yield return null;
        }

        _itemInTube.transform.localPosition = Vector3.zero;
        _itemInSpawner = _itemInTube;
        _rayTransform.gameObject.SetActive(true);
        _itemInSpawner.Projection.Show();
        _itemInTube = null;
        CreateItemInTube();
    }

    private void Drop()
    {
        _itemInSpawner.Drop();
        _itemInSpawner.Projection.Hide();

        _itemInSpawner = null;

        _rayTransform.gameObject.SetActive(false);

        if (_itemInTube)
        {
            StartCoroutine(MoveToSpawner());    
        }
    }
}

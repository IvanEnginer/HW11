using UnityEngine;

public class Ball : ActiveItem
{
    [SerializeField] private BallSetings _ballSetings;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Transform _visualTransform;

    private float _extentionRadiuse = 0.1f;
    private float _minRadiuse = 0.4f;
    private float _maxRadiuse = 0.7f;
    private float _stepsRadiuse = 10f;
    private float _multipleScale = 2f;

    public override void SetLevel(int level)
    {
        base.SetLevel(level);

        _renderer.material = _ballSetings.BallMatterials[level];

        Radius = Mathf.Lerp(_minRadiuse, _maxRadiuse, level / _stepsRadiuse);

        Vector3 ballScale = Vector3.one * Radius * _multipleScale;

        _visualTransform.localScale = ballScale;
        _collider.radius = Radius;
        _trigger.radius = Radius + _extentionRadiuse;

        Projection.Setup(_ballSetings.BallProhectionMaterials[level], _levelText.text, Radius);
    }

    public override void DoEffect()
    {
        base.DoEffect();
        IncreaseLevel();
    }
}

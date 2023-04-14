using TMPro;
using UnityEngine;

public class Projection : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Transform _visualTransform;

    private float _multipleRadiuse = 2f;

    public void Setup(Material material, string numberText, float radiuse)
    {
        _renderer.material = material;
        _text.text = numberText;
        _visualTransform.localScale = Vector3.one * radiuse * _multipleRadiuse;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class ActiveItem : Item
{
    public int Level;
    public float Radius;

    public Rigidbody Rigidbody;

    public Projection Projection;

    public bool IsDead;

    [SerializeField] protected TMP_Text _levelText; 


    [SerializeField] protected SphereCollider _collider;
    [SerializeField] protected SphereCollider _trigger;

    [SerializeField] protected Animator _animator;

    private int _fundationLevelNumber = 2;
    private int _LavelUp = 1;

    private float _dropSpeed = 1.2f;

    private float _timeCollapsed = 0.08f;

    protected virtual void Start()
    {
        Projection.Hide();
    }

    [ContextMenu("IncreaseLevel")]
    public void IncreaseLevel()
    {
        Level++;
        SetLevel(Level);
        _animator.SetTrigger("IncreaseLevel");

        _trigger.enabled = false;
        Invoke(nameof(EnableTrigger), _timeCollapsed);
    }

    public virtual void SetLevel(int level)
    {
        Level = level;

        int number = (int)Mathf.Pow(_fundationLevelNumber, level + _LavelUp);
        string numberString = number.ToString();
 
        _levelText.text = numberString;
    }

    public void SetupToTube()
    {
        _trigger.enabled = false;
        _collider.enabled = false;
        Rigidbody.isKinematic = true;
        Rigidbody.interpolation = RigidbodyInterpolation.None;
    }

    public void Drop()
    {
        _trigger.enabled = true;
        _collider.enabled = true;
        Rigidbody.isKinematic = false;
        Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

        transform.parent = null;

        Rigidbody.velocity = Vector3.down * _dropSpeed;
    }

    public void Disable()
    {
        _trigger.enabled = false;
        Rigidbody.isKinematic = true;
        _collider.enabled = false;
        IsDead = true;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public virtual void DoEffect()
    {

    }

    private void EnableTrigger()
    {
        _trigger.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(IsDead) return;

        if (other.attachedRigidbody)
        {
            ActiveItem otherItem = other.attachedRigidbody.GetComponent<ActiveItem>();
            if (otherItem)
            {
                if(!otherItem.IsDead && Level == otherItem.Level)
                {
                    CollapseManeger.Instance.Collapse(this, otherItem);
                }
            }
        }
    }
}

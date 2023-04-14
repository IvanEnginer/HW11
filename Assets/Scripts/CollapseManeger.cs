using System.Collections;
using UnityEngine;

public class CollapseManeger : MonoBehaviour
{
    public static CollapseManeger Instance;

    private float _boardMoveTime = 1f;
    private float _moveTime = 0.08f;

    private float _additionRadiuse = 0.1f;

    private float _boardAbs = 0.02f;

    private void Awake()
    {
        Instance = this;
    }

    public void Collapse(ActiveItem itemA, ActiveItem itemB)
    {
        ActiveItem toItem;
        ActiveItem fromItem;

        if(Mathf.Abs(itemA.transform.position.y - itemB.transform.position.y) > _boardAbs)
        {
            if(itemA.transform.position.y > itemB.transform.position.y)
            {
                fromItem= itemA;
                toItem= itemB;
            }
            else
            {
                fromItem = itemB; 
                toItem= itemA;
            }
        }
        else
        {
            if(itemA.Rigidbody.velocity.magnitude > itemB.Rigidbody.velocity.magnitude)
            {
                fromItem = itemA;
                toItem = itemB;
            }
            else
            {
                fromItem = itemB;
                toItem = itemA;
            }
        }

        StartCoroutine(CollapseProcess(itemA, itemB));
    }

    public IEnumerator CollapseProcess(ActiveItem fromItem, ActiveItem toItem)
    {
        fromItem.Disable();

        if(fromItem.ItemType == ItemType.Ball || toItem.ItemType == ItemType.Ball)
        {
            Vector3 stratPosition = fromItem.transform.position;

            for (float t = 0f; t < _boardMoveTime; t += Time.deltaTime / _moveTime)
            {
                fromItem.transform.position = Vector3.Lerp(stratPosition, toItem.transform.position, t);
                yield return null;
            }
        }

        fromItem.transform.position = toItem.transform.position;

        if(fromItem.ItemType == ItemType.Ball && toItem.ItemType == ItemType.Ball)
        {
            fromItem.Die();
            toItem.DoEffect();
            ExplodeBall(toItem.transform.position, toItem.Radius + _additionRadiuse);
        }
        else
        {
            if(fromItem.ItemType == ItemType.Ball)
            {
                fromItem.Die();
            }
            else
            {
                fromItem.DoEffect();
            }

            if(toItem.ItemType == ItemType.Ball)
            {
                toItem.Die();
            }
            else
            {
                toItem.DoEffect();
            }
        }
    }

    public void ExplodeBall(Vector3 position, float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius);

        for(int i =0; i < colliders.Length; i++)
        {
            PassiveItems passiveItem = colliders[i].GetComponent<PassiveItems>();

            if (colliders[i].attachedRigidbody)
            {
                passiveItem = colliders[i].attachedRigidbody.GetComponent<PassiveItems>();
            }

            if(passiveItem)
            {
                passiveItem.OnAffect();
            }
        }
    }
}

using UnityEngine;

public class InventorySlotUI : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void AddItem()
    {
        _animator.Play(MyAnimations.WithItem.ToString());
    }

    public void ChangeAnimationOnItemOver(bool isItemOver)
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(MyAnimations.WithItem.ToString())) return;

        if (isItemOver)
        {
            _animator.Play(MyAnimations.ItemOver.ToString());
        }
        else
        {
            _animator.Play(MyAnimations.Idle.ToString());
        }
    }

    private enum MyAnimations
    {
        Idle,
        ItemOver,
        WithItem
    }
}

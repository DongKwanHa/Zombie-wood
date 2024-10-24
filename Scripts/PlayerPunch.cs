using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    public Punch Punch;
    //public Transform gunPivot; // �� ��ġ�� ������


    private PlayerInput playerInput; // �÷��̾��� �Է�
    private Animator playerAnimator; // �ִϸ����� ������Ʈ

    private void Start()
    {
        // ����� ������Ʈ���� ��������
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
    }

    //private void OnEnable()
    //{
    //    // ���Ͱ� Ȱ��ȭ�� �� �ѵ� �Բ� Ȱ��ȭ
    //    gun.gameObject.SetActive(true);
    //}

    //private void OnDisable()
    //{
    //    // ���Ͱ� ��Ȱ��ȭ�� �� �ѵ� �Բ� ��Ȱ��ȭ
    //    gun.gameObject.SetActive(false);
    //}

    private void Update()
    {
        // �Է��� �����ϰ� �� �߻��ϰų� ������
        if (playerInput.Punch)
        {
            Punch.FirePunch();
        }
        playerAnimator.SetTrigger("Melee");
    }

    // ź�� UI ����
   

    // �ִϸ������� IK ����
    //private void OnAnimatorIK(int layerIndex)
    //{
    //    gunPivot.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

    //    playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
    //    playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);

    //    playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
    //    playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);

    //    playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
    //    playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);

    //    playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
    //    playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
    //}
}

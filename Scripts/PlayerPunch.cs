using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    public Punch Punch;
    //public Transform gunPivot; // 총 배치의 기준점


    private PlayerInput playerInput; // 플레이어의 입력
    private Animator playerAnimator; // 애니메이터 컴포넌트

    private void Start()
    {
        // 사용할 컴포넌트들을 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
    }

    //private void OnEnable()
    //{
    //    // 슈터가 활성화될 때 총도 함께 활성화
    //    gun.gameObject.SetActive(true);
    //}

    //private void OnDisable()
    //{
    //    // 슈터가 비활성화될 때 총도 함께 비활성화
    //    gun.gameObject.SetActive(false);
    //}

    private void Update()
    {
        // 입력을 감지하고 총 발사하거나 재장전
        if (playerInput.Punch)
        {
            Punch.FirePunch();
        }
        playerAnimator.SetTrigger("Melee");
    }

    // 탄약 UI 갱신
   

    // 애니메이터의 IK 갱신
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

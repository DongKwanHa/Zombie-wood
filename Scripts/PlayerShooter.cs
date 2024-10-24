using System.Collections;
using UnityEngine;

// 주어진 Gun 오브젝트를 쏘거나 재장전
// 알맞은 애니메이션을 재생하고 IK를 사용해 캐릭터 양손이 총에 위치하도록 조정
public class PlayerShooter : MonoBehaviour 
{
    public Gun gun; // 사용할 총
    public Punch Punch;
    public Transform gunPivot; // 총 배치의 기준점
    public Transform leftHandMount; // 총의 왼쪽 손잡이, 왼손이 위치할 지점
    public Transform rightHandMount; // 총의 오른쪽 손잡이, 오른손이 위치할 지점

    public Transform Punch_Destination; //왼쪽손 펀치시 위치

    private PlayerInput playerInput; // 플레이어의 입력
    private Animator playerAnimator; // 애니메이터 컴포넌트

    bool isidle = true;

    private void Start() 
    {
        // 사용할 컴포넌트들을 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerAnimator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // 슈터가 활성화될 때 총도 함께 활성화
        gun.gameObject.SetActive(true);
    }
    
    private void OnDisable()
    {
        // 슈터가 비활성화될 때 총도 함께 비활성화
        gun.gameObject.SetActive(false);
    }

    private void Update() 
    {
        // 입력을 감지하고 총 발사하거나 재장전
        if(playerInput.fire)
        {
            gun.Fire();
        }
        else if(playerInput.reload) 
        {
            if(gun.Reload() ) 
            {
                playerAnimator.SetTrigger("Reload");
            }
        }

        UpdateUI();

        //if(playerInput.Punch)
        //{
        //    Punch.FirePunch();


        //    playerAnimator.SetTrigger("Melee");
        //}

        if (playerInput.Punch)
        {
            StartCoroutine(HandlePunchCoroutine());
        }
    }

    // 탄약 UI 갱신
    private void UpdateUI()
    {
        if (gun != null && UIManager.instance != null)
        {
            // UI 매니저의 탄약 텍스트에 탄창의 탄약과 남은 전체 탄약을 표시
            UIManager.instance.UpdateAmmoText(gun.magAmmo, gun.ammoRemain);
        }
    }

    private IEnumerator HandlePunchCoroutine()
    {
        // 0.1초 정도 딜레이를 줘서 펀치 전환이 자연스럽게 보이게 함
        yield return new WaitForSeconds(0.1f);

        // 펀치 애니메이션을 트리거
        playerAnimator.SetTrigger("Melee");

        // 펀치 애니메이션 재생 중에는 왼손 IK를 해제 (천천히 0으로 줄임)
        float elapsedTime = 0f;
        float duration = 0.5f; // 0.5초에 걸쳐 IK 해제

        while (elapsedTime < duration)
        {
            // 시간 경과에 따라 왼손의 IK 값을 서서히 0으로 줄임
            float weight = Mathf.Lerp(1.0f, 0.0f, elapsedTime / duration);
            playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
            playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, weight);

            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 최종적으로 왼손의 IK를 완전히 해제
        playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0.0f);
        playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0.0f);

        // 펀치 동작을 실행
        Punch.FirePunch();
    }

    // 애니메이터의 IK 갱신
    private void OnAnimatorIK(int layerIndex)
    {
        // 기본적으로 오른손과 왼손이 총을 잡고 있게 설정
        if (!playerInput.Punch)
        {
            gunPivot.position = playerAnimator.GetIKHintPosition(AvatarIKHint.RightElbow);

            // 왼손 IK 설정
            playerAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1.0f);
            playerAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1.0f);
            playerAnimator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandMount.position);
            playerAnimator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandMount.rotation);

            // 오른손 IK 설정
            playerAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1.0f);
            playerAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1.0f);
            playerAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
            playerAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);
        }
    }
}
using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMovement : MonoBehaviour 
{
    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float rotateSpeed = 180f; // 좌우 회전 속도
    public float mouseSensitivity = 100f;
    public GameObject MoveFlag;

    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터

    private float mouseXInput;

    private void Start() 
    {
        // 사용할 컴포넌트들의 참조를 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        Cursor.lockState=CursorLockMode.Locked;
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    private void FixedUpdate()
    {
        // 물리 갱신 주기마다 움직임, 회전, 애니메이션 처리 실행
        MouseRotation();

       
        //VerticalMove();
        //HorizontalMove();

        MoveCharacter();

        playerAnimator.SetFloat("VerticalMove", playerInput.move);
        playerAnimator.SetFloat("HorizontalMove", playerInput.rotate);
      
    }

    //// 입력값에 따라 캐릭터를 앞뒤로 움직임
    //private void VerticalMove() 
    //{
    //    //if(playerInput.move !=0)
    //    //{
    //    //    Vector3 VmoveDistance = playerInput.move*transform.forward*moveSpeed * Time.deltaTime;
    //    //    playerRigidbody.MovePosition(playerRigidbody.position + VmoveDistance);
    //    //}

    //    if (playerInput.move != 0)
    //    {
    //        Vector3 moveDirection = transform.forward * playerInput.move * moveSpeed * Time.deltaTime;
    //        playerRigidbody.MovePosition(playerRigidbody.position + moveDirection);
    //    }

    //    //playerAnimator.SetFloat("VerticalMove", playerInput.move);

    //}
    ////캐릭터 좌우 옴직임
    //private void HorizontalMove()
    //{

    //    //Vector3 HmoveDistance = playerInput.move * transform.right * moveSpeed * Time.deltaTime;


    //    //playerRigidbody.MovePosition(playerRigidbody.position + HmoveDistance);

    //    //if(playerInput.rotate !=0)
    //    //{
    //    //    Vector3 turn = playerInput.rotate * transform.right *moveSpeed* Time.deltaTime;

    //    //    playerRigidbody.MovePosition(playerRigidbody.position + turn);
    //    //}

    //    if (playerInput.rotate != 0)
    //    {
    //        Vector3 turn = transform.right * playerInput.move * moveSpeed * Time.deltaTime;
    //        playerRigidbody.MovePosition(playerRigidbody.position + turn);
    //    }


    //    //playerAnimator.SetFloat("HorizontalMove", playerInput.rotate);
    //    if (playerInput.rotate != 0)
    //    {
    //        Debug.Log("작동");
    //    }

    //}

    private void MoveCharacter()
    {
        // 앞뒤 움직임 처리 (transform.forward를 이용하여 로컬 좌표 기준으로 이동)
        if (playerInput.move != 0)
        {
            Vector3 moveDirection = MoveFlag.transform.forward * playerInput.move * moveSpeed * Time.deltaTime;
            playerRigidbody.MovePosition(playerRigidbody.position + moveDirection);
        }

        // 좌우 움직임 처리 (transform.right를 이용하여 로컬 좌표 기준으로 이동)
        if (playerInput.rotate != 0)
        {
            Vector3 moveDirection =  playerInput.rotate* MoveFlag.transform.right* moveSpeed * Time.deltaTime;
            playerRigidbody.MovePosition(playerRigidbody.position + moveDirection);
        }
    }

    //입력값에 따라 캐릭터를 좌우로 회전
    private void MouseRotation()
    {
       mouseXInput=Input.GetAxis("Mouse X")*mouseSensitivity*Time.deltaTime;

        Vector3 Mouserotate = new Vector3(0f, mouseXInput, 0f);
        playerRigidbody.MoveRotation(playerRigidbody.rotation*Quaternion.Euler(Mouserotate));
    }
}
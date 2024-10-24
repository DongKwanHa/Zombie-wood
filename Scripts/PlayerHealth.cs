using System.Collections;
using UnityEngine;
using UnityEngine.UI; // UI 관련 코드

// 플레이어 캐릭터의 생명체로서의 동작을 담당
public class PlayerHealth : LivingEntity
{
    public Slider healthSlider; // 체력을 표시할 UI 슬라이더
    public Slider ExpSlider;//경험치를 표시할 UI
    public GameObject Level_Object;//레벨표시
    
  
    
    private float level = 1;
    private float maxExp = 5f;

   
  
    public Text Level_Text;

    public AudioClip deathClip; // 사망 소리
    public AudioClip hitClip; // 피격 소리
    public AudioClip itemPickupClip; // 아이템 습득 소리
    public AudioClip ExpGetSound;//경험치 획득 소리

    private AudioSource playerAudioPlayer; // 플레이어 소리 재생기
    private Animator playerAnimator; // 플레이어의 애니메이터

    private PlayerMovement playerMovement; // 플레이어 움직임 컴포넌트
    private PlayerShooter playerShooter; // 플레이어 슈터 컴포넌트

    bool isPause;

    public GameObject LevelUp_Category;
    private GunData PowerUp;
    private PlayerMovement SpeedUp;
    private LivingEntity VitalUp;
    public Slider SelectTIme;

    private void Awake()
    {
        // 사용할 컴포넌트를 가져오기
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAudioPlayer = GetComponent<AudioSource>();
        playerShooter = GetComponent<PlayerShooter>();
        if(Level_Object != null )
        {
            Level_Text = Level_Object.GetComponentInChildren<Text>();
        }
        SelectTIme.maxValue = 30;
        SelectTIme.minValue = 0;
        SelectTIme.value=SelectTIme.maxValue;
        isPause = false;
        Level_Object.SetActive(false);
    }

    protected override void OnEnable()
    {
        // LivingEntity의 OnEnable() 실행 (상태 초기화)
        base.OnEnable();

        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = startingHealth;
        healthSlider.value = health;

        playerMovement.enabled = true;
        playerShooter.enabled = true;

        ExpSlider.maxValue = maxExp;
        ExpSlider.value = exp;
    }

    // 체력 회복
    public override void RestoreHealth(float newHealth)
    {
        // LivingEntity의 RestoreHealth() 실행 (체력 증가)
        base.RestoreHealth(newHealth);

        healthSlider.value = health;
    }

    public override void GetExpBead(float ValueExp)
    {
        // 경험치를 증가시키고, ExpSlider에 반영
        base.GetExpBead(ValueExp);

        ExpSlider.value = exp; // 경험치를 UI 슬라이더에 반영

        // 만약 경험치가 maxExp 이상이면 초기화
        if (exp >= maxExp)
        {
            exp = 0f; // 경험치 초기화
            ExpSlider.value = exp;

            // 레벨 증가 및 경험치 슬라이더 최대값 증가
            level++;
            maxExp *= 2;
            ExpSlider.maxValue = maxExp;

            // 레벨업 UI를 표시하는 코루틴 실행
            StartCoroutine(LevelUpUI());

        }
    }

    private IEnumerator LevelUpUI()
    {
        // 레벨업 UI 표시
        // 레벨업 UI 숨김
        Level_Object.SetActive(true);
        if (isPause == false)
        {
            Time.timeScale = 0;
            isPause = true;
            LevelUp_Category.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; // 커서 움직임 허용

        }
        if(LevelUp_Category != null)
        {
               
            SelectTIme.value--;
        }

        if(SelectTIme.value == 0)
        {
            LevelUp_Category.SetActive(false);
            Time.timeScale = 1;
            isPause = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; // 커서 잠금
        }
       
        
        // 0.2초 대기
        yield return new WaitForSeconds(0.2f);
        Level_Object.SetActive(false);
        Level_Text.text = "Level " + level; // 레벨 텍스트 업데이트
       
    }

    public void ClickPowerUp()
    {
        PowerUp = FindObjectOfType<GunData>();
        if(PowerUp != null&&!dead)
        {
            PowerUp.damage++;
        }
        else if (dead)
        {
            PowerUp.damage = 25;
        }
        LevelUp_Category.SetActive(false);
        Time.timeScale = 1;
        isPause=false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; // 커서 잠금
    }

    public void ClickSpeedUp()
    {
        SpeedUp = FindObjectOfType<PlayerMovement>();
        if (SpeedUp != null && !dead)
        {
           SpeedUp.moveSpeed+=1f;
        }
        else if (dead)
        {
            SpeedUp.moveSpeed = 5f;
        }
        LevelUp_Category.SetActive(false);
        Time.timeScale = 1;
        isPause = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; // 커서 잠금
    }

    public void ClickVitalUp()
    {
        VitalUp = FindObjectOfType<LivingEntity>();
        if (VitalUp != null && !dead)
        {
            VitalUp.startingHealth +=10;
        }
        else if (dead)
        {
            VitalUp.startingHealth = 100;
        }
        LevelUp_Category.SetActive(false);
        Time.timeScale = 1;
        isPause = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; // 커서 잠금
    }

    // 데미지 처리
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if(!dead)
        {
            //playerAudioPlayer.PlayOneShot(hitClip);
        }

        // LivingEntity의 OnDamage() 실행(데미지 적용)
        base.OnDamage(damage, hitPoint, hitDirection);

        healthSlider.value = health;
    }

    // 사망 처리
    public override void Die()
    {
        // LivingEntity의 Die() 실행(사망 적용)
        base.Die();

        healthSlider.gameObject.SetActive(false);

        //playerAudioPlayer.PlayOneShot(deathClip);
        playerAnimator.SetTrigger("Die");

        playerMovement.enabled = false;
        playerShooter.enabled=false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 아이템과 충돌한 경우 해당 아이템을 사용하는 처리
        IItem item = other.GetComponent<IItem>();
        if (item != null)
        {
            item.Use(gameObject);
            //playerAudioPlayer.PlayOneShot(itemPickupClip);
        }
      
        if(other.CompareTag("ExpBead"))
        {
            GetExpBead(1f);//1의 경험치 획득
            Destroy(other.gameObject);//구슬 파괴
            //playerAudioPlayer.PlayOneShot(ExpGetSound);
        }
    }
}
using System.Collections;
using UnityEngine;

// ���� �����Ѵ�
public class Punch : MonoBehaviour
{
    //���� ���¸� ǥ���ϴµ� ����� Ÿ���� �����Ѵ�
    public enum State
    {
        Ready, // �߻� �غ��
        Empty, // źâ�� ��
        Reloading // ������ ��
    }

    public State state { get; private set; } // ���� ���� ����

    //public Transform fireTransform; // �Ѿ��� �߻�� ��ġ

    public Transform PunchTransform;//��ġ ��ġ
    public Rigidbody PunchHand;//��ġ����;

    public ParticleSystem muzzleFlashEffect; // �ѱ� ȭ�� ȿ��
    //public ParticleSystem shellEjectEffect; // ź�� ���� ȿ��

    //private LineRenderer bulletLineRenderer; // �Ѿ� ������ �׸��� ���� ������

    private AudioSource PunchAudioPlayer; // �� �Ҹ� �����
    //public AudioClip shotClip; // �߻� �Ҹ�
    public AudioClip PunchClip;//��ġ �Ҹ�
    //public AudioClip reloadClip; // ������ �Ҹ�

    //public GunData gunData;

    public float damage = 25; // ���ݷ�
    //private float fireDistance = 50f; // �����Ÿ�
    private float PunchDistance = 5f;//��ġ�����Ÿ�

    //public int ammoRemain = 100; // ���� ��ü ź��
    //public int magCapacity = 25; // źâ �뷮
    //public int magAmmo; // ���� źâ�� �����ִ� ź��


    //public float timeBetFire = 0.12f; // �Ѿ� �߻� ����
    //public float reloadTime = 1.8f; // ������ �ҿ� �ð�
    //private float lastFireTime; // ���� ���������� �߻��� ����


    private void Awake()
    {
        // ����� ������Ʈ���� ������ ��������
        PunchAudioPlayer = GetComponent<AudioSource>();
        //bulletLineRenderer = GetComponent<LineRenderer>();
        //bulletLineRenderer.positionCount = 2;
        //bulletLineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        //�� ���� �ʱ�ȭ
       //ammoRemain = gunData.startAmmoRemain;
       // magAmmo = gunData.magCapacity;

        state = State.Ready;
        //lastFireTime = 0;

    }

    // �߻� �õ�
    public void FirePunch()
    {
        PunchAttack();
    }

    // ���� �߻� ó��
    //private void Shot()
    //{
    //    RaycastHit hit;
    //    Vector3 hitPosition = Vector3.zero;

    //    if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
    //    {
    //        IDamageable target = hit.collider.GetComponent<IDamageable>();
    //        if (target != null)
    //        {
    //            target.OnDamage(gunData.damage, hit.point, hit.normal);
    //        }

    //        hitPosition = hit.point;
    //    }
    //    else
    //    {
    //        hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
    //    }

    //    StartCoroutine(ShotEffect(hitPosition));
    //    magAmmo--;
    //    if (magAmmo <= 0)
    //    {
    //        state = State.Empty;
    //    }
    //}

    private void PunchAttack()
    {
        RaycastHit hit;
        Vector3 hitPosition = Vector3.zero;

        if (Physics.Raycast(PunchTransform.position, PunchTransform.forward, out hit, PunchDistance))
        {
            IDamageable target = hit.collider.GetComponent<IDamageable>();
            if (target != null)
            {
                target.OnDamage(damage, hit.point, hit.normal);
            }

            hitPosition = hit.point;
        }

        //else
        //{
        //    hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        //}

       
        StartCoroutine(PunchEffect(hitPosition));
        //magAmmo--;
        //if (magAmmo <= 0)
        //{
        //    state = State.Empty;
        //}
    }

    // �߻� ����Ʈ�� �Ҹ��� ����ϰ� �Ѿ� ������ �׸���
    //private IEnumerator ShotEffect(Vector3 hitPosition)
    //{
    //    muzzleFlashEffect.Play();



    //    gunAudioPlayer.PlayOneShot(gunData.shotClip);//=> ��ħ
    //    bulletLineRenderer.SetPosition(0, fireTransform.position);
    //    bulletLineRenderer.SetPosition(1, hitPosition);

    //    // ���� �������� Ȱ��ȭ�Ͽ� �Ѿ� ������ �׸���
    //    bulletLineRenderer.enabled = true;

    //    // 0.03�� ���� ��� ó���� ���
    //    yield return new WaitForSeconds(0.03f);
    //    //yield return null; => �� ������ �����

    //    // ���� �������� ��Ȱ��ȭ�Ͽ� �Ѿ� ������ �����0
    //    bulletLineRenderer.enabled = false;
    //}

    private IEnumerator PunchEffect(Vector3 hitPosition)
    {
      
    


       
      

      

        // 0.03�� ���� ��� ó���� ���
        yield return new WaitForSeconds(0.03f);
        //yield return null; => �� ������ �����

      
    }



    //// ������ �õ�
    //public bool Reload()
    //{
    //    if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= gunData.magCapacity)
    //    {
    //        return false;
    //    }

    //    StartCoroutine(ReloadRoutine());
    //    return true;
    //}

    // ���� ������ ó���� ����
    //private IEnumerator ReloadRoutine()
    //{
    //    // ���� ���¸� ������ �� ���·� ��ȯ
    //    state = State.Reloading;

    //    gunAudioPlayer.PlayOneShot(gunData.reloadClip);

    //    // ������ �ҿ� �ð� ��ŭ ó���� ����
    //    yield return new WaitForSeconds(reloadTime);

    //    int ammoToFill = gunData.magCapacity - magAmmo;
    //    if (ammoRemain < ammoToFill)
    //    {
    //        ammoToFill = ammoRemain;
    //    }

    //    magAmmo += ammoToFill;
    //    ammoRemain -= ammoToFill;

    //    // ���� ���� ���¸� �߻� �غ�� ���·� ����
    //    state = State.Ready;
    //}
}
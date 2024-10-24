using System.Collections;
using UnityEngine;
using UnityEngine.AI; // AI, ������̼� �ý��� ���� �ڵ� ��������

// ���� AI ����
public class Zombie : LivingEntity
{
    private Transform Mine;
    public LayerMask whatIsTarget; // ���� ��� ���̾�

    private LivingEntity targetEntity; // ���� ��� => ������ �ִ� ������Ʈ ����ٴϱ�
    private NavMeshAgent navMeshAgent; // ��� ��� AI ������Ʈ

    public ParticleSystem hitEffect; // �ǰ� �� ����� ��ƼŬ ȿ��
    public AudioClip deathSound; // ��� �� ����� �Ҹ�
    public AudioClip hitSound; // �ǰ� �� ����� �Ҹ�

    private Animator zombieAnimator; // �ִϸ����� ������Ʈ
    private AudioSource zombieAudioPlayer; // ����� �ҽ� ������Ʈ
    private Renderer zombieRenderer; // ������ ������Ʈ

    public float damage = 20f; // ���ݷ�
    public float timeBetAttack = 0.5f; // ���� ���� ���� üũ
    private float lastAttackTime; // ������ ���� ����
    private float Knockback = 5.0f;

    public GameObject Exp;
    

    // ������ ����� �����ϴ��� �˷��ִ� ������Ƽ
    private bool hasTarget //set�� ��� �б⸸ ���� �⺻����(�ڵ�����(?))
    {
        get
        {
            // ������ ����� �����ϰ�, ����� ������� �ʾҴٸ� true
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }

            // �׷��� �ʴٸ� false
            return false;
        }
    }

    private void Awake()
    {
        // �ʱ�ȭ
        navMeshAgent =GetComponent<NavMeshAgent>();
        zombieAnimator = GetComponent<Animator>();  
        zombieAudioPlayer = GetComponent<AudioSource>();

        zombieRenderer = GetComponentInChildren<Renderer>();//���� ������Ʈ�� ���� ������Ʈ�� �������� ��������
        Mine = GetComponent<Transform>();
    }

    // ���� AI�� �ʱ� ������ �����ϴ� �¾� �޼���
    public void Setup(ZombieData zombieData)
    {
        startingHealth = zombieData.health;
        health = zombieData.health;

        damage = zombieData.damage;

        navMeshAgent.speed = zombieData.speed;
        zombieRenderer.material.color=zombieData.skinColor;
    }

    private void Start()
    {
        // ���� ������Ʈ Ȱ��ȭ�� ���ÿ� AI�� ���� ��ƾ ����
        StartCoroutine(UpdatePath());

    }

    private void Update()
    {
        // ���� ����� ���� ���ο� ���� �ٸ� �ִϸ��̼� ���
        zombieAnimator.SetBool("HasTarget", hasTarget);
    }

    // �ֱ������� ������ ����� ��ġ�� ã�� ��� ����
    private IEnumerator UpdatePath()
    {
        // ��� �ִ� ���� ���� ����
        while (!dead)
        {
            if(hasTarget)
            {
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(targetEntity.transform.position);
            }
            else
            {
                navMeshAgent.isStopped=true;

                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);

                for(int i=0;i<colliders.Length; i++) 
                {
                    LivingEntity Entity = colliders[i].GetComponent<LivingEntity>();
                    if((Entity != null) && !Entity.dead)
                    {
                        targetEntity=Entity;
                        break;
                    }
                }
            }

            // 0.25�� �ֱ�� ó�� �ݺ�
            yield return new WaitForSeconds(0.25f);
        }
    }

    // �������� �Ծ��� �� ������ ó��
    //�������̽��� ����
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if(!dead)
        {
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation= Quaternion.LookRotation(hitNormal);
            hitEffect.Play();

            zombieAudioPlayer.PlayOneShot(hitSound);

            Rigidbody rigidbody = GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(-hitNormal * Knockback, ForceMode.Impulse);
        }

        // LivingEntity�� OnDamage()�� �����Ͽ� ������ ����
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    // ��� ó��
    public override void Die()
    {
        // LivingEntity�� Die()�� �����Ͽ� �⺻ ��� ó�� ����
        base.Die();

        Collider[] collider = GetComponents<Collider>();

        for(int i=0;i<collider.Length; i++)
        {
            collider[i].enabled = false;
        }

        navMeshAgent.isStopped = true;
        navMeshAgent.enabled = false;

        zombieAnimator.SetTrigger("Die");
        zombieAudioPlayer.PlayOneShot(deathSound);
        //���� ����� ����ġ ���� ���
        GameObject expBead = Instantiate(Exp, Mine.transform.position, Quaternion.identity);
        Destroy(expBead, 10f);
    }

    private void OnTriggerStay(Collider other)
    {
        // Ʈ���� �浹�� ���� ���� ������Ʈ�� ���� ����̶�� ���� ����
        if(!dead && Time.time>=lastAttackTime+timeBetAttack)
        {
            LivingEntity entity = other.GetComponent<LivingEntity>();
            if(entity != null&&entity==targetEntity)
            {
                lastAttackTime = Time.time;

                Vector3 hitPoint = other.ClosestPoint(transform.position);
                Vector3 hitNormal = transform.position -other.transform.position;

                entity.OnDamage(damage, hitPoint, hitNormal);
               
            }
        }
    }
}
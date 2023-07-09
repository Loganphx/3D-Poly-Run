using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Animator _animator;
    private AudioSource _audioSource;
    
    [Header("Particles")]
    [SerializeField] private ParticleSystem _explosionParticle;
    [SerializeField] private ParticleSystem _dirtParticle;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private AudioClip _crashSound;
    
    [Header("Player Settings")]
    [SerializeField] private bool      isGrounded        = true;
    [SerializeField] private float     jumpForce         = 10;
    [SerializeField] private float doubleJumpCooldown = 5;
    [SerializeField] private bool canDoubleJump;
    [SerializeField] private bool isAlive;

    private Action<PlayerController> _onDeath;
    private Action<bool> _onAccelerate;
    
    private static readonly int  JumpTrig = Animator.StringToHash("Jump_trig");
    private static readonly int  SpeedF = Animator.StringToHash("Speed_f");
    private static readonly int DeathTypeINT = Animator.StringToHash("DeathType_int");
    private static readonly int DeathB       = Animator.StringToHash("Death_b");
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        isAlive = true;
        canDoubleJump = true;
    }

    public void Init(Action<PlayerController> onDeath, Action<bool> onAccelerate)
    {
        _onDeath = onDeath;
        _onAccelerate = onAccelerate;
    }
    private void Update()
    {
        if (!isAlive) return;
        if (Input.GetKeyDown(KeyCode.Space)) Jump();
        if (Input.GetKeyDown(KeyCode.LeftShift)) Accelerate(true);
        else if (Input.GetKeyUp(KeyCode.LeftShift)) Accelerate(false);
    }

    private void Jump() 
    {
        if (isGrounded || canDoubleJump)
        {
            _animator.SetTrigger(JumpTrig);
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _dirtParticle.Stop();
            _audioSource.PlayOneShot(_jumpSound);
            _animator.SetTrigger(JumpTrig);

            if(!isGrounded && canDoubleJump) StartCoroutine(DoubleJumpCooldown());
            
            isGrounded = false;
        }
    }

    private void Accelerate(bool isActive)
    {
        _onAccelerate?.Invoke(isActive);
        _animator.SetFloat(SpeedF, isActive ? 1.5f : 1);
    }

    private IEnumerator DoubleJumpCooldown()
    {
        canDoubleJump = false;
        yield return new WaitForSeconds(doubleJumpCooldown);
        canDoubleJump = true;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (isAlive && collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            _dirtParticle.Play();
        }
        else if (isAlive && collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Game Over!");
            _animator.SetBool(DeathB, true);
            _animator.SetInteger(DeathTypeINT, 1);
            
            _explosionParticle.Play();
            _dirtParticle.Stop();
            _audioSource.PlayOneShot(_crashSound);
            
            _onDeath.Invoke(this);
            isAlive = false;
            enabled = false;
        }
    }
}

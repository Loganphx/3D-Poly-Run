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
    
    [SerializeField] private bool      isGrounded        = true;
    [SerializeField] private float     jumpForce         = 10;
    [SerializeField] private float     gravityMultiplier = 1;
    private float doubleJumpCooldown = 5;

    
    [SerializeField] public bool gameOver = false;
    private static readonly int  JumpTrig = Animator.StringToHash("Jump_trig");

    private bool canDoubleJump;

    private static readonly int DeathTypeINT = Animator.StringToHash("DeathType_int");
    private static readonly int DeathB       = Animator.StringToHash("Death_b");

    // Start is called before the first frame update
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        Physics.gravity *= gravityMultiplier;
        canDoubleJump = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if(!gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space)) Jump();
        } 
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

    private IEnumerator DoubleJumpCooldown()
    {
        canDoubleJump = false;
        yield return new WaitForSeconds(doubleJumpCooldown);
        canDoubleJump = true;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(gameOver) return;
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            _dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameOver = true;
            Debug.Log("Game Over!");
            _animator.SetBool(DeathB, true);
            _animator.SetInteger(DeathTypeINT, 1);
            _explosionParticle.Play();
            _dirtParticle.Stop();
            _audioSource.PlayOneShot(_crashSound);
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float _movementSpeed = 4.5f;
    [SerializeField] private float _jumpHeight = 2;

    private Rigidbody2D _rigidbody2D;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private Vector2 _moveInput;

    [SerializeField] private float _sensorSize = 1;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundSensor;

    private Animator _animator;

    private InputAction _attackAction;
    [SerializeField] private int _attackDamage = 2;
    [SerializeField] private Transform _attackHitBox;
    [SerializeField] private float _hitBoxRadius = 0.7f;

    [SerializeField] private InputAction _pauseAction;

    private AudioSource _playerAudioSource;

    [SerializeField] private AudioClip _jumpSound;
    [SerializeField] private AudioClip _attackSound;

    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private int _actualHealth;

    [SerializeField] private AudioClip _damageSound;
    [SerializeField] private AudioClip _deathSound;

    private bool _isDead = false;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _moveAction = InputSystem.actions["Move"];
        _jumpAction = InputSystem.actions["Jump"];
        _animator = GetComponent<Animator>();
        _attackAction = InputSystem.actions["Attack"];
        _pauseAction = InputSystem.actions["Pause"];

        _playerAudioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        //_actualHealth = _maxHealth;
    }

    void Update()
    {
        if (_isDead)
            return;

        if (_pauseAction.WasPressedThisFrame())
        {
            GameManager.Instance.Pause();
        }

        if (GameManager.Instance.IsPaused())
            return;

        _moveInput = _moveAction.ReadValue<Vector2>();

        if (_moveInput.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            _animator.SetBool("IsRunning", true);
        }
        else if (_moveInput.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _animator.SetBool("IsRunning", true);
        }
        else
        {
            _animator.SetBool("IsRunning", false);
        }

        if (_jumpAction.WasPressedThisFrame() && IsGrounded())
            Jump();

        if (_attackAction.WasPressedThisFrame() && IsGrounded())
            Attack();

        _animator.SetBool("IsJumping", !IsGrounded());
    }

    void FixedUpdate()
    {
        if (_isDead)
            return;

        _rigidbody2D.linearVelocity = new Vector2(
            _moveInput.x * _movementSpeed,
            _rigidbody2D.linearVelocity.y
        );
    }

    void Jump()
    {
        _rigidbody2D.AddForce(
            Vector2.up * Mathf.Sqrt(
                _jumpHeight * -2 * Physics2D.gravity.y
            ),
            ForceMode2D.Impulse
        );

        PlaySFX(_jumpSound);
    }

    void Attack()
    {
        _animator.SetTrigger("IsAttack");
        PlaySFX(_attackSound);

        Collider2D[] colliders2D = Physics2D.OverlapCircleAll(
            _groundSensor.position,
            _hitBoxRadius
        );

        foreach (Collider2D enemy in colliders2D)
        {
            if (enemy.gameObject.layer == 7)
            {
                Mimik enemyScript = enemy.GetComponent<Mimik>();

                if (enemyScript != null)
                    enemyScript.TakeDamage(_attackDamage);
            }
        }
    }

    bool IsGrounded()
    {
        Collider2D[] colliders2D = Physics2D.OverlapCircleAll(
            _groundSensor.position,
            _sensorSize
        );

        foreach (Collider2D item in colliders2D)
        {
            if (item.gameObject.layer == 6)
                return true;
        }

        return false;
    }

    void OnDrawGizmos()
    {
        if (_groundSensor != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_groundSensor.position, _sensorSize);
        }

        if (_attackHitBox != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(
                _attackHitBox.position,
                _hitBoxRadius
            );
        }
    }

    void PlaySFX(AudioClip clip)
    {
        if (_playerAudioSource != null && clip != null)
            _playerAudioSource.PlayOneShot(clip);
    }

    public void AddHealth(int _healAmaount)
    {
        _actualHealth += _healAmaount;

        if (_actualHealth > _maxHealth)
            _actualHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (_isDead)
            return;

        _actualHealth -= damage;
        _actualHealth = Mathf.Max(0, _actualHealth);

        if (_actualHealth <= 0)
        {
            Die();
            return;
        }

        PlaySFX(_damageSound);

        GameManager.Instance.StartCoroutine(
            GameManager.Instance.ReduceHealthBar(
                (float)damage / (float)_maxHealth
            )
        );
    }

    void Die()
    {
        if (_isDead)
            return;

        _isDead = true;
        _moveInput = Vector2.zero;

        if (_rigidbody2D != null)
        {
            _rigidbody2D.linearVelocity = Vector2.zero;
            _rigidbody2D.simulated = false;
        }

        if (_deathSound != null && _playerAudioSource != null)
        {
            _playerAudioSource.PlayOneShot(_deathSound);
            StartCoroutine(LoadGameOverAfterSound());
        }
        else
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    private IEnumerator LoadGameOverAfterSound()
    {
        yield return new WaitForSecondsRealtime(_deathSound.length);

        SceneManager.LoadScene("GameOver");
    }

    void LateUpdate()
    {
        if (_isDead)
            return;

        Camera cam = Camera.main;

        if (cam == null)
            return;

        Vector3 viewportPosition =
            cam.WorldToViewportPoint(transform.position);

        if (viewportPosition.y < -0.05f)
            Die();
    }
}
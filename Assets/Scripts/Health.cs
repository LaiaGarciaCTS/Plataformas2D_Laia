using UnityEngine;

public class Health : MonoBehaviour
{
    private AudioSource _healthAudioSource;
    [SerializeField]private AudioClip _healthAudio;

    private CircleCollider2D _collider;
    private SpriteRenderer _spriteRenderer;

    [SerializeField]private int _healthAmaount = 2;


    void Awake()
    {
        _healthAudioSource = GetComponent<AudioSource>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();
    }

    void PlaySFX()
    {
        _healthAudioSource.PlayOneShot(_healthAudio);
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerController _playerScript = collision.GetComponent<PlayerController>();
            _playerScript.AddHealth(_healthAmaount);

            GameManager.Instance.AddHeart();

            PlaySFX();
            _spriteRenderer.enabled = false;
            _collider.enabled = false;
            Destroy(gameObject, 0.5f);
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEditor;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerCollisionHandler))]
[RequireComponent(typeof(WeaponController))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _jumpForce;
    [SerializeField] private PlayerBullet _playerBullet;
    [SerializeField] private GameUIController _gameStateController;

    private WeaponController _weaponController;
    private PlayerCollisionHandler _playercollisionhandler;
    private Rigidbody2D _rigidbody;
    private Vector3 _startPosition;
    private bool _isAlteredGravity;
    private bool _canGravityChange;
    private bool _onMenu;
    private float _startSpeed = 2;
    private float _zeroSpeed = 0;
    private float _normalGravity = 2;
    private float _reversedGravity = -2;

    public float Speed { get; private set; }

    private void Awake()
    {
        _startPosition = new Vector3(0, -3.96f, -3);
        _onMenu = true;
        Speed = 0;
        _canGravityChange = true;
        _isAlteredGravity = false;
        _rigidbody = GetComponent<Rigidbody2D>();
        _playercollisionhandler = GetComponent<PlayerCollisionHandler>();
        _weaponController = GetComponent<WeaponController>();
    }

    private void Update()
    {
        transform.Translate(Vector3.right * Speed * Time.deltaTime);
    }

    private void StartGame()
    {
        Speed = _startSpeed;
        ChangeState(false);
        SetStartPosition();
    }

    private void SetStartPosition()
    {
        transform.position = _startPosition;
    }

    private void GameOverState()
    {
        Speed = _zeroSpeed;
        ChangeState(true);
    }

    private void ChangeState(bool state)
    {
        _onMenu = state;
        _weaponController.ChangeOnMenuValue(_onMenu);
    }

    public void TurnOffGravityChanger()
    {
        SwitchToStandardGravity();
        _canGravityChange = false;
    }

    private void OnJump()
    {
        if (!_isAlteredGravity)
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
        else
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.AddForce(Vector2.down * _jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnChangeGravity()
    {
        if(!_onMenu)
        {
            if (_canGravityChange)
            {
                if (!_isAlteredGravity)
                {
                    _isAlteredGravity = true;
                    _rigidbody.gravityScale = _reversedGravity;
                }
                else
                {
                    SwitchToStandardGravity();
                }
            }
        }
    }
    
    private void SwitchToStandardGravity()
    {
        _isAlteredGravity = false;
        _rigidbody.gravityScale = _normalGravity;
    }

    private void TurnOnGravityChanger()
    {
        _canGravityChange = true;
    }

    private void OnEnable()
    {
        _gameStateController.ChangeState += ChangeState;
        _gameStateController.StartGame += StartGame;
        _gameStateController.GameOver += GameOverState;
        _gameStateController.MenuButtonClick += SetStartPosition;
        _playercollisionhandler.AntiGravitySwitchEnabled += TurnOffGravityChanger;
        _playercollisionhandler.AntiGravitySwitchOffed += TurnOnGravityChanger;
    }

    private void OnDisable()
    {
        _gameStateController.ChangeState -= ChangeState;
        _gameStateController.StartGame -= StartGame;
        _gameStateController.GameOver -= GameOverState;
        _gameStateController.MenuButtonClick -= SetStartPosition;
        _playercollisionhandler.AntiGravitySwitchEnabled -= TurnOffGravityChanger;
        _playercollisionhandler.AntiGravitySwitchOffed -= TurnOnGravityChanger;
    }
}
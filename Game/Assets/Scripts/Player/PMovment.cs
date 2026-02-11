
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PMovment : MonoBehaviour
{
    private bool _isHoldingJump;
    private float _dirX;

    [SerializeField] private GameObject m_menu;

    #region Running Vars
    [Header("running")]

    [Range(0, 100)][SerializeField] private float _speed;
    [Range(0, 10)][SerializeField] private float _sprint;
    [SerializeField] private bool _sprinting;
    [SerializeField] private float _accelaration;
    [SerializeField] private float _deccelaration;
    [SerializeField] private float _resistance;
    [SerializeField] private float _resistanceTime;
    private float _timeOnFloor;
    // Change of Accelaration
    [Range(0, 2)][SerializeField] private float _COA;

    #endregion

    #region JumpingVars
    [Header("JumpStuff")]
    [Range(0, 10)][SerializeField] private int _extraJump;
    [SerializeField] private float _jumpV;
    [SerializeField] private float _maxCharge;
    [SerializeField] private float _airResistance;
    [SerializeField] private float _timeToJump;
    private int _extraJumpMax;
    private float _airTime;
    private float _charge;

    #endregion

    #region WallJumpingVars
    [Header("WallJumping")]
    [SerializeField] private bool _wallJumps = true;
    [SerializeField] private LayerMask _wallLayer;
    [Range(0, 50)][SerializeField] private float _wallJumpF;
    [Range(0, 32)][SerializeField] private float _wallFriction;
    private bool _onWall = false;
    #endregion

    private Rigidbody2D _rb;
    private Animator _anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _extraJumpMax = _extraJump;

        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {

        #region Walking
        // Desired Running Speed Determined by set speed and direction or if sprint button pressed direction,speed and sprint speed
        float targetSpeed = (_sprinting && _airTime == 0) ? _dirX * _speed * _sprint : _dirX * _speed;

        // The Diff between targetSpeed and actual speed
        float speedDiff = targetSpeed - _rb.linearVelocityX;

        // The Rate off Accelaration/Deaccelartion
        float accelRate = (Mathf.Abs(targetSpeed) > 0.05f)? _accelaration : _deccelaration ;

        //finalAccelaration is dependant on 1 how near you are on the target speed so when target speed = 0 then accelaration = 0
        //2 how fast you accelarate and 3 the change of accelaration so when Coa = 1 its a const Coa > 1 its exponential and Coa < 1 its logarythmic
        float finalAccelartion = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, _COA) * Mathf.Sign(speedDiff);

        _rb.AddForce(finalAccelartion * Vector2.right);

        //Animation
        _anim.SetFloat("Walk", Mathf.Abs(_dirX));

        _anim.SetBool("Sprinting", _sprinting);

        #endregion

        #region Resistance
        // if on floor & if target speed faster then deccelaration create reasistance to movment to prevent sliding
        // if in air add air resistance 
        if (_airTime == 0 && Mathf.Abs(_dirX) < 0.5f)
        {
            float finalResistance = (targetSpeed > _deccelaration) ? _resistance : 0;
            // make resistance go in the direction the player is moving
            finalResistance *= Mathf.Sign(_rb.linearVelocityX);
            // add force in the inverted dir the player is walking
            _rb.AddForce(Vector2.right * -finalResistance);
        }
        if(_airTime != 0f)
        {
            float finalResistance = _airResistance;
            // make resistance go in the direction the player is moving
            finalResistance *= Mathf.Sign(_rb.linearVelocityX);
            // add force in the inverted dir the player is walking
            _rb.AddForce(Vector2.right * -finalResistance);
        }
        // Allows for no loss in resistance if player jumps early enough
        if (_timeOnFloor < _resistanceTime && _airTime ==0 )
        {
            _rb.linearDamping = 0;
        }
        #endregion

        #region Jumping

        if (0 == _airTime && _isHoldingJump && _maxCharge > _charge)
        {
            // When after 1 second of holding down maxCharge getsReached
            //charge = power addet to jump after holding down space
            _charge += _maxCharge * Time.deltaTime;
        }

        if (_airTime > 0) _airTime += 1 * Time.deltaTime;
        else if (_airTime == 0) { _timeOnFloor += 1 * Time.deltaTime; }

        #endregion

        #region WallJumping

        // checks if Collider size + tolerance would overlap with wall
        _onWall = Physics2D.OverlapBox(transform.position, new Vector2(gameObject.GetComponent<CapsuleCollider2D>().size.x + 0.1f,
        gameObject.GetComponent<CapsuleCollider2D>().size.x - 0.1f), 0, _wallLayer);

        //if wall jumps activated and 
        if (_onWall && _wallJumps && _airTime != 0)
        {
            // Target Sliding Speed = the fircion of the wall and of stron are you running into it
            float targetSS = _wallFriction * Mathf.Abs(_dirX);
            // The Diff = how near is the target speed
            float SlidingSpeedDiff = targetSS - _rb.linearVelocityY;

            //adds a force that acts against the falling
            _rb.AddForce(Vector2.up * Mathf.Abs(SlidingSpeedDiff));
           
        }
        #endregion

    }

    #region Input

    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _sprinting = true;
        }
        else if (context.canceled)
        {
            _sprinting = false;
        }
    }
    public void InputX(InputAction.CallbackContext context)
    {
        _dirX = context.ReadValue<float>();

        if (_dirX < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (_dirX > 0) 
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

    }
    public void Jump(InputAction.CallbackContext context)
    {
        // if space is hold while on floor charge jump
        // if space is realeased in the time of an tolerance and player hasnt used its first jump and is in time to jump add charge to the normal jumpVelocity
        // if player has extra jumps left and is in air jump with the normal velocity
        if ( context.performed && _extraJump == _extraJumpMax)
        {
            _isHoldingJump = true;
        }
        else if (context.canceled && _timeToJump > _airTime && _extraJump == _extraJumpMax)
        {
            _isHoldingJump = false;
            _rb.linearVelocityY = _jumpV + _charge;
            _extraJump--;
            _anim.SetBool("Jump", true);

            _charge = 0;
        }
        else if (context.performed && _extraJump > 0 && _onWall == false)
        {
            _anim.SetBool("Jump", true);
            _rb.linearVelocityY = _jumpV;
            _extraJump--;
        }

        //wall jump if on wall and jump button pressed
        if (context.performed && _onWall && _wallJumps && _airTime != 0)
        {
            // add force in the opposite direction of the one the player looks
            if (gameObject.GetComponent<SpriteRenderer>().flipX == false)
            {
                _rb.linearDamping = 1;
                _rb.linearVelocityY = 0;
                _rb.AddForce(new Vector2(-1f, 2) * _wallJumpF, ForceMode2D.Impulse);
            }
            else
            {
                _rb.linearDamping = 1;
                _rb.linearVelocityY = 0;
                _rb.AddForce(new Vector2(1f, 2) * _wallJumpF, ForceMode2D.Impulse);
            }
        }

    }

    public void Pause(InputAction.CallbackContext context)
    {
        Time.timeScale = 0;
        m_menu.SetActive(true);
    }

    #endregion

    #region GetMethods
    public float JumpForce() => _jumpV;

    #endregion

    #region MethodsToChangeValues
    public void ChangeJumpForce(float value)
    {
        _jumpV = value;
    }

    #endregion

    private void OnTriggerStay2D(Collider2D collision)
    {
        // if is on floor reset amount of jumps left,set values like resistance etc
        if (collision.gameObject.tag == "Floor" )
        {
            _airTime = 0;
            if (_timeOnFloor > _resistanceTime) { _rb.linearDamping = 1; }
            _extraJump = _extraJumpMax;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            _rb.linearDamping = 1;
            _timeOnFloor = 0;
            _airTime += 1 * Time.deltaTime;
        }

    }
}

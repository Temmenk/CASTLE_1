using UnityEngine;

public class PlayerBehavior : MonoBehaviour 
{
   public float moveSpeed = 10f;
   public float rotateSpeed = 75f;
    public float jumpVelocity = 5f;
    public float distanceToGround = 0.1f;
    public LayerMask groundLayer;
    public GameObject bullet;
    public float bulletSpeed = 100f;
    private float vInput;
    private float hInput;
    private Rigidbody _rb;
    private CapsuleCollider _col;
    private bool doJump = false;
    private bool doShoot = false;
    private GameBehavior _gameManager;
 
    void Start()
    {
         _rb = GetComponent<Rigidbody>();
         _col = GetComponent<CapsuleCollider>();
         _gameManager = GameObject.Find("GameManager").GetComponent<GameBehavior>();
         
     }
 
     void Update()
     {
       vInput = Input.GetAxis("Vertical") * moveSpeed;
       hInput = Input.GetAxis("Horizontal") * rotateSpeed;

        /*
       this.transform.Translate(Vector3.forward * vInput * 
       Time.deltaTime);
       this.transform.Rotate(Vector3.up * hInput * Time.deltaTime);
       */

       if (Input.GetKeyDown(KeyCode.Space) && IsGrounded()) 
        { 
            doJump = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            doShoot = true;
        }
     }
     void FixedUpdate()
    {
        if(doJump)
        {
            _rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
            doJump = false;
        }
        Vector3 rotation = Vector3.up * hInput;

        Quaternion angleRot = Quaternion.Euler(rotation *
            Time.fixedDeltaTime);

        _rb.MovePosition(this.transform.position +
            this.transform.forward * vInput * Time.fixedDeltaTime);

        _rb.MoveRotation(_rb.rotation * angleRot);
        
        if (doShoot)
        {
            doShoot = false;
            //GameObject newBullet = Instantiate(bullet,
                //this.transform.position + new Vector3(1,0,0),
                  //  this.transform.rotation) as GameObject;
            GameObject newBullet = Instantiate(bullet, this.transform.position +
                transform.right, this.transform.rotation) as GameObject;
            
            Rigidbody bulletRB =
                newBullet.GetComponent<Rigidbody>();
            
            bulletRB.linearVelocity = this.transform.forward * 
                                            bulletSpeed;
        }
    }

    private bool IsGrounded()
    {
        Vector3 capsuleBottom = new Vector3( _col.bounds.center.x,
            _col.bounds.min.y, _col.bounds.center.z);

        bool grounded = Physics.CheckCapsule(_col.bounds.center,
            capsuleBottom, distanceToGround, groundLayer,
                QueryTriggerInteraction.Ignore);

        return grounded;
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "enemy")
        {
            int damage = 1;
            if (_gameManager.Armor > 0)
            {
                _gameManager.Armor -= damage; // if there is armor, this drops first
            }
            else
            {
                _gameManager.PlayerHealth -= damage; // drops health when armor depleted
            }
            if (_gameManager.PlayerHealth <= 0)
            {
                Debug.Log("Game Over");
                _gameManager.GameOver();
            }
        }
    }

    private float speedMultipier;

    public void BoostSpeed(float multiplier, float seconds)
    {
        speedMultipier = multiplier;
        moveSpeed *= multiplier;
        Invoke("EndSpeedBoost", seconds);
    }

    private void EndSpeedBoost()
    {
        Debug.Log("catch your breath.");
        moveSpeed /= speedMultipier;
    }
 } 
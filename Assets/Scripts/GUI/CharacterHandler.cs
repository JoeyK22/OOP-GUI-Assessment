using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("RPG/Character Handler")]
public class CharacterHandler : MonoBehaviour
{
    #region Variables
    [Header("CHARACTER HANDLER")]
    [Space(5)]
    [Header("General")]
    #region Health Stamina Mana
    public bool alive;
    public int maxHealth, maxStamina, maxMana;
    public float curHealth, curStamina, curMana;
    #endregion
    [Space(3)]
    [Header("Player Stats")]
    #region Stats
    public int charisma;
    public int constitution;
    public int dexterity;
    public int strength;
    public int wisdom;
    public int intelligence;
    #endregion
    [Space(3)]
    [Header("Levels and Exp")]
    #region Level and Exp
    public int level;
    public int curExp, maxExp;
    #endregion
    [Space(3)]
    [Header("Weapon Stats")]
    #region Weapon
    public int weaponDamage;
    public int ammo, ammoUsed;
    public float coolDown;
    public bool canAttack, isEquipped;
    public GameObject weaponMount, helmMount, packMount;
    #endregion
    [Space(3)]
    [Header("Movement Connection")]
    #region Movement Connection
    public Vector3 moveDir = Vector3.zero;
    public CharacterController charC;
    #endregion
    [Space(3)]
    [Header("Movement Variables")]
    #region Movement Variables
    public float jumpSpeed = 8.0f;
    public float speed = 6.0f;
    public float walkSpeed = 6, crouchSpeed = 2, sprintSpeed = 14;
    public float gravity = 20.0f;
    #endregion
    [Space(3)]
    [Header("Camera Connection")]
    #region Camera
    public RenderTexture miniMap; 
	public Camera miniMapCamera;
	public enum RotationalAxis
	{
		MouseXAndY = 0,
		MouseX = 1,
		MouseY = 2
	}
	public RotationalAxis axis = RotationalAxis.MouseX;
	public float sensitivityX = 15f;
	public float sensitivityY = 15f;
	public float minimumY = -60f;
	public float maximumY = 60f;
	float rotationY = 0;
    #endregion
    [Space(3)]
    [Header("Animations")]
    #region Animator
    public Animator anim;
    #endregion
    [Space(3)]
    [Header("Check Point Elements")]
    #region Check Points
    public GameObject curCheckPoint;
    #endregion
    [Space(3)]
    [Header("Textures")]
    #region Textures
    public Renderer meshRenderer;
    public int armourIndex, clothesIndex, eyesIndex, hairIndex, helmIndex, mouthIndex, skinIndex;
    #endregion
    #endregion
    #region Awake
    void Awake()
    {
        #region AutoSetUp    
        charC = this.GetComponent<CharacterController>();
        weaponMount = GameObject.FindGameObjectWithTag("WeaponMount");
        packMount = GameObject.FindGameObjectWithTag("PackMount");
        helmMount = GameObject.FindGameObjectWithTag("HelmMount");
        miniMapCamera = GameObject.FindGameObjectWithTag("MiniMapCamera").GetComponent<Camera>();
        meshRenderer = GameObject.FindGameObjectWithTag("PlayerMesh").GetComponent<Renderer>();
        miniMap = Resources.Load("Minimap") as RenderTexture;
        miniMapCamera.targetTexture = miniMap;
        anim = this.GetComponent<Animator>();
        #endregion
        #region Stat Load
        charisma = PlayerPrefs.GetInt("Charisma", 8);
        constitution = PlayerPrefs.GetInt("Constitution", 8);


        #endregion
    }

    

    #endregion
	#region Start
    void Start()
    {
		if(this.GetComponent<Rigidbody>())
		{
			GetComponent<Rigidbody>().freezeRotation = true;
		}
        this.transform.position = curCheckPoint.transform.position;
    }
	#endregion
    #region Update
    void Update()
    {
		Movement();
		MouseLook();
		CheckPoint();
		ExpHandler();
		PickUpHandler();
    }
    #endregion
	#region LateUpdate
	void LateUpdate()
	{
		StatCaps();
	}
	#endregion
	#region Movement
    void Movement()
    {
        if (charC.isGrounded)
        {
            moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDir = transform.TransformDirection(moveDir);
            moveDir *= speed;
            if (Input.GetButton("Jump"))
            {
                moveDir.y = jumpSpeed;
            }
        }
        moveDir.y -= gravity * Time.deltaTime;
        charC.Move(moveDir * Time.deltaTime);
    }
	#endregion
	#region CheckPoint
	void CheckPoint()
	{
		#region Update
		if(curHealth == 0)
		{
			this.transform.position = curCheckPoint.transform.position;
			curHealth = maxHealth;
		}
		#endregion
	}
	#endregion
	#region Checkpoint Trigger
	void OnTriggerEnter(Collider other)
	{
		#region OnTriggerEnter
		if(other.CompareTag("CheckPoint"))
		{
			curCheckPoint = other.gameObject;
			PlayerPrefs.SetString("SpawnPoint",curCheckPoint.name);
		}
		#endregion
	}
	#endregion
	#region GUI Bars
	void OnGUI()
	{
		#region OnGUI
		float scrW = Screen.width/16;
		float scrH = Screen.height/9;
		
		GUI.Box(new Rect(6*scrW,0.25f*scrH,4*scrW,0.5f*scrH),"");
		GUI.Box(new Rect(6*scrW,0.25f*scrH,curHealth*(4*scrW)/maxHealth,0.5f*scrH),"");
		GUI.Box(new Rect(6*scrW,0.25f*scrH,4*scrW,0.5f*scrH),"");

		GUI.Box(new Rect(6*scrW,0.75f*scrH,4*scrW,0.5f*scrH),"");
		GUI.Box(new Rect(6*scrW,0.75f*scrH,curMana*(4*scrW)/maxMana,0.5f*scrH),"");
		GUI.Box(new Rect(6*scrW,0.75f*scrH,4*scrW,0.5f*scrH),"");

		GUI.Box(new Rect(6*scrW,1.25f*scrH,4*scrW,0.5f*scrH),"");
		GUI.Box(new Rect(6*scrW,1.25f*scrH,curStamina*(4*scrW)/maxStamina,0.5f*scrH),"");
		GUI.Box(new Rect(6*scrW,1.25f*scrH,4*scrW,0.5f*scrH),"");
		#endregion
	}
	#endregion
	#region Stats and Caps
	void StatCaps()
	{
		#region Connect to LateUpdate
		if(curHealth > maxHealth)
		{
			curHealth = maxHealth;
		}
		if(curHealth <= 0)
		{
			curHealth = 0;
		}
		if(curMana > maxMana)
		{
			curMana = maxMana;
		}
		if(curMana <= 0)
		{
			curMana = 0;
		}
		if(curStamina > maxStamina)
		{
			curStamina = maxStamina;
		}
		if(curStamina <= 0)
		{
			curStamina = 0;
		}
		#endregion
	}
	#endregion
	#region EXP
	void ExpHandler()
	{
		if(curExp >= maxExp)
		{
			curExp -= maxExp;
			level++;
			maxExp += 50;
		}
	}
	#endregion
	#region MouseLook
	void MouseLook()
	{
		if(axis == RotationalAxis.MouseXAndY)
		{
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X")* sensitivityX;
			
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY,maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY,rotationX,0);
		}
		else if(axis == RotationalAxis.MouseX)
		{
			transform.Rotate(0,Input.GetAxis("Mouse X") * sensitivityX,0);
		}
		else
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp(rotationY,minimumY,maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}
	}
	#endregion
	#region Interact
	void PickUpHandler()
	{
		if(Input.GetKeyDown(KeyCode.E))
		{
			Ray interact;
			interact = Camera.main.ScreenPointToRay(new Vector2(Screen.width/2, Screen.height/2));
			RaycastHit hitinfo;
			if(Physics.Raycast(interact,out hitinfo,10))
			{
				#region NPC tag
				if(hitinfo.collider.CompareTag("NPC"))
				{
					Debug.Log("Hit the NPC");
				}
				#endregion
				#region Item
				if(hitinfo.collider.CompareTag("Item"))
				{
					Debug.Log("Hit an Item");
				}
				#endregion
			}
		}
	}
	#endregion
}
*/
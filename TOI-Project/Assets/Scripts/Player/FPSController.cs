using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 414

/// <summary>
/// Classe que controla o Player em primeira pessoa.
/// Autor: Rodrigo Badin
/// </summary>
[RequireComponent(typeof(CharacterController))] //Necessita do CharacterController
public class FPSController : MonoBehaviour
{
	public List<AudioSource> AudioSources = new List<AudioSource>();
	private int audioToUse = 0;

	[SerializeField] private float m_walkSpeed = 3f; //Velocidade de caminhada
	[SerializeField] private float m_runSpeed = 6f; //Velocidade de corrida
	[SerializeField] private float m_crouchSpeed = 1f; //Velocidade agachado
	[SerializeField] private float stickToGroundForce = 5.0f; //Segura player no chão quando desce escadas
	[SerializeField] private float gravityMultiplier = 2.5f; //Multiplicador de gravidade para um maior controle do jogador
	[SerializeField] private float runStepLengthen = 0.5f; //Atrasa o movimento da cabeça quando corre
	[SerializeField] private CurveControllerBob headBob = new CurveControllerBob();
	[SerializeField] private Animator animator;

	private bool m_CanRun;

	public bool CanRun
	{
		get { return m_CanRun; }
		set { m_CanRun = value; }
	}

	// Use o Mouse Look do Standard Assets
	[SerializeField] private UnityStandardAssets.Characters.FirstPerson.MouseLook mouseLook; //Referencia ao MouseLook;

	private Camera viewCamera = null;
	private Vector2 inputVector = Vector2.zero;
	private Vector3 moveDirection = Vector3.zero;
	private bool previouslyGrounded = false;
	private bool isWalking = true;
	private bool isCrouching = false;
	private Vector3 localSpaceCameraPos = Vector3.zero;
	private float controllerHeight = 0.0f;
	private float m_speed;

	//Timers
	private float fallingTimer = 0.0f;

	//Componentes externos
	private CharacterController characterController = null;

	// Máquina de estados
	[SerializeField] private PlayerMoveStatus m_movementStatus = PlayerMoveStatus.NotMoving;

	// Propriedades públicas
	public PlayerMoveStatus MovementStatus { get { return m_movementStatus; } }
	public float WalkSpeed { get { return m_walkSpeed; } }
	public float RunSpeed { get { return m_runSpeed; } }
	public float CrouchSpeed { get { return m_crouchSpeed; } }
	public float CurrentSpeed { get { return m_speed; } }

	private static bool m_playerCanMove;

	public static bool CanMove
	{
		get { return m_playerCanMove; }
		set { m_playerCanMove = value; }
	}

	private void OnDestroy()
	{
		if (GameManager.Instance)
		{
			GameManager.Instance.PauseNotification -= Pause;
			GameManager.Instance.ResumeNotification -= Resume;
		}
	}

	#region Singleton
	//Início do Singleton
	public static FPSController Instance;
	private void Awake()
	{
		{
			if (Instance == null)
				Instance = this;

			else if (Instance != this)
				Destroy(this);
			//Fim do Singleton
			#endregion
			#region Awake
			// Scripts do Awake
			#endregion
		}	
	}

	void Pause()
	{
		mouseLook.SetCursorLock(true);
	}

	void Resume()
	{
		mouseLook.SetCursorLock(true);
	}

	private void Start()
	{
		if (GameManager.Instance)
		{
			GameManager.Instance.PauseNotification += Pause;
			GameManager.Instance.ResumeNotification += Resume;
		}

		m_playerCanMove = true;

		//Referência ao caracter controller
		characterController = GetComponent<CharacterController>();

		controllerHeight = characterController.height; //Armazena a altura do charactercontroller

		//Referência a câmera
		viewCamera = Camera.main;

		localSpaceCameraPos = viewCamera.transform.localPosition;

		// Configura o jogador para não movendo
		m_movementStatus = PlayerMoveStatus.NotMoving;

		// Reseta os timers
		fallingTimer = 0.0f;

		// Configura o script Mouse Look
		mouseLook.Init(transform, viewCamera.transform);

		// Referencia ao RewiredPlayer
		//playerInput = ReInput.players.GetPlayer(0);

		//Inicializa o Objeto Head Bob
		headBob.Initialize();
		headBob.RegisterEventCallback(1.5f, PlayFootStepSound, CurveControlledBobCallbackType.Vertical);

		if (animator == null) Debug.LogError("Animator do Player não indicado!");
	}

	float originalWalkSpeed, originalRunSpeed;
	public void SlowDown()
	{
		if (originalWalkSpeed == WalkSpeed) return;
		if (originalRunSpeed == RunSpeed) return;

		originalWalkSpeed = WalkSpeed;
		originalRunSpeed = RunSpeed;

		m_walkSpeed = 1f;
		m_runSpeed = 1f;
	}


	public void RestoreOriginalSpeed()
	{
		m_walkSpeed = originalWalkSpeed;
		m_runSpeed = originalRunSpeed;
	}

	private void Update()
	{
		if (!CanMove) return;
		//Impede de correr se estiver andando para atrás.
		if (inputVector.y < 0)
			CanRun = false;
		else
			CanRun = true;


		// Se estivermos caindo, incrementar o timer
		if (characterController.isGrounded) fallingTimer = 0f;
		else fallingTimer += Time.deltaTime;

		// Permite Mouse Look uma chance de processar o mouse e rotacionar a câmera
		if (Time.timeScale > Mathf.Epsilon)
			mouseLook.LookRotation(transform, viewCamera.transform);

		//if (playerInput.GetButtonDown("Crouch"))
		//{
		//    isCrouching = !isCrouching;
		//    characterController.height = isCrouching == true ? controllerHeight / 2 : controllerHeight;
		//}

		// Calcula o estado do character
		if (!previouslyGrounded && characterController.isGrounded)
		{
			if (fallingTimer > 0.5f)
			{
				//TODO: Som de aterizagem
			}

			moveDirection.y = 0f;
			m_movementStatus = PlayerMoveStatus.Landing;
		}
		else if (!characterController.isGrounded)
			m_movementStatus = PlayerMoveStatus.NotGrounded;
		else if (characterController.velocity.sqrMagnitude < 0.01f)
			m_movementStatus = PlayerMoveStatus.NotMoving;
		else if (isCrouching)
			m_movementStatus = PlayerMoveStatus.Crouching;
		else if (isWalking)
			m_movementStatus = PlayerMoveStatus.Walking;
		else
			m_movementStatus = PlayerMoveStatus.Running;

		previouslyGrounded = characterController.isGrounded;

	}


	private void FixedUpdate()
	{
		if (!m_playerCanMove) return;

		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		if (m_CanRun)
			isWalking = !Input.GetButton("Run");
		else
			isWalking = true;

		inputVector = new Vector2(horizontal, vertical);

		if (inputVector.sqrMagnitude > 1)
		{
			inputVector.Normalize();
		}

		// Seta a velocidade conforme se o input é andando ou correndo
		m_speed = isCrouching ? m_crouchSpeed : isWalking ? m_walkSpeed : m_runSpeed;
		inputVector = new Vector2(horizontal, vertical);

		// Normaliza o input se exceder 1 na distância combinada.
		if (inputVector.sqrMagnitude > 1) inputVector.Normalize();

		// Sempre move com a câmera para frente como se fosse a direção que estivesse apontada
		Vector3 desiredMove = transform.forward * inputVector.y + transform.right * inputVector.x;

		// Pega a normal da superfice que está sendo tocada para mover junto a ela
		RaycastHit hitDown;
		if (Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out hitDown, characterController.height / 2f, 1))
			desiredMove = Vector3.ProjectOnPlane(desiredMove, hitDown.normal).normalized;


		// Escala o momimento pela a velocidade atual (andando ou correndo)
		moveDirection.x = desiredMove.x * m_speed;
		moveDirection.z = desiredMove.z * m_speed;

		// Se está no chão
		if (characterController.isGrounded)
		{
			moveDirection.y = -stickToGroundForce;
		}
		else
		{
			// Caso contrário  não está no chão então aplica a gravidade multiplicada pelo modificador de gravidade;
			moveDirection += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
		}

		// Move o Character Controller

		characterController.Move(moveDirection * Time.fixedDeltaTime);

		// Estamos movendo? Somente no plano.
		Vector3 speedXZ = new Vector3(characterController.velocity.x, 0.0f, characterController.velocity.z);
		if (speedXZ.magnitude > 0.01f)
			viewCamera.transform.localPosition = localSpaceCameraPos + headBob.GetVectorOffset(characterController.velocity.magnitude * (isWalking ? 1 : runStepLengthen));
		else
			viewCamera.transform.localPosition = localSpaceCameraPos;


		//animator.SetFloat("Speed", speedXZ.magnitude);

	}

	void PlayFootStepSound()
	{
		if (isCrouching) return; //TODO: Sons diferentes quando estiver agachado e correndo

		AudioSources[audioToUse].Play();
		audioToUse = (audioToUse == 0) ? 1 : 0;
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position, new Vector3(0.4f, 1, 0.4f));
	}


}

// Delegates
public delegate void CurveControlledBobCallback();

[System.Serializable]
public class CurveControlledBobEvent
{
	public float Time = 0.0f;
	public CurveControlledBobCallback Function = null;
	public CurveControlledBobCallbackType Type = CurveControlledBobCallbackType.Vertical;
}


[System.Serializable]
public class CurveControllerBob // Controla o movimento da cabeça (head bob)
{
	[SerializeField] //Curva do movimento da cabeça
	private AnimationCurve bobCurve = new AnimationCurve(
		new Keyframe(0f, 0f), new Keyframe(0.5f, 1f),
		new Keyframe(1f, 0f), new Keyframe(1.5f, -1f),
		new Keyframe(2f, 0f));

	// Variáveis do controle da magnitude do balanço da cabeça
	[SerializeField] private float horizontalMultiplier = 0.01f; //Controla o multiplicador do movimento para esquerda e direita da cabeça
	[SerializeField] private float verticalMultiplier = 0.02f; //Controla o multiplicador do movimento para cima e baixo da cabeça
	[SerializeField] private float verticalToHorizontalSpeedRatio = 2.0f; //Proproção da curva em relação ao movimento vertical e o horizontal
	[SerializeField] private float baseInterval = 1.0f;

	private float xPlayHead;
	private float yPlayHead;
	private float prev_xPlayHead = 0.0f;
	private float prev_yPlayHead = 0.0f;
	private float curveEndTime;
	private List<CurveControlledBobEvent> events = new List<CurveControlledBobEvent>();

	public void Initialize()
	{
		curveEndTime = bobCurve[bobCurve.length - 1].time;
		xPlayHead = 0.0f;
		yPlayHead = 0.0f;
		prev_xPlayHead = 0.0f;
		prev_yPlayHead = 0.0f;

	}

	public void RegisterEventCallback(float time, CurveControlledBobCallback function, CurveControlledBobCallbackType type)
	{
		CurveControlledBobEvent ccBobEvent = new CurveControlledBobEvent();
		ccBobEvent.Time = time;
		ccBobEvent.Function = function;
		ccBobEvent.Type = type;
		events.Add(ccBobEvent);
		events.Sort(
			delegate (CurveControlledBobEvent t1, CurveControlledBobEvent t2)
			{
				return (t1.Time.CompareTo(t2.Time));
			}
			);
	}

	public Vector3 GetVectorOffset(float speed)
	{
		xPlayHead += (speed * Time.deltaTime) / baseInterval;
		yPlayHead += ((speed * Time.deltaTime) / baseInterval) * verticalToHorizontalSpeedRatio;
		if (xPlayHead > curveEndTime)
			xPlayHead -= curveEndTime;

		if (yPlayHead > curveEndTime)
			yPlayHead -= curveEndTime;

		//Processar eventos
		for (int i = 0; i < events.Count; i++)
		{
			CurveControlledBobEvent ev = events[i];
			if (ev != null)
			{
				if (ev.Type == CurveControlledBobCallbackType.Vertical)
				{
					if ((prev_yPlayHead < ev.Time && yPlayHead >= ev.Time) ||
					  (prev_yPlayHead > yPlayHead && (ev.Time > prev_yPlayHead || ev.Time <= yPlayHead)))
					{
						ev.Function();
					}
				}
			}
		}

		float xPos = bobCurve.Evaluate(xPlayHead) * horizontalMultiplier;
		float yPos = bobCurve.Evaluate(yPlayHead) * verticalMultiplier;

		prev_xPlayHead = xPlayHead;
		prev_yPlayHead = yPlayHead;

		return new Vector3(xPos, yPos, 0f);

	}


}

#pragma warning restore 414


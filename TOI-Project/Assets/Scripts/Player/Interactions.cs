using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Controla a manipulação de objetos e Raycast
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerInventory))]
public class Interactions : MonoBehaviour
{
	Camera m_Camera;

	public GameObject FeedbackPanel;
	private CanvasGroup FeedbackPanelCanvasGroup;
	private TextMeshProUGUI FeedbackPanelText;
	[SerializeField] private Image FeedbackPanelImage;
	public PlayerInventory playerInventory;

	[Range(1, 5)] public float rayDistance = 3f;
	[Range(5, 15)] public float throwForce = 5f;
	private float inputHoldDelay = 1.666f;
	public PlayerHand currentHand = PlayerHand.Right;

	public DropplableObject objectInLeftHand = DropplableObject.None;
	public DropplableObject objectInRightHand = DropplableObject.None;

	private WaitForSeconds inputHoldWait;
	public bool movingRightHand = false;
	public bool movingLeftHand = false;
	public bool droppable = false;
	public bool cannotDrop = false;
	private bool isPlayerNearInteractable = false;
	private readonly float playerDistanceFromInteractable = 2f;
	private readonly Vector3 centerScreen = new Vector3(0.5f, 0.5f, 0);

	[SerializeField] private Transform rightHandTransform;
	[SerializeField] private Transform leftHandTransform;
	[SerializeField] private Image rightHandUI;
	[SerializeField] private Image leftHandUI;
	[SerializeField] private Animator animator;

	public Layer[] layerPriorities =
	{
		Layer.Default,
		Layer.Obstacles,
		Layer.Interactable,
		Layer.Pickupable,
		Layer.Droppable,
		Layer.Observable
	};


	private RaycastHit m_hit;
	public RaycastHit Hit
	{
		get { return m_hit; }
	}

	[SerializeField] private Layer m_layerHit;
	public Layer LayerHit
	{
		get { return m_layerHit; }
		set { m_layerHit = value; }
	}

	public GameObject carriedObjectRight = null;
	public GameObject carriedObjectLeft = null;

	[HideInInspector] public GameObject lastHighlighted = null; //Último gameobject a ser destacado


	#region Singleton
	//Inicio do Singleton
	private static Interactions instance = null;
	public static Interactions Instance // Esse script possuí uma referência estática a si mesmo para que os outros scripts possam acessar sem precisar de uma referência para tal.
	{
		get { return instance; }
	}

	private void Awake()
	{
		// Essa é uma medida comum para manipular uma classe com uma referência a si mesma.
		// Se a variável da instância existir, mas não for este objeto, destrua este objeto.
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		else
		{
			// Caso contrário, este objeto será asignado para a instância.
			instance = this;
		}//Fim do Singleton
		#endregion
		#region Awake
		// Demais scripts Awake
		#endregion
	}

	private void Start()
	{
		m_Camera = Camera.main;
		if (m_Camera == null)
		{
			Debug.LogError("Câmera não encontrada");
			Destroy(this.gameObject);
		}

		if (rightHandTransform == null)
		{
			Debug.LogError("Right Hand não encontrada!");
		}

		if (leftHandTransform == null)
		{
			Debug.LogError("Left Hand não encontrada!");
		}

		if (!rightHandUI)
		{
			Debug.LogError("Right Hand UI não encontrada!");
		}

		if (!leftHandUI)
		{
			Debug.LogError("Left Hand UI não encontrada!");
		}

		if (!FeedbackPanel)
		{
			Debug.LogError("FeedbackPanel não encontrado!");
		}

		if (!FeedbackPanelCanvasGroup)
			FeedbackPanelCanvasGroup = FeedbackPanel.GetComponent<CanvasGroup>();

		if (!FeedbackPanelText)
			FeedbackPanelText = FeedbackPanel.GetComponentInChildren<TextMeshProUGUI>();

		if (!FeedbackPanelImage)
			FeedbackPanelImage = FeedbackPanel.GetComponentInChildren<Image>();

		if (!playerInventory)
			playerInventory = GetComponent<PlayerInventory>();

		HighlightHandUI(currentHand);

		inputHoldWait = new WaitForSeconds(inputHoldDelay);

	}
	private void Update()
	{
		foreach (Layer layer in layerPriorities)
		{
			var hit = RaycastForLayer(layer);
			if (hit.HasValue)
			{
				m_hit = hit.Value;

				m_layerHit = layer;
			}

			OnLayerChange(m_layerHit);
		}

		if (lastHighlighted != null)
		{
			if (Vector3.Distance(transform.position, lastHighlighted.transform.position) > playerDistanceFromInteractable)
			{
				Deselect();
				isPlayerNearInteractable = false;
			}
			else
			{
				isPlayerNearInteractable = true;
			}
		}

		if (Input.GetButtonDown("Swap Hand"))
		{
			SwitchCurrentHand(currentHand);
		}

		if (!cannotDrop && droppable && (currentHand == PlayerHand.Right ? carriedObjectRight : currentHand == PlayerHand.Left ? carriedObjectLeft : false))
		{
			if (Input.GetButtonDown("Get"))
			{
				DropObject(currentHand);
				return;
			}
		}


		if (m_layerHit == Layer.Pickupable)
		{
			FeedbackObjectAction(m_hit);

			if (!isPlayerNearInteractable) return;

			if (Input.GetButtonDown("Get"))
			{
				if (carriedObjectRight && carriedObjectLeft) return;
				if (!carriedObjectLeft || !carriedObjectRight)
					PickUpObject(m_hit.transform, currentHand);
			}
		}

		if (m_layerHit == Layer.Interactable)
		{
			FeedbackObjectAction(m_hit);

			if (Input.GetButtonDown("Get"))
			{
				InteractableObjectAction(m_hit);
			}

		}
		if (m_layerHit == Layer.Droppable)
		{
			if (Input.GetButtonDown("Get"))
			{
				DroppableObjectAction(m_hit);
			}
		}

		if (m_layerHit == Layer.Observable)
		{
			TriggerLookupAction(Hit);
		}

	}

	private void TriggerLookupAction(RaycastHit hit)
	{
		hit.transform.SendMessageUpwards("LookupAction");
	}

	public bool IsCarryingWithBothHands = false;

	private void PickUpObject(Transform transform, PlayerHand hand)
	{
		Deselect();

		if (transform.GetComponent<PickableObject>().needBothHands && IsBothHandsFree())
		{
			IsCarryingWithBothHands = true;
			SwitchCurrentHand(PlayerHand.Left);

			Rigidbody rb = transform.GetComponent<Rigidbody>();
			carriedObjectRight = transform.gameObject;
			playerInventory.GetItem(carriedObjectRight.GetComponent<PickableObject>(), PlayerHand.Right);

			if (rb == null) return;
			rb.isKinematic = true;
			rb.transform.rotation = Quaternion.identity;
			rb.transform.SetParent(rightHandTransform);
			StartCoroutine(WaitForRightHand());
		}
		else if (transform.GetComponent<PickableObject>().needBothHands && !IsBothHandsFree())
		{
			return;
		}
		else
		{
			Rigidbody rb = transform.GetComponent<Rigidbody>();
			if (hand == PlayerHand.Left)
			{
				carriedObjectLeft = transform.gameObject;
				playerInventory.GetItem(carriedObjectLeft.GetComponent<PickableObject>(), PlayerHand.Left);
			}
			else
			{
				carriedObjectRight = transform.gameObject;
				playerInventory.GetItem(carriedObjectRight.GetComponent<PickableObject>(), PlayerHand.Right);
			}
			if (rb == null) return;
			rb.isKinematic = true;
			rb.transform.rotation = Quaternion.identity;
			if (hand == PlayerHand.Left)
			{
				rb.transform.SetParent(leftHandTransform);
				StartCoroutine(WaitForLeftHand());
			}
			else
			{
				rb.transform.SetParent(rightHandTransform);
				StartCoroutine(WaitForRightHand());
			}
		}

	}


	internal void DropObject(PlayerHand hand)
	{

		Rigidbody rb;
		if (hand == PlayerHand.Left)
		{
			playerInventory.DropItem(carriedObjectLeft.GetComponent<PickableObject>(), PlayerHand.Left);
			movingLeftHand = false;
			rb = carriedObjectLeft.GetComponent<Rigidbody>();
		}
		else
		{
			playerInventory.DropItem(carriedObjectRight.GetComponent<PickableObject>(), PlayerHand.Right);
			movingRightHand = false;
			rb = carriedObjectRight.GetComponent<Rigidbody>();
		}
		rb.transform.parent = null;
		rb.isKinematic = false;
		rb.AddForce(m_Camera.transform.forward * throwForce, ForceMode.Impulse);
		if (hand == PlayerHand.Left)
			carriedObjectLeft = null;
		else
		{
			carriedObjectRight = null;
			IsCarryingWithBothHands = false;
			SwitchCurrentHand(PlayerHand.Left);
		}
	}

	internal void RemoveObject(PlayerHand hand)
	{

		GameObject go;
		if (hand == PlayerHand.Left)
		{
			playerInventory.DropItem(carriedObjectLeft.GetComponent<PickableObject>(), PlayerHand.Left);
			movingLeftHand = false;
			go = carriedObjectLeft.gameObject;
		}
		else
		{
			playerInventory.DropItem(carriedObjectRight.GetComponent<PickableObject>(), PlayerHand.Right);
			movingRightHand = false;
			go = carriedObjectRight.gameObject;
		}
		Destroy(go);

		if (hand == PlayerHand.Left)
			carriedObjectLeft = null;
		else
			carriedObjectRight = null;

	}

	private void SwitchCurrentHand(PlayerHand hand)
	{
		if (IsCarryingWithBothHands)
		{
			currentHand = PlayerHand.Right;
			leftHandUI.enabled = false;
			rightHandUI.enabled = false;
			return;
		}
		else
		{
			leftHandUI.enabled = true;
			rightHandUI.enabled = true;
		}
		switch (hand)
		{
			case PlayerHand.Left:
				currentHand = PlayerHand.Right;
				HighlightHandUI(PlayerHand.Right);
				break;
			case PlayerHand.Right:
				currentHand = PlayerHand.Left;
				HighlightHandUI(PlayerHand.Left);
				break;
			default:
				break;
		}
	}


	private void HighlightHandUI(PlayerHand playerHand)
	{
		switch (playerHand)
		{
			case PlayerHand.Left:
				leftHandUI.color = Color.yellow;
				rightHandUI.color = Color.white;
				break;
			case PlayerHand.Right:
				leftHandUI.color = Color.white;
				rightHandUI.color = Color.yellow;
				break;
			default:
				break;
		}
	}

	private void OnLayerChange(Layer layer)
	{
		switch (layer)
		{
			case Layer.Default:
				Deselect();
				droppable = true;
				break;
			case Layer.Pickupable:
				Deselect();
				Highlight(m_hit.transform);
				break;
			case Layer.Interactable:
				Deselect();
				droppable = false;
				Highlight(m_hit.transform);
				break;
			case Layer.RaycastEndStop:
				Deselect();
				droppable = true;
				break;
			default:
				break;
		}
	}

	private void FeedbackObjectAction(RaycastHit hit)
	{
		if (!isPlayerNearInteractable) return;

		hit.transform.SendMessageUpwards("GetFeedback");
	}

	private void InteractableObjectAction(RaycastHit hit)
	{
		if (!isPlayerNearInteractable) return;


		// Só interage se a mão atual estiver livre
		if (currentHand == PlayerHand.Left && objectInLeftHand == DropplableObject.None)
		{
			hit.transform.SendMessageUpwards("Interaction");
		}
		else if (currentHand == PlayerHand.Right && objectInRightHand == DropplableObject.None)
		{
			hit.transform.SendMessageUpwards("Interaction");
		}
		else if (currentHand == PlayerHand.Right && objectInRightHand == DropplableObject.Picture_1)
		{
			hit.transform.SendMessageUpwards("DropPicture_1");
		}
		else if (currentHand == PlayerHand.Right && objectInRightHand == DropplableObject.Picture_3)
		{
			hit.transform.SendMessageUpwards("DropPicture_3");
		}
		else
		{
			hit.transform.SendMessageUpwards("GetFeedback");
		}

	}

	public bool IsCurrentHandFree()
	{
		return IsThisHandFree(currentHand);
	}

	public bool IsThisHandFree(PlayerHand hand)
	{

		switch (hand)
		{
			case PlayerHand.Left:
				if (objectInLeftHand == DropplableObject.None) return true;
				else return false;
			case PlayerHand.Right:
				if (objectInRightHand == DropplableObject.None) return true;
				else return false;
			default:
				break;
		}

		return false;
	}

	public bool IsBothHandsFree()
	{
		if (IsThisHandFree(PlayerHand.Left) && IsThisHandFree(PlayerHand.Right))
			return true;
		else
			return false;
	}

	private void DroppableObjectAction(RaycastHit hit)
	{
		if (!isPlayerNearInteractable) return;

		if (currentHand == PlayerHand.Left && objectInLeftHand == DropplableObject.Lever)
		{
			hit.transform.SendMessageUpwards("DropObject");
			RemoveObject(PlayerHand.Left);
		}
		else if (currentHand == PlayerHand.Right && objectInRightHand == DropplableObject.Lever)
		{
			hit.transform.SendMessageUpwards("DropObject");
			RemoveObject(PlayerHand.Right);
		}
		else
		{
			hit.transform.SendMessageUpwards("Interact");
		}
	}

	private void FixedUpdate()
	{
		// Animações de pegar o objeto
		if (movingRightHand)
		{
			MeshFilter mf = rightHandTransform.GetComponentInChildren<MeshFilter>();
			if (mf != null)
			{
				mf.transform.localPosition = Vector3.Slerp(mf.transform.localPosition, Vector3.zero, Time.deltaTime * 5f);
			}
		}

		if (movingLeftHand)
		{
			MeshFilter mf = leftHandTransform.GetComponentInChildren<MeshFilter>();
			if (mf != null)
			{
				mf.transform.localPosition = Vector3.Slerp(mf.transform.localPosition, Vector3.zero, Time.deltaTime * 5f);
			}
		}
	}



	/// <summary>
	/// Destaca o Gameobject tocado
	/// </summary>
	/// <param name="t"></param>
	private void Highlight(Transform t)
	{
		if (t.gameObject != lastHighlighted)
		{
			m_hit.transform.SendMessageUpwards("GetFeedback");
			m_hit.transform.SendMessageUpwards("HighlightObject");
		}

		lastHighlighted = t.gameObject;
	}

	/// <summary>
	/// Retira o realce do último gameobject destacado
	/// </summary>
	void Deselect()
	{
		if (lastHighlighted)
		{
			lastHighlighted.transform.SendMessageUpwards("DeselectObject");
			lastHighlighted = null;

			HideFeedback();
		}
	}

	RaycastHit? RaycastForLayer(Layer layer)
	{
		int layerMask = 1 << (int)layer;
		Ray ray = m_Camera.ViewportPointToRay(centerScreen);

		RaycastHit hit;
		bool hasHit = Physics.Raycast(ray, out hit, rayDistance, layerMask);
		if (hasHit)
		{
			return hit;
		}
		return null;
	}

	private IEnumerator WaitForInteraction()
	{
		droppable = false;
		yield return inputHoldWait;
		droppable = true;
	}

	private IEnumerator WaitForRightHand()
	{
		movingRightHand = true;
		yield return inputHoldWait;
		movingRightHand = false;
	}

	private IEnumerator WaitForLeftHand()
	{
		movingLeftHand = true;
		yield return inputHoldWait;
		movingLeftHand = false;
	}

	/// <summary>
	/// Exibe a mensagem enviada para o jogador
	/// </summary>
	/// <param name="message">Mensagem a ser exibida</param>
	public void DisplayFeedback(string message)
	{
		if (ControlIDManager.Instance) FeedbackPanelText.spriteAsset = ControlIDManager.Instance.GetControllerGlyphys();
		FeedbackPanelText.text = message;
		FeedbackPanelCanvasGroup.alpha = 1;
		FeedbackPanelImage.enabled = false;
	}

	/// <summary>
	/// Exibe a mensagem em forma de ícone
	/// </summary>
	/// <param name="messageIcon">Ícone</param>
	public void DisplayFeedback(Sprite messageIcon)
	{
		if (FeedbackPanelImage.enabled) return;
		FeedbackPanelCanvasGroup.alpha = 1;
		FeedbackPanelImage.color = new Color(255f, 255f, 255f, 255f);
		FeedbackPanelImage.sprite = messageIcon;
		FeedbackPanelImage.enabled = true;
	}

	/// <summary>
	/// Exibe a mensagem com texto e ícone
	/// </summary>
	/// <param name="message">Mensagem</param>
	/// <param name="messageIcon">Ícone</param>
	public void DisplayFeedback(string message, Sprite messageIcon)
	{
		if (ControlIDManager.Instance) FeedbackPanelText.spriteAsset = ControlIDManager.Instance.GetControllerGlyphys();
		FeedbackPanelText.text = message;
		FeedbackPanelCanvasGroup.alpha = 1;
		FeedbackPanelImage.color = new Color(255f, 255f, 255f, 100f);
		FeedbackPanelImage.sprite = messageIcon;
		FeedbackPanelImage.enabled = true;
	}

	/// <summary>
	/// Oculta a mensage de feedback em x segundos
	/// </summary>
	/// <param name="time"></param>
	IEnumerator HideFeedback(float time)
	{
		yield return new WaitForSeconds(time);
		HideFeedback();
	}

	/// <summary>
	/// Oculta a mensagem de feedback
	/// </summary>
	public void HideFeedback()
	{
		FeedbackPanelText.text = "";
		FeedbackPanelCanvasGroup.alpha = 0f;
		FeedbackPanelImage.sprite = null;
		FeedbackPanelImage.enabled = false;
	}

}

using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    public bool ClintCharacter = false;

    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private CameraController _cameraControllerPrefab;
    [field: SerializeField] public CanvasRotator CanvasRotator { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public CharacterController CharacterController { get; private set; }
    public Material Material { get; private set; }
    public AttackReaction AttackReaction { get; private set; }
    public DashAction DashAction { get; private set; }
    public CameraController CameraController { get; private set; }
    public CharacterActionType CharacterActionType { get; private set; } = CharacterActionType.Idle;
    [field: SerializeField] public bool CharacterReadyForUseAction { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        CharacterController = GetComponent<CharacterController>();
        AttackReaction = GetComponent<AttackReaction>();
        DashAction = GetComponent<DashAction>();
        Material = GetComponentInChildren<SkinnedMeshRenderer>().material;

        CharacterReadyForUseAction = true;
    }

    private void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)
                && CharacterReadyForUseAction
                && !DashAction.Dashing)
            {
                DashAction.CmdStartDash();
            }
        }
    }

    private void Start()
    {
        if (isLocalPlayer)
        {
            Vector3 cameraSpawnPosition = transform.position;
            cameraSpawnPosition.y = 1.4f;
            CameraController = Instantiate(_cameraControllerPrefab, cameraSpawnPosition, transform.rotation);
            CameraController.SetupTarget(_cameraTarget);
            CanvasRotator.SetCamera(CameraController.Camera);
        }
    }

    public void SetCharacterAction(CharacterActionType characterActionType)
    {
        if (CharacterActionType == characterActionType)
            return;

        CharacterActionType = characterActionType;
    }

    [ClientRpc]
    public void RpcSetPosition(Vector3 position)
    {
        transform.position = position;
    }
}

using UnityEngine;

public class PlayerMovements : PlayerAction
{
    [SerializeField] private float _speed = 10;

    private void Update()
    {
        if (isLocalPlayer)
        {
            float axisZ = Input.GetAxis("Vertical");
            float axisX = Input.GetAxis("Horizontal");

            if (Player.DashAction.Dashing)
                return;

            if (axisZ != 0 || axisX != 0)
            {
                transform.forward = Vector3.Slerp(
                    Player.transform.forward,
                    new Vector3(Player.CameraController.transform.forward.x, 0, Player.CameraController.transform.forward.z),
                    _speed * Time.deltaTime
                    );

            }

            Vector3 movement = (Player.transform.forward * axisZ + Player.transform.right * axisX) * _speed;
            Player.CharacterController.SimpleMove(movement);

            Player.Animator.SetFloat("Run", Mathf.Abs(axisZ + axisX));
        }
    }
}

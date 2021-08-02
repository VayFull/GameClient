using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameClient.Scripts.Shoot.Calculators.ShootResults;
using GameClient.Scripts.Shoot.Types;
using GameClient.Scripts.Shoot.Weapons;
using UnityEngine;

namespace GameClient.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        public float walkingSpeed = 7.5f;
        public float runningSpeed = 11.5f;
        public float jumpSpeed = 8.0f;
        public float gravity = 20.0f;
        public Camera playerCamera;
        public float lookSpeed = 2.0f;
        public float lookXLimit = 45.0f;

        private CharacterController _characterController;
        Vector3 moveDirection = Vector3.zero;
        float rotationX = 0;
        private Vector3 _previousPosition;
        private Vector3 _previousRotation;

        [HideInInspector] public bool canMove = true;

        private Communicator Communicator;
        public GameObject GameMenu;

        public List<Weapon> Weapons;
        public Weapon ActiveWeapon;

        void Start()
        {
            Weapons = new List<Weapon>();
            Weapons.Add(new AkWeapon(new FirearmsType()));
            Weapons.Add(new KnifeWeapon(new MeleeType()));

            ActiveWeapon = Weapons[0];
            
            Communicator = GameObject.FindWithTag("GameController").GetComponent<Communicator>();
            _characterController = GetComponent<CharacterController>();

            _previousPosition = transform.position;
            _previousRotation = transform.rotation.eulerAngles;
        }

        void Update()
        {
            if (Communicator.ConnectedToServer)
            {
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                Vector3 right = transform.TransformDirection(Vector3.right);
                bool isRunning = Input.GetKey(KeyCode.LeftShift);
                float curSpeedX =
                    canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
                float curSpeedY =
                    canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
                float movementDirectionY = moveDirection.y;
                moveDirection = (forward * curSpeedX) + (right * curSpeedY);

                if (Input.GetButton("Jump") && canMove && _characterController.isGrounded)
                {
                    moveDirection.y = jumpSpeed;
                }
                else
                {
                    moveDirection.y = movementDirectionY;
                }

                if (!_characterController.isGrounded)
                {
                    moveDirection.y -= gravity * Time.deltaTime;
                }

                _characterController.Move(moveDirection * Time.deltaTime);

                if (canMove)
                {
                    rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                    rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                    playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                    transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
                }

                var playerTransform = transform;
                var position = playerTransform.position;
                var rotation = playerTransform.rotation.eulerAngles;

                if (_previousPosition != position || _previousRotation != rotation)
                {
                    SendCurrentPositionAndRotation(position, rotation);
                    _previousPosition = position;
                    _previousRotation = rotation;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
                GameMenu.SetActive(!GameMenu.activeInHierarchy);
            
            if (Input.GetKey(KeyCode.Mouse0))
            {
                var shootResult = ActiveWeapon.Shoot(playerCamera.transform, Communicator.Id);
                Communicator.HandleShootResult(shootResult);
            }
        }

        private void SendCurrentPositionAndRotation(Vector3 currentPosition, Vector3 currentRotation)
        {
            var bytes = Encoding.ASCII.GetBytes(
                $"pos={Communicator.Id}&pos?{currentPosition.x}:{currentPosition.y}:{currentPosition.z}" +
                $"?{currentRotation.x}:{currentRotation.y}:{currentRotation.z}");
            Communicator.Send(bytes);
        }
    }
}
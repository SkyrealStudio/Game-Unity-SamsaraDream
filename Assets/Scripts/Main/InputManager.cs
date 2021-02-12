using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace enumNameSpace
{
    public enum EnumStatus
    {
        Left = 0,
        Right = 1,
        Up = 2,
        Down = 3,
        Jump = 4,
        Interact = 5
    }
}

namespace inputManager 
{
    public class InputManager : MonoBehaviour
    {
        public enum EnumStatus
        {
            Left = 0,
            Right = 1,
            Up = 2,
            Down = 3,
            Jump = 4,
            Interact = 5
        }

        //Jump 和 Interact 由Gamemanager来轮询....

        private readonly KeyCode m_kcLeft = KeyCode.A;
        private readonly KeyCode m_kcRight = KeyCode.D;
        private readonly KeyCode m_kcUp = KeyCode.W;
        private readonly KeyCode m_kcDown = KeyCode.S;
        private readonly KeyCode m_kcJump = KeyCode.Space;
        private readonly KeyCode m_kcInteract = KeyCode.J;

        private bool allowUI;
        private bool priv_jumpLocker = false;

        public GameManager gm;
        public Buttoner[] buttoners;

        internal bool[] status;

        private void Start()
        {
            EnumStatus e = new EnumStatus();
            status = new bool[System.Enum.GetNames(e.GetType()).Length];
            allowUI = gm.g_allowUI;
        }

        void Update()
        {
            if(!allowUI)
            {
                //Left Right
                if (Input.GetKey(m_kcLeft) == Input.GetKey(m_kcRight) == true)
                    status[(int)EnumStatus.Left] = status[(int)EnumStatus.Right] = false;
                else
                {
                    status[(int)EnumStatus.Left] = Input.GetKey(m_kcLeft);
                    status[(int)EnumStatus.Right] = Input.GetKey(m_kcRight);
                }

                //Up Down
                if (Input.GetKey(m_kcUp) == Input.GetKey(m_kcDown) == true)
                    status[(int)EnumStatus.Up] = status[(int)EnumStatus.Down] = false;
                else
                {
                    status[(int)EnumStatus.Up] = Input.GetKey(m_kcUp);
                    status[(int)EnumStatus.Down] = Input.GetKey(m_kcDown);
                }

                //Jump Interact
                status[(int)EnumStatus.Jump] = Input.GetKeyDown(m_kcJump);
                status[(int)EnumStatus.Interact] = Input.GetKey(m_kcInteract);
            }
            else
            {
                //Left Right
                if (buttoners[(int)EnumStatus.Left] == buttoners[(int)EnumStatus.Right])
                    status[(int)EnumStatus.Left] = status[(int)EnumStatus.Right] = false;
                else
                {
                    status[(int)EnumStatus.Left] = buttoners[(int)EnumStatus.Left].pressed;
                    status[(int)EnumStatus.Right] = buttoners[(int)EnumStatus.Right].pressed;
                }

                //Up Down
                if (buttoners[(int)EnumStatus.Up] == buttoners[(int)EnumStatus.Down])
                    status[(int)EnumStatus.Up] = status[(int)EnumStatus.Down] = false;
                else
                {
                    status[(int)EnumStatus.Up] = buttoners[(int)EnumStatus.Up].pressed;
                    status[(int)EnumStatus.Down] = buttoners[(int)EnumStatus.Down].pressed;
                }

                //Jump
                if (buttoners[(int)EnumStatus.Jump].pressed && priv_jumpLocker == false)
                {
                    priv_jumpLocker = true;
                    status[(int)EnumStatus.Jump] = true;
                }
                else if (!buttoners[(int)EnumStatus.Jump].pressed && priv_jumpLocker == true)
                {
                    priv_jumpLocker = false;
                }

                //Interact
                status[(int)EnumStatus.Interact] = buttoners[(int)EnumStatus.Interact].pressed;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ooparts.dungen;

namespace ooparts.dungen
{
    public class PlayerMove : MonoBehaviour
    {
        public float speed;

        private float gravityForce;
        private Vector3 moveVector;

        private CharacterController ch_controller;
        private Animator ch_animator;


        void Start()
        {
            transform.localScale *= RoomMapManager.TileSize / 2;
            ch_controller = GetComponent<CharacterController>();
            ch_animator = GetComponent<Animator>();
        }

        void Update()
        {
            CharacterMove();
            GamingGravity();          
            //if (Input.GetKey(KeyCode.W))
            //{
            //rig.AddForce(Vector3.forward * speed * Time.deltaTime);
            //}
            //if (Input.GetKey(KeyCode.S))
            //{
            // rig.AddForce(Vector3.back * speed * Time.deltaTime);
            // }
            // if (Input.GetKey(KeyCode.A))
            // {
            //     rig.AddForce(Vector3.left * speed * Time.deltaTime);
            // }
            //if (Input.GetKey(KeyCode.D))
            // {
            //     rig.AddForce(Vector3.right * speed * Time.deltaTime);
            // }
            // if (Input.GetKeyDown(KeyCode.Space) && Physics.Raycast(transform.position, Vector3.down, 5.5f)) //0.55
            // {
            //     rig.AddForce(Vector3.up * jump);
            // }
        }

        private void CharacterMove()
        {
            moveVector = Vector3.zero;
            moveVector.x = Input.GetAxis("Horizontal") * speed;
            moveVector.z = Input.GetAxis("Vertical") * speed;
            if (moveVector.x != 0 || moveVector.z != 0) ch_animator.SetBool("Move", true);
            else ch_animator.SetBool("Move", false);
            if(Vector3.Angle(Vector3.forward, moveVector) > 1f || Vector3.Angle(Vector3.forward, moveVector) == 0)
            {
                Vector3 direct = Vector3.RotateTowards(transform.forward, moveVector, speed, 0.0f);
                transform.rotation = Quaternion.LookRotation(direct);
            } 

            moveVector.y = gravityForce;
            ch_controller.Move(moveVector * Time.deltaTime);
        }

        private void GamingGravity()
        {
            if (!ch_controller.isGrounded) gravityForce -= 20f * Time.deltaTime;
            else gravityForce = -1f;
        }
    }
}

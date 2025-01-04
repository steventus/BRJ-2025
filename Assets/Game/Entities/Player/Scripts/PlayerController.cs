using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] ParryAbility parry;
    [SerializeField] BulletTime bulletTime;
    [SerializeField] SpinAttack spinAttack;
    [SerializeField] float moveSpeed;
    Vector3 moveVector, aimVector;
    bool canMove = true;
    void OnEnable() {
        Game.events.OnBulletTimeStart += DisableMovement;
        Game.events.OnBulletTimeEnd += EnableMovement;
    }
    void OnDisable() {
        Game.events.OnBulletTimeStart -= DisableMovement;
        Game.events.OnBulletTimeEnd -= EnableMovement;
    }
    void Update()
    {
        GetMoveInput();
        Move();
        
        if (Input.GetKey(KeyCode.Space))
        {
            //parry / dodge
            parry.Parry();
        }
        
        if (Input.GetKey(KeyCode.LeftShift)) {
            bulletTime.Activate();

            spinAttack.ChargeSpin();
        }
        else {
            bulletTime.Deactivate();

            spinAttack.UnleashSpin();
        }
    }

    private void GetMoveInput()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        moveVector = new(horizontalInput, verticalInput);
    }

    private void Move()
    {
        if(!canMove) return;

        transform.Translate(moveVector.normalized * moveSpeed * Time.deltaTime);
    }

    void DisableMovement() {
        canMove = false;
    }
    void EnableMovement() {
        canMove = true;
    }
}

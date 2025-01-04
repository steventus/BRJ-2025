using UnityEngine;

public class SpecialBulllet : Bullet
{
    bool directionChosen = false;
    Vector3 direction;
    [SerializeField] float range;
    protected override void Move() {
        if (!directionChosen) {
            direction = transform.position + (Vector3.right * range);
            directionChosen = true;
        }

        Vector3 moveVector = Vector3.Slerp(transform.position, direction, moveSpeed * Time.deltaTime);
        transform.position = moveVector;
    }
}
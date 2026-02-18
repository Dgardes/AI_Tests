using UnityEngine;

public class MyScriptAI : MonoBehaviour
{
    public Transform target;
    public float speed = 5f;
    public float rayDistance = 1f;
    public float sideRayDistance = 0.3f;
    public float sideStepWidth = 0.6f;

    private bool goLeft = true;

    void Update()
    {
        if (target == null) return;

        //пулучаем местоположение цели и положение персонажа
        Vector3 targetPos = new Vector3(target.position.x, target.position.y, 0);
        Vector3 currentPos = new Vector3(transform.position.x, transform.position.y, 0);
        Vector3 dirToTarget = (targetPos - currentPos).normalized;

        //вектор "право" относительно направления на цель
        Vector3 rightDir = Vector3.Cross(dirToTarget, Vector3.forward).normalized;
        //вкектор "лево" относительно направления на цель
        Vector3 leftDir = -rightDir;

        // Точки вылета габаритных лучей в стороны
        Vector3 leftShoulder = transform.position + (leftDir * sideStepWidth);
        Vector3 rightShoulder = transform.position + (rightDir * sideStepWidth);

        // Пускаем 4 луча 
        RaycastHit hitFrontL, hitFrontR, hitSideL, hitSideR;
        // 2 габаритных луча прямо на цель
        bool frontL = Physics.Raycast(leftShoulder, dirToTarget, out hitFrontL, rayDistance);
        bool frontR = Physics.Raycast(rightShoulder, dirToTarget, out hitFrontR, rayDistance);

        // 2 боковых луча в стороны
        bool sideL = Physics.Raycast(leftShoulder, leftDir, out hitSideL, sideRayDistance);
        bool sideR = Physics.Raycast(rightShoulder, rightDir, out hitSideR, sideRayDistance);
        
        //вектор движения прямо на цель
        Vector3 moveDir = dirToTarget;

        //если хоть один габаритный луч упёрся в стену
        if (frontL || frontR)
        {
            if(goLeft)
            { moveDir = leftDir; }
            if(!goLeft)
            { moveDir = rightDir; }

            if (sideL) 
            { goLeft = false; }
            if (sideR) 
            { goLeft = true; }
        }

        // движение
        Vector3 velocity = moveDir.normalized * speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x + velocity.x, transform.position.y + velocity.y, transform.position.z);
        
        Debug.DrawRay(leftShoulder, dirToTarget * rayDistance, frontL ? Color.red : Color.blue);
        Debug.DrawRay(rightShoulder, dirToTarget * rayDistance, frontR ? Color.red : Color.blue);
        Debug.DrawRay(leftShoulder, leftDir * sideRayDistance, sideL ? Color.red : Color.blue);
        Debug.DrawRay(rightShoulder, rightDir * sideRayDistance, sideR ? Color.red : Color.blue);
    }
}

using System.Collections;
using UnityEngine;

public class MonsterFSM : CharacterFSM {
    private MonsterBase monsterBase;

    private float moveSpeed = 8f, rotateSpeed = 3f;
    public float DistanceFromPlayer => Vector3.Distance(transform.position, playerBase.transform.position);

    private bool PlayerInAttackRange => DistanceFromPlayer <= 10 && playerBase.transform.position.y - transform.position.y < 1 && !playerBase.IsDie
                 && playerBase.PlayerInMonsterRange(monsterBase.limitRange_Min, monsterBase.limitRange_Max);

    protected override void Awake() {
        base.Awake();
        monsterBase = GetComponent<MonsterBase>();
    }

    protected IEnumerator Idle() {
        do {
            yield return null;

            //Jika jarak ke pemain kurang dari 10 dan perbedaan ketinggian kurang dari 1
            if (PlayerInAttackRange && !playerBase.IsDie)
                SetState(CharacterState.Trace);

            //kondisi kalau berjalan
            else if(Random.Range(1, 20) == 2)
                SetState(CharacterState.Walk);
        } while(!IsNewState);
    }

    protected IEnumerator Walk() {
        Vector3 movePos = new Vector3(Random.Range(-1, 2), 0, Random.Range(-1, 2));

        do {
            yield return null;
            MoveController.LookDirection(transform, movePos);
            MoveController.RigidMovePos(transform, movePos, moveSpeed);
            MoveController.LimitMoveRange(transform, monsterBase.limitRange_Min, monsterBase.limitRange_Max);

            //Jika jarak ke pemain kurang dari 10 dan perbedaan ketinggian kurang dari 1
            if (PlayerInAttackRange && !playerBase.IsDie)
                SetState(CharacterState.Trace);

            //kondisi kalau idle dengan probabilistas random
            else if (Random.Range(1, 30) == 2)
                SetState(CharacterState.Idle);
        } while(!IsNewState);
    }

    protected IEnumerator Trace() {
        do {
            yield return null;
            if(!playerBase.IsJumping) {
                MoveController.LookTarget(transform, playerBase.transform, rotateSpeed);
                MoveController.RigidMovePos(transform, playerBase.transform.position - transform.position, moveSpeed);
                MoveController.LimitMoveRange(transform, monsterBase.limitRange_Min, monsterBase.limitRange_Max);
            }

            //Jika jarak dari pemain lebih dari 10 atau perbedaan ketinggian lebih dari 1, status user akan menjadi siaga.
            if (!PlayerInAttackRange)
                SetState(CharacterState.Idle);

            //kondisi kalau misal user bertabrakan dengan enemy, status user akan menjadi attack
            if(characterBase.CheckRaycastHit("Player") && !characterBase.IsColliderDie)
                SetState(CharacterState.Attack);
        } while(!IsNewState);
    }

    protected IEnumerator Attack() {
        do {
            yield return null;
            MoveController.LookTarget(transform, playerBase.transform, rotateSpeed);

            bool raycastTarget = characterBase.AttackToTarget("Player");
            if(!raycastTarget) {
                if(playerBase.IsDie)
                    SetState(CharacterState.Walk);
                else
                    SetState(CharacterState.Trace);
            }
        } while(!IsNewState);
    }
}
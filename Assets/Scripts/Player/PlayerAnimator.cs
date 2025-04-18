using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAnimator : MonoBehaviour
{
    

    private PlayerMovement _player;
    private GeneratedCharacterSize _character;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Animator _anim;

    private void OnEnable()
    {
        _player.GroundedChanged += OnGroundedChanged;
        _player.RollChanged += OnRollChanged;
        _player.Jumped += OnJumped;        
        _player.AttackStart += OnAttackChanged;
        _player.AttackReleased += OnAttackExecuted;
    }
    private void Awake() {
        _player = GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //Handle sprite flip
        if (_player.Input.x != 0) _sprite.flipX = !_player.FacingRight;

        //Handle move
        _anim.SetFloat("Speed", Mathf.Abs(_player.Velocity.x));
        _anim.SetFloat("VSpeed", _player.Velocity.y);

        //Handle crouching
        _anim.SetBool("Crouching", _player.Crouching);


    }
    
    private void OnGroundedChanged(bool grounded, float impact){
        _anim.SetBool("Grounded", grounded);
        if(grounded){
            _anim.ResetTrigger("Jump");
        }
    }

    private void OnJumped(JumpType jumpType){
        _anim.SetTrigger("Jump");
    }

    private void OnRollChanged(bool rolling, Vector2 dir){
        //Debug.Log(rolling);
        if(rolling){
            _anim.SetTrigger("Roll");
        }
        else{
            _anim.ResetTrigger("Roll");
        }
    }

    private void OnAttackChanged(bool attacking){
        //Debug.Log(attacking);
        if(attacking){
            _anim.SetTrigger("Charging");
        }
        else{
            _anim.ResetTrigger("Charging");
        }
    }

    private void OnAttackExecuted(AttackType attack){
        _anim.SetInteger("AttackType", (int)attack);
    }

}

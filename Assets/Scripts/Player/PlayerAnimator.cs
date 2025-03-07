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
    }
    private void Awake() {
        _player = GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //Handle sprite flip
        if (_player.Input.x != 0) _sprite.flipX = _player.Input.x < 0;

        //Handle move
        _anim.SetFloat("Speed", Mathf.Abs(_player.Velocity.x));
        _anim.SetFloat("VSpeed", _player.Velocity.y);

        //Handle crouching
        _anim.SetBool("Crouching", _player.Crouching);
        // Debug.Log(_player.Crouching);


    }
    
    private void OnGroundedChanged(bool grounded, float impact){
        _anim.SetBool("Grounded", grounded);
    }

}

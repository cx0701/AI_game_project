using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class RoleManagement : MonoBehaviour
{
    private Animator player_Ani;
    private Rigidbody2D rb; 
    bool isLeft;
    bool isRight;
    bool isUp;
    bool isDown;
    private Vector3 PlayerPos; 
    private Vector2 moveX;
    void Start()
    {
        isLeft = false;
        isRight = false;
        isUp = false;
        isDown= false;
        player_Ani = this.gameObject.GetComponent<Animator>();
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    public void ToLeft(InputAction.CallbackContext ctx)
    {
        //print(ctx);
        if (ctx.performed){
            isRight = false;
            isLeft = true;
            SetAnimator();
            MoveOnX(-2);
        }else{
            //Canceled
            isLeft = false;
            SetAnimator();
        }
    }
    public void ToRight(InputAction.CallbackContext ctx)
    {
        //print(ctx);
        if (ctx.performed){
            isLeft = false;
            isRight = true;
            SetAnimator();
            MoveOnX(2);
        }else{
            //Canceled
            isRight = false;
            SetAnimator();
        }
    }
    public void Up(InputAction.CallbackContext ctx)
    {
        //print(ctx);
        if (ctx.performed){
            isUp = true;
            isDown = false;
            SetAnimator();
            MoveOnY(2);
        }else{
            //Canceled
            isUp = false;
            SetAnimator();
        }
    }
    public void Down(InputAction.CallbackContext ctx)
    {
        if (ctx.performed){
            isDown = true;
            isUp = false;
            SetAnimator();
            MoveOnY(-2);
        }else{
            //Canceled
            isDown = false;
            SetAnimator();
        }
    }
    void SetAnimator()
    {
        player_Ani.SetBool("Left", isLeft);
        player_Ani.SetBool("Right", isRight);
        player_Ani.SetBool("Up", isUp);
        player_Ani.SetBool("Down", isDown);
    }
    void MoveOnX(float speed){
        PlayerPos = this.transform.position;
        transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
    }
    void MoveOnY(float speed){
        PlayerPos = this.transform.position;
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);
    }
    private void Update() {
        if(isLeft){
            MoveOnX(-2);
        }else if(isRight){
            MoveOnX(2);
        }else if(isUp){
            MoveOnY(2);
        }else if(isDown){
            MoveOnY(-2);
        }
    }
}

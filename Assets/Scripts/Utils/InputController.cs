using System;
using Coin;
using DG.Tweening;
using Lean.Pool;
using Units;
using UnityEngine;

namespace Utils
{
    public class InputController : MonoBehaviour
    {
        private void Update()
        {
            // Check if 2d object is clicked or not
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Mouse is pressed down");
                Camera cam = Camera.main;

                //Raycast depends on camera projection mode
                Vector2 origin = Vector2.zero;
                Vector2 dir = Vector2.zero;

                if (cam.orthographic)
                {
                    origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                else
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    origin = ray.origin;
                    dir = ray.direction;
                }

                RaycastHit2D hit = Physics2D.Raycast(origin, dir);

                //Check if we hit anything
                if (!hit)
                {
                    Debug.Log("click outside");
                    return;
                }
                
                if (hit.collider.TryGetComponent(out Tower tower))
                {
                    Debug.Log("Tower is clicked");        
                }
                else if (hit.collider.TryGetComponent(out BuildSpot buildSpot))
                {
                    Debug.Log("BuildSpot is clicked");        
                } 
                else if (hit.collider.TryGetComponent(out Enemy enemy))
                {
                    Debug.Log("Enemy is clicked");   
                }
                else if (hit.collider.TryGetComponent(out CoinView coin))
                {
                    Debug.Log("Coin is clicked");
                    var extraCoin = coin.Amout;
                    LeanPool.Despawn(coin.gameObject);
                    EventManager.GetInstance().Notify(Events.UpdateMoney, extraCoin);
                }
                else
                {
                    Debug.Log("We hit " + hit.collider.name);
                }
            }
        }
    }
}
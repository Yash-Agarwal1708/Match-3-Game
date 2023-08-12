using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFront : MonoBehaviour
{
    private static TileFront selected1; // 1 
    private static TileFront selected2;
    private static TileFront selected;
    private SpriteRenderer Renderer; // 2
    public Vector3Int Position;
    // Start is called before the first frame update
    void Start()
    {
        Renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Select(SpriteRenderer tempRenderer1) // 4
    {
    tempRenderer1.color = Color.grey;
    }

    public void Unselect(SpriteRenderer tempRenderer) // 5 
    {
    tempRenderer.color = Color.white;
    }

// private void OnMouseDown() //6
//     {
//     if(selected != null)
//     {
//         if (selected == this)
//             return;
//         selected.Unselect();
//         if (Vector2Int.Distance(selected.Position, Position) == 1)
//         {
//             GridManager.Instance.SwapTiles(Position, selected.Position);
//             selected = null;
//         } else {
//             selected = this;
//             Select();
//         }
//     }
//     selected = this;
//     Select();
//     }
private void OnMouseDown() //6
    {
    SoundManager.Instance.PlaySound(SoundType.TypeSelect);
        // print(Position.x+","+Position.y+","+Position.z);
    if(selected1 == null)
    {
    if(selected2==this)
        {
        SpriteRenderer Renderertemp= GridManagerFront.Instance.GetSpriteRendererAt(selected2.Position.x,selected2.Position.y,selected2.Position.z);
        Unselect(Renderertemp);
        selected2=null;
        return;
        }
    selected1 = this;
    SpriteRenderer Renderer1= GridManagerFront.Instance.GetSpriteRendererAt(selected1.Position.x,selected1.Position.y,selected1.Position.z);
    Select(Renderer1);
    // print(selected1.Position);
    }
    else
    {
        if(selected2==null)
        {
           if(selected1==this)
           {
            SpriteRenderer Renderertemp= GridManagerFront.Instance.GetSpriteRendererAt(selected1.Position.x,selected1.Position.y,selected1.Position.z);
            Unselect(Renderertemp);
            selected1=null;
            return;
           }
           selected2 = this;
           SpriteRenderer Renderer2= GridManagerFront.Instance.GetSpriteRendererAt(selected2.Position.x,selected2.Position.y,selected2.Position.z);
           Select(Renderer2); 
        }
        else
        {
            if(selected2==this)
            {
            SpriteRenderer Renderertemp= GridManagerFront.Instance.GetSpriteRendererAt(selected2.Position.x,selected2.Position.y,selected2.Position.z);
            Unselect(Renderertemp);
            selected2=null;
            return;
            }
            if(selected1==this)
           {
            SpriteRenderer Renderertemp= GridManagerFront.Instance.GetSpriteRendererAt(selected1.Position.x,selected1.Position.y,selected1.Position.y);
            Unselect(Renderertemp);
            selected1=null;
            return;
           }
            SpriteRenderer Renderer1= GridManagerFront.Instance.GetSpriteRendererAt(selected1.Position.x,selected1.Position.y,selected1.Position.z);
            SpriteRenderer Renderer2= GridManagerFront.Instance.GetSpriteRendererAt(selected2.Position.x,selected2.Position.y,selected2.Position.z);
            SpriteRenderer Renderer3= GridManagerFront.Instance.GetSpriteRendererAt(Position.x,Position.y,Position.z);
            if((Renderer1.sprite==Renderer2.sprite)&&(Renderer2.sprite==Renderer3.sprite))
            {
                Renderer1.sprite = null;
                Renderer2.sprite = null;
                Renderer3.sprite = null;
                // GridManagerFront.Instance.NumMoves--;
                GridManagerFront.Instance.Score+=30;
                GridManagerFront.Instance.TimeLeft+=0.25f;
                SoundManager.Instance.PlaySound(SoundType.TypePop);
            }
            Unselect(Renderer1);
            Unselect(Renderer2);
            selected1=null;
            selected2=null;
        }
    }
    }
    }

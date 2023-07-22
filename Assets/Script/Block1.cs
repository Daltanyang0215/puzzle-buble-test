using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public enum State
{
    Ready,
    Move,
    End
}

public class Block1 : MonoBehaviour
{
    public State state;
    public Type type;
    public Color color;

    Vector2 movevector;

    private void Start()
    {
        //type = (Type)Random.Range(0, 4);
        //color = HexegonBubleManager.instance.blockcolor[(int)type];
        //GetComponent<SpriteRenderer>().color = color;

        type = (Type)HexegonBubleManager.instance.nextblockindex;

        HexegonBubleManager.instance.nextblockindex = Random.Range(0, 4);
        HexegonBubleManager.instance.nextBlock.color = HexegonBubleManager.instance.blockcolor[HexegonBubleManager.instance.nextblockindex];


        color = HexegonBubleManager.instance.blockcolor[(int)type];
        GetComponent<SpriteRenderer>().color = color;


    }
    private void Update()
    {
        switch (state)
        {
            case State.Ready:
                if (Input.GetMouseButtonUp(0))
                {
                    Vector2 mos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if ((mos.x > -1) && (mos.x < 11) && (mos.y > -9) && (mos.y < 1))
                    {
                        float engle = 0;
                        if (Vector2.Distance( HexegonBubleManager.instance.savevector , mos)<0.1f)
                        {
                            engle = HexegonBubleManager.instance.GetAngle(HexegonBubleManager.instance.firespwan.transform.position, mos) * Mathf.PI / 180;
                        }
                        else
                        {
                            engle = HexegonBubleManager.instance.GetAngle(HexegonBubleManager.instance.savevector, mos);
                            if (engle < -90) engle += 360;
                            engle = Mathf.Clamp(engle, 5, 175);
                            engle *= Mathf.PI / 180;
                        }
                        //engle = Mathf.Clamp(engle, 5, 175);
                        //Debug.Log(engle);
                        movevector = new Vector2(Mathf.Cos(engle), Mathf.Sin(engle)) * 0.5f;
                        state = State.Move;
                        HexegonBubleManager.instance.fireending = true;
                    }
                }
                break;
            case State.End:
                
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        if (state == State.Move)
        {
            transform.Translate(movevector*Time.deltaTime*36);
            if((transform.position.x < 0.1f)||(transform.position.x > 9.9f))
            {
                movevector.x = -movevector.x;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if(((collision.tag == "Block")||(collision.tag == "Wall"))&&(state==State.Move))
            {
                transform.Translate(Vector2.zero);
                state = State.End;
                if((Mathf.Round(Mathf.Abs(transform.position.y) / 0.866f) % 2 == 1) == HexegonBubleManager.instance.offsetspwan)
                {
                    Vector2 pos = new Vector2(Mathf.RoundToInt(transform.position.x - 0.5f) + 0.5f, Mathf.Round(Mathf.Abs(transform.position.y) / 0.866f) * -0.866f);
                    RaycastHit2D ray = Physics2D.Raycast(pos, Vector2.zero);
                    if (ray.collider != null)
                    {
                        if((ray.collider.tag == "Block")&&(ray.collider.gameObject != this.gameObject))
                        {
                            pos = new Vector2(Mathf.RoundToInt(transform.position.x-movevector.x - 0.5f) + 0.5f, Mathf.Round(Mathf.Abs(transform.position.y - movevector.y) / 0.866f) * -0.866f);
                        }
                    }
                    transform.position = pos;
                }
                else
                {
                    Vector2 pos = new Vector2(Mathf.RoundToInt(transform.position.x ), Mathf.Round(Mathf.Abs(transform.position.y) / 0.866f) * -0.866f);
                    RaycastHit2D ray = Physics2D.Raycast(pos, Vector2.zero);
                    if (ray.collider != null)
                    {
                        if ((ray.collider.tag == "Block") && (ray.collider.gameObject != this.gameObject))
                        {
                            pos = new Vector2(Mathf.RoundToInt(transform.position.x - movevector.x), Mathf.Round(Mathf.Abs(transform.position.y) / 0.866f) * -0.866f);
                        }
                    }
                    transform.position = pos;
                }
                GetComponent<CircleCollider2D>().radius = 0.48f;

                Invoke("BlockEnd", 0.1f);
            }
        }
    }

    public void BlockEnd()
    {
        HexegonBubleManager.instance.Fire_End_Coroutine(transform.position);
    }

//    private void OnDrawGizmos()
//    {
////        for (int j = 0; j <= 6; j++)
//        {
//            Gizmos.color = new Color32(145, 244, 139, 210);

//            //Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + Mathf.Sin((90 + 60 * j) * Mathf.PI / 180), transform.position.y + Mathf.Cos((90 + 60 * j) * Mathf.PI / 180)));
//        }
//        Gizmos.d(transform.position, 1);

//    }
}

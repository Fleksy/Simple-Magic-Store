using UnityEngine;

public class CustomerScript : MonoBehaviour
{
    public GameManagerScript gamaManager;
    public Animator animator;
    private int speed = 3;
    public Vector3[] positions;
    public  int posNumber;
    public int posFinal;
    public bool moveFlag;
    public double waitingTime;

    void Update()
    {
        Animator();
            if (moveFlag)
                transform.position =
                    Vector3.MoveTowards(transform.position, positions[posNumber], speed * Time.deltaTime);
            if (transform.position == positions[posNumber])
            {
                posNumber++;
                if (posNumber == posFinal)
                {
                    moveFlag = false;
                    if (posFinal == 10)
                    {
                        gamaManager.customers.Remove(gamaManager.customers[0]);
                        DestroyImmediate(gameObject);
                    }
                }
            }

            if (!GameObject.Find("Canvas").transform.GetChild(3).gameObject.activeSelf)
            {
                waitingTime += Time.deltaTime;
            }

            if (waitingTime >= 120)
            {
                waitingTime = 0;
                gamaManager.ShowMessage("Клиент рассердился и ушел");
               gamaManager.MoveQueue();
            }
         
    }

    public void GoBeginOfQueue()
    {
        Rigidbody2D rgb = transform.GetComponent<Rigidbody2D>();
        rgb.transform.Translate(positions[0]);
        posNumber = 0;
        posFinal = 2;
        moveFlag = true;
        animator = GetComponent<Animator>();
        Animator();
    }

    public void GoAway()
    {
        posFinal = 10;
        moveFlag = true;
    }

    void Animator()
    {
        animator.SetBool("walkDown", false);
        animator.SetBool("walkUp", false);
        animator.SetBool("walkLeft", false);
        animator.SetBool("walkRight", false);
        // костыль
        if ( (posFinal == 7))
        {
            animator.SetBool("walkDown", true);
            animator.SetBool("walkUp", false);
        }
        else if (transform.position.x == positions[posNumber].x && transform.position.y > positions[posNumber].y)
        {
            animator.SetBool("walkDown", true);
        }

        else if (transform.position.x == positions[posNumber].x && transform.position.y < positions[posNumber].y)
        {
            animator.SetBool("walkUp", true);
        }

        else if (transform.position.x > positions[posNumber].x && transform.position.y == positions[posNumber].y)
        {
            animator.SetBool("walkLeft", true);
        }

        else if (transform.position.x < positions[posNumber].x && transform.position.y == positions[posNumber].y)
        {
            animator.SetBool("walkRight", true);
        }
        
        
    }
}
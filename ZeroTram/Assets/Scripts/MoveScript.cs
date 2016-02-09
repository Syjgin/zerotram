using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class MoveScript : MonoBehaviour
{
    public float speed;//Переменная, отвечающая за скорость движения
    public Vector2 direction = new Vector2(0, 0);//Переменная для направления движения
    private Vector2 movement;//Переменная для передачи движения на Rigidbody
    private Vector3 scale; //Переменная для текущего размера
    public Vector3 newscale;//Переменная для нового размера
    public float Coef;//Переменная , отвечающая за коэффициент
    private float Dist;//Переменная, отвечающая за дистанцию от центра

    void Start()
    {
        scale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z); // Проверяем текущий размер
        newscale = scale;
    }

    void Update()
    {
        if (transform.position.x > 0)
        {
            direction = new Vector2(0.5f, -1);
        }
        else if (transform.position.x < 0)
        {
            direction = new Vector2(-0.5f, -1);
        }
        else
        {
            direction = new Vector2(0, -1);
        }
        Dist = Mathf.Abs(transform.position.x);
        if (Mathf.Abs(transform.position.x) < 1)
        {
            Dist = 1;
        }
        movement = new Vector2((speed * direction.x) / (Dist * Coef) ,(speed * direction.y) / (Dist * Coef));//Присваиваем ей значение
        this.transform.localScale = Vector3.Lerp(this.transform.localScale, newscale, Time.deltaTime);//Присваиваем новый размер объекту
        newscale = new Vector3(1 + Time.deltaTime, 1 + Time.deltaTime, 1 + Time.deltaTime);//Задаем изменение размера
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = movement;//Передаем на Rigidbody движение
        if (GetComponent<Renderer>().isVisible == false)//Проверяем, виден ли объект в камере
        {
            Destroy(gameObject);
        }
    }
}
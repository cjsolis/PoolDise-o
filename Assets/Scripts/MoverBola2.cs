using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverBola2 : MonoBehaviour {

    Vector3 posAnterior;              //En esta variable se guarda la posición inicial de la bola blanca para saber si se esta moviendo o no.
    float posYPaloInicial = 26.1f;    //Esta es la distancia por defecto que tiene el taco con la bola blanca.
    float posYPaloFinal = 30.1f;      //Esta es la distancia máxima que tiene el taco con la bola blanca.
    private Rigidbody rb;             //Rigidbody de la bola.   
    int speed = 6000;                 //Velocidad con la que se golpea la bola.       
    GameObject palo;                  //GameObject que referencia el taco.
    bool banderaClick = false;        //Bandera para saber si el click derecho fue apretado.


    //Funcion que inicializa el juego
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        palo = GameObject.Find("Taco");
        posAnterior = transform.position;
    }

    //Funcion loop que se repite por cada frame del juego.
    private void FixedUpdate()
    {
        //Esta vara agarra la posición en la que esta la cámara
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        //Esta vara agarra la posición del mouse en esa cámara.
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        //Llama a rotar para detectar si el palo da vueltas
        Rotar(positionOnScreen, mouseOnScreen);

        //Valida que se apreto el clic para levantar una bandera.
        if (Input.GetMouseButtonDown(0))
        {
            banderaClick = true;
        }

        //Llama a moverse si detecta que dieron clic 
        if (!ValidarMovimiento())
        {
            palo.SetActive(true);               //Vuelve el palo visible para el jugador

            if (banderaClick)                   //Verifica que se apreto el click con una bandera
            {
                if (Input.GetMouseButton(0))    //Verifica que ese click se mantenga apretado
                {

                    Vector3 temporal = palo.transform.localPosition;    //Variable temporal la cual captura la posición del palo para antes/después de modificarlo
                    if (temporal.y <= posYPaloFinal)
                    {
                        temporal.y += 0.04f;            //Aumenta la distancia entre el palo y la bola por 0.04 (en total la distancia es de 4)
                        palo.transform.localPosition = temporal;    //Le asigna a la posicion del palo una nueva con la y anterior modificada.

                        //Inicio - Bloque que cambia las velocidades que va a tener la bola dependiendo de la fuerza.
                        if (temporal.y >= 26.1f && temporal.y <= 27.1f)
                        {
                            speed = 6000;
                        }

                        else if (temporal.y > 27.1f && temporal.y <= 28.1f)
                        {
                            speed = 13000;
                        }

                        else if(temporal.y > 28.1f && temporal.y <= 29.1f)
                        {
                            speed = 20000;
                        }

                        else if (temporal.y > 29.1f && temporal.y <= 30.1f)
                        {
                            speed = 27000;
                        }
                        //Fin - Bloque que cambia las velocidades que va a tener la bola dependiendo de la fuerza.
                    }

                }

                else
                {
                    Vector3 temporal = palo.transform.localPosition;        //Variable temporal la cual captura la posición del palo para antes/después de modificarlo
                    if (temporal.y > posYPaloInicial)
                    {
                        temporal.y -= 0.8f;             //Aumenta la distancia entre el palo y la bola por 0.8 (para que parezca que se devuelve rápido)
                        palo.transform.localPosition = temporal;        //Le asigna a la posicion del palo una nueva con la y anterior modificada.
                    }

                    if (temporal.y <= posYPaloInicial)      //Una vez el palo pase la posición estandar entonces se llama la funcion de mover la bola.
                    {
                        Moverse(positionOnScreen, mouseOnScreen);
                    }

                }
            }     
        }
    }

    //Funcion que se encarga de validar la posición de la bola blanca para ver si se sigue moviendo.
    bool ValidarMovimiento()
    {
        Vector3 desplazamiento = transform.position - posAnterior;
        posAnterior = transform.position;

        return desplazamiento.magnitude > 0.001;
    }

    //Función que se encarga de mover la bola blanca.
    void Moverse(Vector3 a, Vector3 b)
    {
        /*Como la pantalla y el mouse son dos vectores, entonces solo van a tener X y Y,
        entonces agarramos la diferencia en X y Y de ambos y sacamos la dirección en la
        que debería ir la bola cuando la pega el palo.*/
        float moveHorizontal = a.x - b.x;
        float moveVertical = a.y - b.y;

        //Crea un vector de 3 parámetros para mover la bola, nada más que la bola solo se mueve en X y Z, no en Y, por lo que hay un 0 en Y.
        Vector3 movimiento2 = new Vector3(moveHorizontal * speed, 0, moveVertical * speed);

        //Se encarga de reiniciar la posición estandar del palo y de esconderlo mientras la bola se mueve.
        Vector3 temporal = palo.transform.localPosition;
        temporal.y = posYPaloInicial;
        palo.transform.localPosition = temporal;
        palo.SetActive(false);

        //Reinicia la bandera de que se apreto el click.
        banderaClick = false;

        //Le aplica fuerza a la bola para que se mueva
        rb.AddForce(movimiento2);
        
    }

    //Funicion que se encarga de hacer girar el palo al rededor de la bola blanca.
    void Rotar(Vector3 a, Vector3 b)
    {
        //Esta vara agarra el ángulo en el que está el mouse para hacer que el palo gire.
        float angle = AngleBetweenTwoPoints(a, b);

        //Esta es la que se encarga de girar la bola con el palo
        transform.rotation = Quaternion.Euler(new Vector3(0f, -1 * angle, 92f));
    }

    //Esta vara es la que se encarga de transformar la posición X y Y de la cámara y del mouse en la cámara y la convierte en un ángulo.
    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

}

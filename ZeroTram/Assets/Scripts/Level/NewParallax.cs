using UnityEngine;
using Assets;
using System.Collections.Generic;

public class NewParallax : MonoBehaviour
{
    public float XIncrement = 0.01f;
    public float YIncrement = 0.1f;
    public float MaxVelocity = 5f;
    private const float BaseScale = 0.4f;
    private const float MinScaleCoef = 0.1f;
    private const float ScaleIncrement = 0.5f;

    private float _velocity;
    private float _horizontalVelocity;

    void Start()
    {
        _velocity = MaxVelocity / transform.position.x;
        if (_velocity > 0)
            _velocity *= -1;
        _horizontalVelocity = XIncrement;
        if (transform.position.x < 0)
            _horizontalVelocity *= -1;
        UpdateScale();
    }

    void Update()
    {
        if (!GameController.GetInstance().IsDoorsOpen())
        {
            float newY = transform.position.y + Time.deltaTime * _velocity;
            float newX = transform.position.x + _horizontalVelocity;
            transform.position = new Vector3(newX, newY, transform.position.z);
            UpdateScale();            
        }
    }

    private void UpdateScale()
    {
        float screenY = Camera.main.WorldToViewportPoint(transform.position).y + ScaleIncrement;
        if (screenY < MinScaleCoef)
        {
            Destroy(gameObject);
            return;
        }
        screenY += ScaleIncrement;
        float scaleCoef = BaseScale / screenY;
        transform.localScale = new Vector3(scaleCoef, scaleCoef, scaleCoef);
    }

}

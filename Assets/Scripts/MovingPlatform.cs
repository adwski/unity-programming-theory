using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    const float maxSpeed = 10f;
    const float maxStopTime = 10f;

    Rigidbody _rbody;

    // ENCAPSULATION
    public float speed {
        get { return _speed; }
        set { validateAndSetSpeed(speed); }
    }

    // ENCAPSULATION
    public float stopTime
    {
        get { return _stopTime; }
        set { validateAndSetStopTime(stopTime); }
    }

    public bool active { get { return _active; } }

    float _speed = 1.0f;
    float _stopTime = 2.0f;

    bool _active = false;
    bool forward = false;
    bool stop = false;
    bool gracefulStop = false;


    Vector3 direction;
    Vector3 target;
    Vector3 destination;
    Vector3 start;

    // ABSTRACTION
    protected void StartMove(Vector3 point, float mSpeed, float sTime)
    {
        validateAndSetSpeed(mSpeed);
        validateAndSetStopTime(sTime);
        StartWithStop(point);
    }

    // POLYMORPHISM
    // ABSTRACTION
    protected void StartMoveNow(Vector3 point, float mSpeed, float sTime)
    {
        validateAndSetSpeed(mSpeed);
        validateAndSetStopTime(sTime);
        StartNow(point);
    }

    // POLYMORPHISM
    // ABSTRACTION
    protected void StartMoveNow(Vector3 point)
    {
        StartNow(point);
    }

    // ABSTRACTION
    protected void StopMove()
    {
        gracefulStop = true;
    }

    private void validateAndSetStopTime(float s)
    {
        if (s > maxStopTime)
        {
            _stopTime = maxStopTime;
        }
        else if (s < 0)
        {
            _stopTime = 0;
        }
        else
        {
            _stopTime = s;
        }

        Debug.Log("Platform stopTime: " + _stopTime);
    }

    private void validateAndSetSpeed(float s)
    {
        if (s > maxSpeed)
        {
            _speed = maxSpeed;
        }
        else if (s < 0)
        {
            _speed = 0;
        }
        else
        {
            _speed = s;
        }
        Debug.Log("Platform speed: " + _speed);
    }

    private void StartNow(Vector3 point)
    {
        Init();
        Debug.Log("Platform started: " + point);
        destination = point;
        start = _rbody.position;
        _active = true;
        forward = true;
    }

    private void StartWithStop(Vector3 point)
    {
        Init();
        Debug.Log("Platform started with stop: " + point);
        destination = point;
        start = _rbody.position;
        _active = true;
        forward = false;
        Stop();
    }

    void Init()
    {
        _rbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (_active && !stop)
        {
            MoveTowards();
        }
    }

    void Update()
    {
        if (_active && !stop)
        {
            CheckTarget();
        }
    }

    void CheckTarget()
    {
        if (Vector3.Dot( (target - _rbody.position).normalized, direction ) < 0f)
        {
            Stop();
        }
    }

    void MoveTowards()
    {
        _rbody.MovePosition(_rbody.position + direction * _speed * Time.deltaTime);
    }

    void Stop()
    {
        Debug.Log("Platform stopped");
        stop = true;
        if (GracefulStopCheck())
        {
            Debug.Log("Platform will be resumed");
            Invoke(nameof(ResumeMove), _stopTime);
        }
    }

    void ResumeMove()
    {
        AssignDirection();
        stop = false;
    }

    void AssignDirection()
    {
        if (!forward)
        {
            direction = (destination - start).normalized;
            target = destination;
            forward = true;
        }
        else
        {
            direction = (start - destination).normalized;
            target = start;
            forward = false;
        }
        Debug.Log("Platform is moving to: " + target);
    }

    bool GracefulStopCheck()
    {
        if (gracefulStop)
        {
            _active = false;
            Debug.Log("Platform stopped gracefuly");
        }
        return _active;
    }
}

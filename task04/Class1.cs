namespace task04;
public interface ISpaceship
{
    void MoveForward();
    void Rotate(int angle);
    void Fire();
    int Speed { get; }
    int FirePower { get; }
}

public class Fighter : ISpaceship
{
    private int _position = 0;
    private int _angle = 0;
    private int _rocketCount = 10;

    public int Speed => 100;
    public int FirePower => 50;

    public void MoveForward()
    {
        _position += Speed;
    }

    public void Rotate(int angle)
    {
        _angle = (_angle + angle) % 360;
    }

    public void Fire()
    {
        if (_rocketCount > 0)
        {
            _rocketCount--;
        }
    }

    public int GetPosition() => _position;
    public int GetAngle() => _angle;
    public int GetRocketCount() => _rocketCount;
}

public class Cruiser : ISpaceship
{
    private int _position = 0;
    private int _angle = 0;
    private int _rocketCount = 5;

    public int Speed => 50;
    public int FirePower => 100;

    public void MoveForward()
    {
        _position += Speed;
    }

    public void Rotate(int angle)
    {
        _angle = (_angle + angle) % 360;
    }

    public void Fire()
    {
        if (_rocketCount > 0)
        {
            _rocketCount--;
        }
    }

    public int GetPosition() => _position;
    public int GetAngle() => _angle;
    public int GetRocketCount() => _rocketCount;
}
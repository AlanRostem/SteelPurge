using Godot;

public class DoubleTapDetector
{
    private const float PressIntervalTime = 0.2f;
    private bool _pressedOnce = false;
    private float _currentPressTime = 0f;

    public bool IsInputActionDoubleTapped(string action, float delta)
    {
        if (!_pressedOnce)
        {
            if (Input.IsActionJustPressed(action))
            {
                _pressedOnce = true;
            }
            else
            {
                _pressedOnce = false;
                _currentPressTime = 0;
            }

            return false;
        }

        _currentPressTime += delta;
        if (_currentPressTime <= PressIntervalTime)
        {
            if (Input.IsActionJustPressed(action))
            {
                _pressedOnce = false;
                _currentPressTime = 0;
                return true;

            }
        }
        else
        {
            _pressedOnce = false;
            _currentPressTime = 0;
        }

        return false;
    }
}
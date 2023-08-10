using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static InputManager Instance { get => instance; }
    //[SerializeField ] protected Ray ray;
    public Ray _Ray;

    private void Awake()
    {
        InputManager.instance = this;
    }
    private void FixedUpdate()
    {
        this.GetMousePos();
    }

    protected virtual void GetMousePos()
    {
        this._Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    }
}

using Zenject;
using UnityEngine;
using System;
public class MotionControl : MonoBehaviour
{
    [SerializeField] private int speed = 5;
    [SerializeField] private float rotateSpeedX = 100f; // Скорость вращения игрока
    [SerializeField] private float rotateSpeedY = 100f; // Скорость вращения камеры по вертикали
    [SerializeField] private float rotateSpeedXMobile = 10f; // Скорость вращения игрока на мобильных устройствах
    [SerializeField] private float rotateSpeedYMobile = 10f; // Скорость вращения камеры по вертикали на мобильных устройствах
    [SerializeField] private float minVerticalAngle = -40f; // Минимальный угол наклона камеры вверх
    [SerializeField] private float maxVerticalAngle = 40f; // Максимальный угол наклона камеры вниз

    private float currentVerticalAngle = 0f; // Текущий угол наклона камеры по вертикали
    private Vector3 _previousMousePosition; // Предыдущее положение мыши

    [Inject] private BagPresenter bagPres;

    internal CharacterController charController; // Контроллер управления персонажем
    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }
    private void  Update()
    {
        TouchItems();
        MovePlayer();
        RotatePlayer();
    }

    private void MovePlayer()
    {
        Vector3 direction = transform.TransformDirection(Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal"));
        charController.Move(direction * speed * Time.deltaTime);
    }

    private void TouchItems()
    {
        TouchBag();
    }

    private void TouchBag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out RaycastHit hitInfo, 10f))
            {

                if (hitInfo.collider.GetComponent<BagPresenter>())
                {

                    bagPres.OpenBag();
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && bagPres.UiIsOpen)
        {
            bagPres.RemoveItem();
            bagPres.CloseBag();
        }
    }


    private void RotatePlayer()
    {
        if (ShouldRotatePlayer())
        {
            Vector2 rotationInput = GetRotationInput();
            RotateHorizontally(rotationInput.x);
            RotateVertically(rotationInput.y);
        }
    }

    private bool ShouldRotatePlayer()
    {

        short countTouches = Convert.ToInt16(Input.GetMouseButton(0) || Input.GetMouseButton(1));
#if UNITY_ANDROID
        countTouches = (short) Input.touchCount;
#endif

        return (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() && countTouches>0);
    }

    private Vector2 GetRotationInput()
    {

        return GetMouseRotationInput();

    }

    private Vector2 GetMouseRotationInput()
    {
        if (Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0))
        {
            Vector3 currentMousePosition = Input.mousePosition;
            Vector3 mouseDelta = currentMousePosition - _previousMousePosition;
            _previousMousePosition = currentMousePosition;

            // Плавное изменение значения с использованием Lerp
            float smoothHorizontal = Mathf.Lerp(0, mouseDelta.x * rotateSpeedX * Time.deltaTime, 0.1f);
            float smoothVertical = Mathf.Lerp(0, mouseDelta.y * rotateSpeedY * Time.deltaTime, 0.1f);

            return new Vector2(smoothHorizontal, smoothVertical);
        }
        _previousMousePosition = Input.mousePosition;
        return Vector2.zero;
    }

    private Vector2 GetTouchRotationInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                return new Vector2(
                    touch.deltaPosition.x * rotateSpeedXMobile * Time.deltaTime,
                    touch.deltaPosition.y * rotateSpeedYMobile * Time.deltaTime
                );
            }
        }
        return Vector2.zero;
    }

    private void RotateHorizontally(float horizontalInput)
    {
        transform.Rotate(Vector3.up * horizontalInput);
    }

    private void RotateVertically(float verticalInput)
    {
        currentVerticalAngle -= verticalInput; // Инвертируем для естественного поворота
        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, minVerticalAngle, maxVerticalAngle);
        Camera.main.transform.localRotation = Quaternion.Euler(currentVerticalAngle, 0, 0);
    }
}
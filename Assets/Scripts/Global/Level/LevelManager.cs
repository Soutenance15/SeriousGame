using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    PlayerInput playerInput;

    InputAction togglePlayAction;

    // Action
    public static Action<bool> OnPlayPauseTime;

    [SerializeField]
    Button menuButton;

    bool onPause;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        togglePlayAction = playerInput.actions["TogglePlay"];
        menuButton.onClick.AddListener(() => PlayPauseTime());
    }

    void OnEnable()
    {
        togglePlayAction.performed += OnTogglePlay;
    }

    void OnDisable()
    {
        togglePlayAction.performed -= OnTogglePlay;
    }

    // Actions

    void OnTogglePlay(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        PlayPauseTime();
    }

    public void PlayPauseTime()
    {
        Debug.Log("Toogle Play");
        onPause = !onPause;
        if (onPause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
        OnPlayPauseTime?.Invoke(onPause);
    }
}

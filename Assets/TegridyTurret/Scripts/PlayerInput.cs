// GENERATED AUTOMATICALLY FROM 'Assets/TegridyTurret/Scripts/PlayerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""Turret"",
            ""id"": ""dd4c0532-315b-46df-9a45-62c6481f9289"",
            ""actions"": [
                {
                    ""name"": ""RotateTurret"",
                    ""type"": ""Value"",
                    ""id"": ""7df78ae6-b318-4d19-8196-d1faa55996dd"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""18208d03-80c3-4798-aa2a-5716a7adf6b9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ChangeAmmo"",
                    ""type"": ""Button"",
                    ""id"": ""87d985f2-4513-446d-b501-78a9c25be61b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""74184478-8d10-40d8-a10e-9de3811041a3"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateTurret"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""31b4d278-e84c-4cce-b4cf-82a7dda11de0"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateTurret"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""33ec7421-01ce-4dfa-a2e0-531ae710c0ad"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateTurret"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""21c63303-ba0e-40e2-860e-b3e46eb5da84"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateTurret"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8fc03836-2497-441f-b61e-a759e012a4fa"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RotateTurret"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9ac7f12e-703e-4aee-b6c5-10dcc5edf667"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c9fd5791-5223-4142-badd-673541b608d7"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeAmmo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Turret
        m_Turret = asset.FindActionMap("Turret", throwIfNotFound: true);
        m_Turret_RotateTurret = m_Turret.FindAction("RotateTurret", throwIfNotFound: true);
        m_Turret_Fire = m_Turret.FindAction("Fire", throwIfNotFound: true);
        m_Turret_ChangeAmmo = m_Turret.FindAction("ChangeAmmo", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Turret
    private readonly InputActionMap m_Turret;
    private ITurretActions m_TurretActionsCallbackInterface;
    private readonly InputAction m_Turret_RotateTurret;
    private readonly InputAction m_Turret_Fire;
    private readonly InputAction m_Turret_ChangeAmmo;
    public struct TurretActions
    {
        private @PlayerInput m_Wrapper;
        public TurretActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @RotateTurret => m_Wrapper.m_Turret_RotateTurret;
        public InputAction @Fire => m_Wrapper.m_Turret_Fire;
        public InputAction @ChangeAmmo => m_Wrapper.m_Turret_ChangeAmmo;
        public InputActionMap Get() { return m_Wrapper.m_Turret; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TurretActions set) { return set.Get(); }
        public void SetCallbacks(ITurretActions instance)
        {
            if (m_Wrapper.m_TurretActionsCallbackInterface != null)
            {
                @RotateTurret.started -= m_Wrapper.m_TurretActionsCallbackInterface.OnRotateTurret;
                @RotateTurret.performed -= m_Wrapper.m_TurretActionsCallbackInterface.OnRotateTurret;
                @RotateTurret.canceled -= m_Wrapper.m_TurretActionsCallbackInterface.OnRotateTurret;
                @Fire.started -= m_Wrapper.m_TurretActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_TurretActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_TurretActionsCallbackInterface.OnFire;
                @ChangeAmmo.started -= m_Wrapper.m_TurretActionsCallbackInterface.OnChangeAmmo;
                @ChangeAmmo.performed -= m_Wrapper.m_TurretActionsCallbackInterface.OnChangeAmmo;
                @ChangeAmmo.canceled -= m_Wrapper.m_TurretActionsCallbackInterface.OnChangeAmmo;
            }
            m_Wrapper.m_TurretActionsCallbackInterface = instance;
            if (instance != null)
            {
                @RotateTurret.started += instance.OnRotateTurret;
                @RotateTurret.performed += instance.OnRotateTurret;
                @RotateTurret.canceled += instance.OnRotateTurret;
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
                @ChangeAmmo.started += instance.OnChangeAmmo;
                @ChangeAmmo.performed += instance.OnChangeAmmo;
                @ChangeAmmo.canceled += instance.OnChangeAmmo;
            }
        }
    }
    public TurretActions @Turret => new TurretActions(this);
    public interface ITurretActions
    {
        void OnRotateTurret(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnChangeAmmo(InputAction.CallbackContext context);
    }
}

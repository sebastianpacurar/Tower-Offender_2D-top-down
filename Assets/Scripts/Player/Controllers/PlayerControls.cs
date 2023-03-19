//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Scripts/Player/Controllers/PlayerControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""8813999d-43de-45bb-a2b9-b8f3b0cdc032"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""5a6db4b6-fd6a-49ce-934f-029d181a0472"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Steer"",
                    ""type"": ""Value"",
                    ""id"": ""67aba640-3253-4b12-9f0c-661946f81d77"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""56274ea8-4b15-4ef3-af5f-500c1114e610"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SpeedBoost"",
                    ""type"": ""Button"",
                    ""id"": ""1c541686-3a1c-45bd-98c4-b5679a20d5ba"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CameraZoom"",
                    ""type"": ""Value"",
                    ""id"": ""d4c8a5ea-0fef-4397-9f74-775eca8e0467"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Accelerate"",
                    ""id"": ""ee5d4527-3ebd-41a0-961c-0e9439a4f325"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""9306118e-5b7c-4fdb-9704-e6d1cd8abf17"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""056ba89f-91d6-403a-aea8-7e22cc9669aa"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Rotate"",
                    ""id"": ""0ae894b5-a2bd-4290-ae98-0b50fab84668"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steer"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""78a4fcf0-7553-4e0e-a234-86769fc6bb3a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Steer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""c64339be-c1ad-47c6-8ae6-5729e85b2a57"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Steer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1ae63ee7-7d1e-4b1d-b796-bed8ea19aeb0"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ec423ba2-0c68-481b-a006-5d0914345671"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""SpeedBoost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""2e4e69aa-bb39-414b-bb42-42317724e54a"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraZoom"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""57b1195b-2c87-49b1-9646-d050d9dab470"",
                    ""path"": ""<Mouse>/scroll/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""CameraZoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""7916f62c-4286-4dad-b200-7dc7edfbcd66"",
                    ""path"": ""<Mouse>/scroll/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""CameraZoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""5c39deb7-be7a-4b39-8100-bdd88c0c27b7"",
            ""actions"": [
                {
                    ""name"": ""FirstWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""566ec825-472b-4763-afe6-4d7dba054528"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SecondWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""f4692fe5-df31-442a-a081-630eb596811a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ThirdWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""1dc989ce-4ef4-4d33-b6ff-2d7126061968"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""FourthWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""61e2f97c-5063-43e4-98c9-a52d0b7fc048"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""1f560eb6-0bdc-4287-aa17-8538e4704970"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1cee0238-66f1-43d7-b7f8-652d0bfc69f4"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""FirstWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bf424976-132c-4116-8dc8-ef026308ef3b"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""SecondWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""367eaf2b-4dab-4881-aa9d-3a839ac01902"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ThirdWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""362c993d-66df-465f-84d2-b7dee159c8ff"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""FourthWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9ffec99a-6bb8-463c-b7e6-3196ec0cd091"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Steer = m_Player.FindAction("Steer", throwIfNotFound: true);
        m_Player_Shoot = m_Player.FindAction("Shoot", throwIfNotFound: true);
        m_Player_SpeedBoost = m_Player.FindAction("SpeedBoost", throwIfNotFound: true);
        m_Player_CameraZoom = m_Player.FindAction("CameraZoom", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_FirstWeapon = m_UI.FindAction("FirstWeapon", throwIfNotFound: true);
        m_UI_SecondWeapon = m_UI.FindAction("SecondWeapon", throwIfNotFound: true);
        m_UI_ThirdWeapon = m_UI.FindAction("ThirdWeapon", throwIfNotFound: true);
        m_UI_FourthWeapon = m_UI.FindAction("FourthWeapon", throwIfNotFound: true);
        m_UI_Pause = m_UI.FindAction("Pause", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Steer;
    private readonly InputAction m_Player_Shoot;
    private readonly InputAction m_Player_SpeedBoost;
    private readonly InputAction m_Player_CameraZoom;
    public struct PlayerActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Steer => m_Wrapper.m_Player_Steer;
        public InputAction @Shoot => m_Wrapper.m_Player_Shoot;
        public InputAction @SpeedBoost => m_Wrapper.m_Player_SpeedBoost;
        public InputAction @CameraZoom => m_Wrapper.m_Player_CameraZoom;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Steer.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSteer;
                @Steer.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSteer;
                @Steer.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSteer;
                @Shoot.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnShoot;
                @SpeedBoost.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpeedBoost;
                @SpeedBoost.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpeedBoost;
                @SpeedBoost.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSpeedBoost;
                @CameraZoom.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraZoom;
                @CameraZoom.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraZoom;
                @CameraZoom.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCameraZoom;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Steer.started += instance.OnSteer;
                @Steer.performed += instance.OnSteer;
                @Steer.canceled += instance.OnSteer;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @SpeedBoost.started += instance.OnSpeedBoost;
                @SpeedBoost.performed += instance.OnSpeedBoost;
                @SpeedBoost.canceled += instance.OnSpeedBoost;
                @CameraZoom.started += instance.OnCameraZoom;
                @CameraZoom.performed += instance.OnCameraZoom;
                @CameraZoom.canceled += instance.OnCameraZoom;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_FirstWeapon;
    private readonly InputAction m_UI_SecondWeapon;
    private readonly InputAction m_UI_ThirdWeapon;
    private readonly InputAction m_UI_FourthWeapon;
    private readonly InputAction m_UI_Pause;
    public struct UIActions
    {
        private @PlayerControls m_Wrapper;
        public UIActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @FirstWeapon => m_Wrapper.m_UI_FirstWeapon;
        public InputAction @SecondWeapon => m_Wrapper.m_UI_SecondWeapon;
        public InputAction @ThirdWeapon => m_Wrapper.m_UI_ThirdWeapon;
        public InputAction @FourthWeapon => m_Wrapper.m_UI_FourthWeapon;
        public InputAction @Pause => m_Wrapper.m_UI_Pause;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @FirstWeapon.started -= m_Wrapper.m_UIActionsCallbackInterface.OnFirstWeapon;
                @FirstWeapon.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnFirstWeapon;
                @FirstWeapon.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnFirstWeapon;
                @SecondWeapon.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSecondWeapon;
                @SecondWeapon.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSecondWeapon;
                @SecondWeapon.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSecondWeapon;
                @ThirdWeapon.started -= m_Wrapper.m_UIActionsCallbackInterface.OnThirdWeapon;
                @ThirdWeapon.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnThirdWeapon;
                @ThirdWeapon.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnThirdWeapon;
                @FourthWeapon.started -= m_Wrapper.m_UIActionsCallbackInterface.OnFourthWeapon;
                @FourthWeapon.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnFourthWeapon;
                @FourthWeapon.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnFourthWeapon;
                @Pause.started -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @FirstWeapon.started += instance.OnFirstWeapon;
                @FirstWeapon.performed += instance.OnFirstWeapon;
                @FirstWeapon.canceled += instance.OnFirstWeapon;
                @SecondWeapon.started += instance.OnSecondWeapon;
                @SecondWeapon.performed += instance.OnSecondWeapon;
                @SecondWeapon.canceled += instance.OnSecondWeapon;
                @ThirdWeapon.started += instance.OnThirdWeapon;
                @ThirdWeapon.performed += instance.OnThirdWeapon;
                @ThirdWeapon.canceled += instance.OnThirdWeapon;
                @FourthWeapon.started += instance.OnFourthWeapon;
                @FourthWeapon.performed += instance.OnFourthWeapon;
                @FourthWeapon.canceled += instance.OnFourthWeapon;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnSteer(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnSpeedBoost(InputAction.CallbackContext context);
        void OnCameraZoom(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnFirstWeapon(InputAction.CallbackContext context);
        void OnSecondWeapon(InputAction.CallbackContext context);
        void OnThirdWeapon(InputAction.CallbackContext context);
        void OnFourthWeapon(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
}

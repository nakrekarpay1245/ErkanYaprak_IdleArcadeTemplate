using UnityEditor;
using UnityEngine;

public class ProjectOverviewWindow : EditorWindow
{
    private Vector2 scrollPosition;
    private Texture2D logoTexture;

    [MenuItem("Window/Idle Arcade Template Overview")]
    public static void ShowWindow()
    {
        var window = GetWindow<ProjectOverviewWindow>("Idle Arcade Template");
        window.minSize = new Vector2(600, 500);
    }

    [InitializeOnLoadMethod]
    private static void InitOnProjectLoad()
    {
        // Automatically show the window when the project is first opened
        EditorApplication.update += OpenOnceOnLoad;
    }

    private static void OpenOnceOnLoad()
    {
        if (!SessionState.GetBool("ProjectOverviewShown", false))
        {
            ShowWindow();
            SessionState.SetBool("ProjectOverviewShown", true);
        }
        EditorApplication.update -= OpenOnceOnLoad;
    }

    private void OnEnable()
    {
        // Load the logo texture for display
        logoTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ErkanYaprak_IdleArcadeTemplate/_Game/Publishing/Logo.png");
    }

    private void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        GUILayout.Space(10);

        // Display the logo
        if (logoTexture != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(logoTexture, GUILayout.Width(128), GUILayout.Height(128));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(10);

        // Project Overview Section
        EditorGUILayout.LabelField("Idle Arcade Template", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("A mobile-focused game development template that helps quickly prototype and build top-down arcade games. It features extensible components adhering to SOLID principles, editor tools for fast iteration, and highly optimized workflows.", EditorStyles.wordWrappedLabel);

        GUILayout.Space(15);

        // TopDownCharacter Overview Section
        EditorGUILayout.LabelField("TopDownCharacter Presentation", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Built with modular and extendable components to manage movement, interaction, item collection, attacks, and more. Utilizes ScriptableObjects for easier configuration.", EditorStyles.wordWrappedLabel);

        // Tutorial and Demo Links
        GUILayout.Space(15);
        if (GUILayout.Button("Watch YouTube Tutorial"))
        {
            Application.OpenURL("https://www.youtube.com/watch?v=AbNMG2LzjAY");
        }
        if (GUILayout.Button("Play WebGL Demo"))
        {
            Application.OpenURL("https://erkanyaprak.itch.io/idlearcade");
        }

        // Installation & Setup Section
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Installation & Setup", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Clone the repository or include as a UnityPackage. Open the project in Unity, play the sample scene, and start building your game.", EditorStyles.wordWrappedLabel);

        // Key Features Section
        GUILayout.Space(15);
        EditorGUILayout.LabelField("Key Features", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("- TopDownCharacterController: Movement handled via keyboard/joystick using Unity's CharacterController.\n" +
                                   "- TopDownCharacterCollector: Collect items like coins and power-ups.\n" +
                                   "- TopDownCharacterInteractor: Interact with doors, chests, etc.\n" +
                                   "- TopDownCharacterDamageHandler: Manages health and damage logic.\n" +
                                   "- TopDownCharacterAttackHandler: Attack enemies with various weapons.", EditorStyles.wordWrappedLabel);

        // Example Character Creation Section
        GUILayout.Space(15);
        EditorGUILayout.LabelField("Character Creation", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Use custom editor tools to create fully configured characters with attributes such as speed, health, and damage. The editor automatically sets up components like movement, interaction, and animation systems.", EditorStyles.wordWrappedLabel);

        // Future Roadmap Section
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Future Roadmap", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Planned features include ranged weapons, jumping mechanics, enemy AI, and harvestable resources.", EditorStyles.wordWrappedLabel);

        // Contact Information
        GUILayout.Space(20);
        EditorGUILayout.LabelField("Contact Information", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Developer: Erkan Yaprak\nGitHub: nakrekarpay1245\nEmail: rknyprk79@gmail.com", EditorStyles.wordWrappedLabel);

        EditorGUILayout.EndScrollView();

        // Debugging aid for easier code changes
        if (GUILayout.Button("Close Window"))
        {
            Close();
        }
    }
}
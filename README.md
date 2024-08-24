# Idle Arcade Template

## 1. Project Overview
**Idle Arcade Template** is a mobile-focused game development template that allows you to quickly prototype and create top-down arcade games. Featuring a comprehensive set of components, powerful editor tools, and an extensible architecture adhering to SOLID principles, this template offers both developers and game designers an optimized workflow for creating dynamic and engaging gameplay.

### How to Use?
- **YouTube Video:** [Click Here to Play the How To Use Video](https://youtu.be/nDBlYK3O1Fs?list=RDnDBlYK3O1Fs](https://www.youtube.com/@erkanyaprak2390)

- **Live Demo:** [Click Here to Play the WebGL Demo](https://erkanyaprak.itch.io/idlearcade)
- **Download Options:** Clone directly from the repository, or use as a UnityPackage to include in your project.

## 2. Key Features
- **TopDownCharacterController:** Handle character movement with keyboard and joystick input using Unity's built-in `CharacterController`. PlayerInputSO reduces dependency and takes all data and applies it. The values ​​in PlayerInputSO are changed by InputHandler.
- **TopDownCharacterCollector:** Collect common items (e.g., coins) and rare items (e.g., scrolls or power-ups).
- **TopDownCharacterInteractor:** Interact with in-game objects like doors, chests, and more.
- **TopDownCharacterDamageHandler:** Manage character health, damage, and death systems.
- **TopDownCharacterAttackHandler:** Enable characters to attack enemies and interact with weapons.
- **TopDownCharacterAnimator:** Support for various animations, such as Idle, Walk, Run, Attack, and Death.
- **Extensible Interfaces & Abstract Classes:** Leverage `IDamager`, `IDamageable`, `AbstractDamageableBase`, `AbstractDamagerBase`, `ICollector`, `ICollectible`, `IInteractor`, `IInteractable` to add flexible functionality to any game object.
- **Custom Editor Tools:** Easy-to-use editor windows for character creation and configuration via ScriptableObjects.
- **Advanced Camera Tracking Code:** Advanced camera system that follows the character smoothly and zooms in and out according to the character's movement.

## 3. Installation & Setup

### Requirements
- **Unity Version:** 2020 LTS or later
- **Dependencies:** There is no dependency

### Installation Steps
1. **Clone the Repository:**
    ```bash
    git clone https://github.com/nakrekarpay1245/ErkanYaprak_IdleArcadeTemplate.git
    ```
2. **Open in Unity:** Open the project using the Unity Hub, ensuring you have the correct version of Unity installed.
3. **Play the Example Scene:** Load the provided sample scene to test the core functionality out-of-the-box.

### WebGL Demo
You can experience the template firsthand by playing the WebGL demo. Click the link below:

[Play WebGL Demo](#)

## 4. Usage

### Creating a New Character
Using the custom editor tools, you can create fully-configured characters in minutes:

- **TopDownCharacterConfigSOEditor:** Create a new `TopDownCharacterConfigSO` to configure character parameters such as speed, health, and damage. Unused parameters are automatically hidden to reduce clutter for game designers.
- **TopDownCharacterCreator:** Create new characters using any model—be it Humanoid, Generic, or Custom Rigged—by simply selecting a model and configuring its attributes through the editor window.

### Example: Character Creation
Here's how to create a new character with the built-in editor tools:

1. Open the `TopDownCharacterCreator` window from the Unity menu.
2. Select the character model, name the character, and assign the `TopDownCharacterConfigSO`.
3. The editor will automatically generate the character with the appropriate components, such as movement, attack, and animation systems, based on the provided configuration.

## 5. Architecture & Technologies

### Core Components Overview
- **TopDownCharacterController**  
  Handles movement using Unity’s `CharacterController` component. Supports both keyboard and joystick input, making it ideal for cross-platform development.

- **TopDownCharacterCollector**  
  Manages the collection of items. Implements the `ICollector` interface to interact with items tagged as `ICollectible`.

- **TopDownCharacterAttackHandler**  
  Enables attack functionality. Inherits from `AbstractDamagerBase`, which implements the `IDamager` interface for compatibility with various attack systems. The attack handler triggers damage effects on objects implementing the `IDamageable` interface.

- **TopDownCharacterDamageHandler**  
  Manages character health and death logic. Implements `IDamageable` and derives from `AbstractDamageableBase`, facilitating interaction with `IDamager` entities.

- **TopDownCharacterAnimator**  
  Manages animations for Idle, Walking, Running, Attacking, Dying, and more. Works seamlessly with Unity’s animation controller and ensures correct animations are played based on character state.

### Interfaces & Abstract Classes
This template is built with flexibility in mind, leveraging the following interfaces and abstract classes:

- `IDamager` & `AbstractDamagerBase`: Interface and base class for any object capable of dealing damage.
- `IDamageable` & `AbstractDamageableBase`: Interface and base class for any object that can receive damage.
- `ICollector` & `ICollectible`: Interface for objects that can collect and be collected.
- `IInteractor` & `IInteractable`: Interface for objects that can interact with and be interacted by characters or other game entities.

### Editor Scripting
The template features several custom editor scripts to streamline character creation and configuration:

- **TopDownCharacterConfigSOEditor:** Automatically hides unused variables in the editor for a cleaner experience.
- **TopDownCharacterConfigCreator:** Provides an intuitive window for creating and managing `TopDownCharacterConfigSO` instances.
- **TopDownCharacterCreator:** Enables quick character creation through the Unity editor, complete with components for movement, interaction, collection, and more.

## 6. Roadmap
Planned features for future releases:
- Advanced Test Scene with additional gameplay scenarios.
- Ranged Weapons.
- Jump and Fall Mechanics.
- AI for Enemies and Targets.
- Harvestable and Exploitable Resources.

## 7. Contact Information
- **Developer:** Erkan Yaprak
- **GitHub Profile:** [nakrekarpay1245](https://github.com/nakrekarpay1245)
- **Personal Website:** [erkanyaprak.w3spaces.com](https://erkanyaprak.w3spaces.com)
- **Old Clone Projects:** [All Games](https://erkanyaprak.w3spaces.com/allgames.html)
- **Upcoming Project Promotion Page:** *Hard Deliver* (Upcoming on Steam)
- **Email:** [rknyprk79@gmail.com](mailto:rknyprk79@gmail.com)
- **LinkedIn:** [Erkan Yaprak](https://www.linkedin.com/in/erkan-yaprak)

## 8. Contribution
You can contact us for the contributions you would like to make.

## 9. Support
For any issues or inquiries, please submit an issue on the GitHub repo or contact me via the email provided above.

using MonsterTamer.Inventory;
using MonsterTamer.Party;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MonsterTamer.Characters.Core
{
    /// <summary>
    /// Represents a character in the game world (Player, Trainer, NPC).
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterStateController))]
    internal sealed class Character : MonoBehaviour
    {
        private const float GridSnapX = 0.5f;
        private const float GridSnapY = 1f;

        [SerializeField, Required] private CharacterDefinition definition;

        internal CharacterDefinition Definition => definition;
        internal CharacterStateController StateController { get; private set; }
        internal InventoryManager Inventory { get; private set; }
        internal PartyManager Party { get; private set; }

        private void Awake()
        {
            StateController = GetComponent<CharacterStateController>();
            Inventory = new InventoryManager(definition.InventoryDefinition);
            Party = new PartyManager(definition.PartyDefinition);

            SnapToGrid();
        }

        internal void Relocate(Vector3 position)
        {
            transform.position = position;
            SnapToGrid();
        }

        private void SnapToGrid()
        {
            Vector3 snapedPosition = transform.position;
            transform.position = new Vector3(
                Mathf.Round(snapedPosition.x / GridSnapX) * GridSnapX,
                Mathf.Floor(snapedPosition.y / GridSnapY) * GridSnapY,
                0f
            );
        }
    }
}

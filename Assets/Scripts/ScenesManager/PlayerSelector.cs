using Players;
using UnityEngine;
using UnityEngine.UI;

namespace ScenesManager
{
    public class PlayerSelector : MonoBehaviour
    {
        [SerializeField] private Text usernameText;
        [SerializeField] private GameObject playerLayoutGroup;

        public void SetupHomeScreen(PlayerController playerController)
        {
            // TODO: Get player real username
            usernameText.text = "Username";

            InstantiateSelectedCharacter(playerController);
        }

        public void InstantiateSelectedCharacter(PlayerController playerController)
        {
            if (playerLayoutGroup.transform.childCount > 0)
            {
                Destroy(playerLayoutGroup.transform.GetChild(0).gameObject);
            }

            // TODO: Spawn correct player character and change Rotation
            GameObject player = Instantiate(playerController.Players[0].equippedCharacter.prefab, Vector3.zero + new Vector3(-200f, -25, 0), Quaternion.Euler(0, 150, 0));
            player.transform.SetParent(playerLayoutGroup.transform, false);
            player.transform.localScale = new Vector3(150, 150, 150);
        }
    }
}

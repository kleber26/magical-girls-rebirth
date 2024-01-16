using System.Collections.Generic;
using Players;
using ScriptableObjects.CharacterObjects;
using ScriptableObjects.Skills;
using UnityEngine;
using UnityEngine.UI;

public class HomeUi : MonoBehaviour
{
    [SerializeField] Text hpGo;
    [SerializeField] Text atkGo;
    [SerializeField] Text defGo;
    [SerializeField] Text staminaGo;

    [SerializeField] Image s1Sprite;
    [SerializeField] Text s1Name;
    [SerializeField] Text s1SDesc;

    [SerializeField] Image s2Sprite;
    [SerializeField] Text s2Name;
    [SerializeField] Text s2SDesc;

    [SerializeField] Image s3Sprite;
    [SerializeField] Text s3Name;
    [SerializeField] Text s3SDesc;

    string hp;
    string atk;
    string def;
    string stamina;

    PlayerController playerController;

    public void Initialize(PlayerController playerController)
    {
        this.playerController = playerController;
        SetUiStatistics();
    }

    public void SetUiStatistics()
    {
        UpdateStats();
        hpGo.text = "LIFE: " + hp;
        atkGo.text = "ATTACK: " + atk;
        defGo.text = "DEFENSE: " + def;
        staminaGo.text = "STAMINA: " + stamina;
        UpdateSkills();
    }

    private void UpdateStats()
    {
        CharacterObject character = playerController.MainPlayer().equippedCharacter;

        hp = character.life.ToString();
        atk = character.atk.ToString();
        def = character.def + "%";
        stamina = character.stamina.ToString();
    }

    private void UpdateSkills()
    {
        List<SkillObject> playerSkills = playerController.MainPlayer().equippedCharacter.skills;

        s1Sprite.sprite = playerSkills[0].skillSprite;
        s1Name.text = playerSkills[0].name;
        s1SDesc.text = playerSkills[0].description;

        s2Sprite.sprite = playerSkills[1].skillSprite;
        s2Name.text = playerSkills[1].name;
        s2SDesc.text = playerSkills[1].description;

        s3Sprite.sprite = playerSkills[2].skillSprite;
        s3Name.text = playerSkills[2].name;
        s3SDesc.text = playerSkills[2].description;
    }
}

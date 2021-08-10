using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Character
{

    public string name;

    public CharacterFunction function;

    public string currentLocation;

    public Character()
    {
        this.name = "";
        this.function = CharacterFunction.MainCharacter;
    }
    public Character(string newName, CharacterFunction characterFunction)
    {
        name = newName;
        function = characterFunction;
    }
}

[System.Serializable]
public class Npc : Character
{
    public bool alreadyMet;
    [TextArea]
    public string description;
    public int relationshipPoint;
    [TextArea]
    public string extraInfo;

    public Npc(string newName, CharacterFunction characterFunction)
    {
        name = newName;
        function = characterFunction;
        alreadyMet = false;
        description = "";
        relationshipPoint = 0;
        extraInfo = "";
    }


    public Npc(string newName, CharacterFunction characterFunction, bool _alreadyMet, string _description, int _relationshipPoint, string _extraInfo)
    {
        name = newName;
        function = characterFunction;
        alreadyMet = _alreadyMet;
        description = _description;
        relationshipPoint = _relationshipPoint;
        extraInfo = _extraInfo;
    }


}


[System.Serializable]
public class MainCharacter : Character
{
    public int experience;
    public int money;
    public int currentEnergy;
    public List<Skill> skills;

    public MainCharacter()
    {
        int maxEnergy = 3;
        name = "Dash";
        function = CharacterFunction.MainCharacter;
        money = 0;
        experience = 0;
        skills = new List<Skill>();
        Array skillArray = Enum.GetValues(typeof(SkillType));
        foreach (SkillType skill in skillArray)
        {
            switch (skill)
            {
                case SkillType.Energy:
                    skills.Add(new Skill(skill, "Description Skill 1", false, maxEnergy));
                    break;
                case SkillType.Riding:
                    skills.Add(new Skill(skill, "Description Skill 2", false, 0));
                    break;
                case SkillType.Study:
                    skills.Add(new Skill(skill, "Description Skill 3", false, 0));
                    break;
                default:
                    break;
            }
        }

        currentEnergy = maxEnergy;
    }

    public MainCharacter(MainCharacter mainCharacter)
    {
        name = mainCharacter.name;
        function = CharacterFunction.MainCharacter;
        currentLocation = mainCharacter.currentLocation;

        experience = mainCharacter.experience;
        money = mainCharacter.money;
        currentEnergy = mainCharacter.currentEnergy;
        skills = mainCharacter.skills;
    }

    public int GetSkillValue(SkillType skillType)
    {
        foreach (Skill skill in skills)
        {
            if (skill.skillType == skillType)
            {
                return skill.value;
            }
        }
        return -1;
    }

    public void SetSkillValue(SkillType skillType, int value)
    {
        foreach (Skill skill in skills)
        {
            if (skill.skillType == skillType)
            {
                skill.value = value;
            }
        }
    }

    public void IncreaseSkillValue(SkillType skillType)
    {
        foreach (Skill skill in skills)
        {
            if (skill.skillType == skillType)
            {
                skill.value += 1;
            }
        }
    }

    public bool DecreaseEnergy()
    {
        if (currentEnergy - 1 < 0)
            return false;
        else
        {
            currentEnergy -= 1;
            return true;
        }

    }

    public void FillEnergy()
    {
        currentEnergy = GetSkillValue(SkillType.Energy);
    }


}

[System.Serializable]
public class Skill
{
    public SkillType skillType;
    public bool unlocked;
    public string SkillName;
    public int value;
    [TextArea]
    public string informations;

    public Skill(SkillType _skillType, string description, bool _unlocked, int _value)
    {
        skillType = _skillType;
        SkillName = "- " + _skillType.ToString().ToUpper() + " SKILL -";
        value = _value;
        informations = description;
        unlocked = _unlocked;
    }
}


/// <summary>
/// Rename skills here
/// </summary>
public enum SkillType
{
    Energy, // Skill1
    Riding, // Skill2
    Study  // Skill3
}


/// <summary>
/// PlayerInfo.CreateCharacters() contain the default name for each npcs.
/// </summary>
public enum CharacterFunction
{
    MainCharacter,
    test
}

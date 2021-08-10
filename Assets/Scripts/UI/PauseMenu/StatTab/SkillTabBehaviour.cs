using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTabBehaviour : MonoBehaviour {

    public GameObject skillPanel;
    public GameObject NpcPanel;

    public GameObject SkillUIPrefab;
    public GameObject NpcFramePrefab;

    public List<Skill> skills;
    public List<Npc> npcs;

    // Use this for initialization
    void Start()
    {

        skills = GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.mainCharacter.skills;

        npcs = GameManager.instance.managers.variablesManager.gameSavedData.GameInfo.Npcs;

        foreach (Skill skill in skills)
        {
            if (skill.unlocked)
            {
                SpawnSkill(skill);
            }
        }

        foreach(Npc npc in npcs)
        {
            if(npc.alreadyMet)
            {
                SpawnNpcFrame(npc);
            }
        }
    }

	public void SpawnSkill(Skill skill)
    {
        GameObject go = Instantiate(SkillUIPrefab, skillPanel.transform);
        go.name = skill.skillType.ToString() + " preview";
        SkillUIBehaviour skillUI = go.GetComponent<SkillUIBehaviour>();
        skillUI.skillToDisplay = skill;
    }

    public void SpawnNpcFrame(Npc npc)
    {
        GameObject go = Instantiate(NpcFramePrefab, NpcPanel.transform);
        go.name = npc.name.ToString() + " frame preview";
        NpcFrameBehaviour npcFrame = go.GetComponent<NpcFrameBehaviour>();
        npcFrame.npcToDisplay = npc;
    }
}

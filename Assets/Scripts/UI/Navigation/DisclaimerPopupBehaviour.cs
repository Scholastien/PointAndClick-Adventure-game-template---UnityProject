using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisclaimerPopupBehaviour : MonoBehaviour
{
    public int saveID;
    public DisclaimerType disclaimerType;

    [Header("UI")]
    public Image imgPreview;

    public TextMeshProUGUI saveType;
    public TextMeshProUGUI customName;

    public TextMeshProUGUI playerLocationAndTime;
    public TextMeshProUGUI money;

    public void RemplaceInfo(UI_SavedData savedData)
    {
        saveID = savedData.id;
        imgPreview.sprite = savedData.tmp_SpritePreview;
        saveType.text = savedData.savedTypeOrIndex;
        customName.text = savedData.savedCustomName != "" ? savedData.savedNameWithTimestamp : savedData.savedCustomName;
        playerLocationAndTime.text = savedData.savedPlayerLocationAndTime;
        money.text = savedData.savedMoney;
    }
}

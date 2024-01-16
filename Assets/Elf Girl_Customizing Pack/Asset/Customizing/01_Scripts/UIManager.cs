using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public RectTransform Btn_Customizing;
    float Btn_Customizing_Start_x = 200f;
    float Btn_Customizing_End_x = 1300;

    void Start()
    {
        Hair_DyeingColor = new Color[5];
        Hair_DyeingColor[0] = new Color(1, 1, 1, 1);
        Hair_DyeingColor[1] = new Color(1, 0.7f, 0.7f, 1);
        Hair_DyeingColor[2] = new Color(1, 1, 0, 1);
        Hair_DyeingColor[3] = new Color(0.6f, 1, 0.6f, 1);
        Hair_DyeingColor[4] = new Color(0 / 255f, 202 / 255f, 255 / 255f, 1);

        Face_DyeingColor = new Color[5];
        Face_DyeingColor[0] = new Color(1, 1, 1, 1);
        Face_DyeingColor[1] = new Color(255/255f, 225/255f, 225/255f, 1);
        Face_DyeingColor[2] = new Color(255 / 255f, 190 / 255f, 190 / 255f, 1);
        Face_DyeingColor[3] = new Color(255 / 255f, 188 / 255f, 236 / 255f, 1);
        Face_DyeingColor[4] = new Color(255 / 255f, 255 / 255f, 200 / 255f, 1);

        Text_DyeingHairName.text = Text_DyeingHairName.transform.parent.name.Remove(0, 10) + ": " + "<size=30><color=RED>" + Hair_DyeingColor[DyeingHairNumber] + "</color></size>";
        Text_DyeingFaceName.text = Text_DyeingFaceName.transform.parent.name.Remove(0, 10) + ": " + "<size=30><color=RED>" + Face_DyeingColor[DyeingFaceNumber] + "</color></size>";
        Text_DyeingBodyName.text = Text_DyeingBodyName.transform.parent.name.Remove(0, 10) + ": " + "<color=RED>" + Ranger_DyeingBodyList[0].name + "</color>";
        Text_DyeingAccessoryName.text = Text_DyeingAccessoryName.transform.parent.name.Remove(0, 10) + ": " + "<color=RED>" + Ranger_DyeingAccessoryList[0].name + "</color>";

        Btn_Reset();
    }

    public void Btn_Reset()
    {
        ClassList[ClassNumber].gameObject.SetActive(false);
        ClassList[0].gameObject.SetActive(true);

        Btns1.SetActive(true);
        Btns2.SetActive(false);
        Btn_Look();

        NameReset();

        ClassNumber = 0;
       

        DyeingHairNumber = 0;
        DyeingFaceNumber = 0;
        DyeingBodyNumber = 0;
        DyeingAccessoryNumber = 0;


        IS_Customizing = false;
        Btn_Customizing.anchoredPosition = new Vector3(0, Btn_Customizing_Start_x, 0);
    }


    public void Btn_Customizing_Animation()
    {
        if (!IS_Customizing)
        {
            IS_Customizing = true;
            StartCoroutine(IE_Btn_Customizing_Animation(0.5f));
            return;
        }

        if (IS_Customizing)
        {
            IS_Customizing = false;
            StartCoroutine(IE_Btn_Customizing_Animation(0.5f));
            return;
        }
    }

    bool IS_Customizing = false;
    IEnumerator IE_Btn_Customizing_Animation(float _time)
    {
        Btn_Customizing.GetComponent<Button>().enabled = false;

        if (IS_Customizing)
        {
            float t = 0;
            while (t < _time)
            {
                t += Time.deltaTime;
                Btn_Customizing.anchoredPosition = Vector3.Lerp(new Vector3(0, Btn_Customizing_Start_x, 0), new Vector3(0, Btn_Customizing_End_x, 0), t / _time);
                yield return null;
            }

            Btn_Customizing.anchoredPosition = new Vector3(0, Btn_Customizing_End_x, 0);
        }

        if (!IS_Customizing)
        {
            float t = 0;
            while (t < _time)
            {
                t += Time.deltaTime;
                Btn_Customizing.anchoredPosition = Vector3.Lerp(new Vector3(0, Btn_Customizing_End_x, 0), new Vector3(0, Btn_Customizing_Start_x, 0), t / _time);
                yield return null;
            }

            Btn_Customizing.anchoredPosition = new Vector3(0, Btn_Customizing_Start_x, 0);
        }

        Btn_Customizing.GetComponent<Button>().enabled = true;
    }

    public eClassType ClassType;
    public GameObject[] ClassList,
                        Ranger_HairList, Ranger_FaceList, Ranger_BodyList, Ranger_AccessoryList,
                        Healer_HairList, Healer_FaceList, Healer_BodyList, Healer_AccessoryList,
                        Wizard_HairList, Wizard_FaceList, Wizard_BodyList, Wizard_AccessoryList;
    public int ClassNumber, HairNumber, FaceNumber, BodyNumber, AccessoryNumber = 0;
    public Text Text_ClassName, Text_HairName, Text_FaceName, Text_BodyName, Text_AccessoryName;


    public Texture[] Ranger_DyeingBodyList, Ranger_DyeingAccessoryList,
                     Healer_DyeingBodyList, Healer_DyeingAccessoryList,
                     Wizard_DyeingBodyList, Wizard_DyeingAccessoryList;
    public Color[] Hair_DyeingColor, Face_DyeingColor;
    public int DyeingHairNumber, DyeingFaceNumber, DyeingBodyNumber, DyeingAccessoryNumber = 0;
    public Text Text_DyeingHairName, Text_DyeingFaceName, Text_DyeingBodyName, Text_DyeingAccessoryName;

    public GameObject Btns1, Btns2, Look, Dyeing;
    public Image Btn_Look_Image, Btn_Dyeing_Image;
    public void Btn_Look()
    {
        Look.SetActive(true);
        Dyeing.SetActive(false);
        Btn_Look_Image.color = new Color(1, 1, 1, 1);
        Btn_Dyeing_Image.color = new Color(1, 0.65f, 0.65f, 1);
    }
    public void Btn_Dyeing()
    {
        Look.SetActive(false);
        Dyeing.SetActive(true);
        Btn_Look_Image.color = new Color(1, 0.65f, 0.65f, 1);
        Btn_Dyeing_Image.color = new Color(1, 1, 1, 1);
    }

    public void Btn_SelectClass()
    {
        Btns1.SetActive(false);
        Btns2.SetActive(true);

        ClassType = (eClassType)ClassNumber;
        NameReset();
    }

    public void Btn_Class()
    {
        Changer(ClassList, ref ClassNumber, Text_ClassName);
    }

    public void Btn_Hair()
    {
        if (ClassType == eClassType.RANGER)
        {
            Btn_RangerHair();
            return;
        }
        if (ClassType == eClassType.HEALER)
        {
            Btn_HealerHair();
            return;
        }
        if (ClassType == eClassType.WIZARD)
        {
            Btn_WizardHair();
            return;
        }
    }
    public void Btn_Face()
    {
        if (ClassType == eClassType.RANGER)
        {
            Btn_RangerFace();
            return;
        }
        if (ClassType == eClassType.HEALER)
        {
            Btn_HealerFace();
            return;
        }
        if (ClassType == eClassType.WIZARD)
        {
            Btn_WizardFace();
            return;
        }
    }
    public void Btn_Body()
    {
        if (ClassType == eClassType.RANGER)
        {
            Btn_RangerBody();
            return;
        }
        if (ClassType == eClassType.HEALER)
        {
            Btn_HealerBody();
            return;
        }
        if (ClassType == eClassType.WIZARD)
        {
            Btn_WizardBody();
            return;
        }
    }
    public void Btn_Accessory()
    {
        if (ClassType == eClassType.RANGER)
        {
            Btn_RangerAccessory();
            return;
        }
        if (ClassType == eClassType.HEALER)
        {
            Btn_HealerAccessory();
            return;
        }
        if (ClassType == eClassType.WIZARD)
        {
            Btn_WizardAccessory();
            return;
        }
    }

    private void Btn_RangerHair()
    {
        Changer(Ranger_HairList, ref HairNumber, Text_HairName);
    }
    private void Btn_RangerFace()
    {
        Changer(Ranger_FaceList, ref FaceNumber, Text_FaceName);
    }
    private void Btn_RangerBody()
    {
        Changer(Ranger_BodyList, ref BodyNumber, Text_BodyName);
    }
    private void Btn_RangerAccessory()
    {
        Changer(Ranger_AccessoryList, ref AccessoryNumber, Text_AccessoryName);
    }

    private void Btn_HealerHair()
    {
        Changer(Healer_HairList, ref HairNumber, Text_HairName);
    }
    private void Btn_HealerFace()
    {
        Changer(Healer_FaceList, ref FaceNumber, Text_FaceName);
    }
    private void Btn_HealerBody()
    {
        Changer(Healer_BodyList, ref BodyNumber, Text_BodyName);
    }
    private void Btn_HealerAccessory()
    {
        Changer(Healer_AccessoryList, ref AccessoryNumber, Text_AccessoryName);
    }

    private void Btn_WizardHair()
    {
        Changer(Wizard_HairList, ref HairNumber, Text_HairName);
    }
    private void Btn_WizardFace()
    {
        Changer(Wizard_FaceList, ref FaceNumber, Text_FaceName);
    }
    private void Btn_WizardBody()
    {
        Changer(Wizard_BodyList, ref BodyNumber, Text_BodyName);
    }
    private void Btn_WizardAccessory()
    {
        Changer(Wizard_AccessoryList, ref AccessoryNumber, Text_AccessoryName);
    }

    public void Changer(GameObject[] _GoList, ref int _Number, Text _text)
    {
        if (_GoList.Length - 1 <= _Number)
        {
            _GoList[_GoList.Length - 1].gameObject.SetActive(false);
            _Number = 0;
        }
        else
        {
            _GoList[_Number].gameObject.SetActive(false);
            _Number++;
        }
        _GoList[_Number].gameObject.SetActive(true);

        if(_text)
           _text.text = _text.transform.parent.name.Remove(0, 4) + ": " + "<color=RED>" + _GoList[_Number].name + "</color>";
    }


    public void Changer_Btn_Dyeing_Hair(GameObject[] _GoList, ref int _Number, Text _text)
    {
        if (Hair_DyeingColor.Length - 1 <= DyeingHairNumber)
        {
            DyeingHairNumber = 0;
        }
        else
        {
            DyeingHairNumber++;
        }

        _GoList[_Number].GetComponent<Renderer>().material.color = Hair_DyeingColor[DyeingHairNumber];
        _text.text = _text.transform.parent.name.Remove(0, 10) + ": " + "<size=30><color=RED>" + Hair_DyeingColor[DyeingHairNumber] + "</color></size>";

    }
    public void Changer_Btn_Dyeing_Face(GameObject[] _GoList, ref int _Number, Text _text)
    {
        if (Face_DyeingColor.Length - 1 <= DyeingFaceNumber)
        {
            DyeingFaceNumber = 0;
        }
        else
        {
            DyeingFaceNumber++;
        }

        _GoList[_Number].GetComponent<Renderer>().material.color = Face_DyeingColor[DyeingFaceNumber];
        _text.text = _text.transform.parent.name.Remove(0, 10) + ": " + "<size=30><color=RED>" + Face_DyeingColor[DyeingFaceNumber] + "</color></size>";
    }
    public void Changer_Btn_Dyeing_Body(GameObject[] _GoList, ref int _Number, Text _text)
    {
        if (_Number == 0)
        {
            if (Ranger_DyeingBodyList.Length - 1 <= DyeingBodyNumber)
            {
                DyeingBodyNumber = 0;
            }
            else
            {
                DyeingBodyNumber++;
            }

            _GoList[_Number].GetComponent<Renderer>().material.SetTexture("_MainTex", Ranger_DyeingBodyList[DyeingBodyNumber]);
            _text.text = _text.transform.parent.name.Remove(0, 10) + ": " + "<color=RED>" + Ranger_DyeingBodyList[DyeingBodyNumber].name + "</color>";
        }

        if (_Number == 1)
        {
            if (Healer_DyeingBodyList.Length - 1 <= DyeingBodyNumber)
            {
                DyeingBodyNumber = 0;
            }
            else
            {
                DyeingBodyNumber++;
            }

            _GoList[_Number].GetComponent<Renderer>().material.SetTexture("_MainTex", Healer_DyeingBodyList[DyeingBodyNumber]);
            _text.text = _text.transform.parent.name.Remove(0, 10) + ": " + "<color=RED>" + Healer_DyeingBodyList[DyeingBodyNumber].name + "</color>";
        }

        if (_Number == 2)
        {
            if (Wizard_DyeingBodyList.Length - 1 <= DyeingBodyNumber)
            {
                DyeingBodyNumber = 0;
            }
            else
            {
                DyeingBodyNumber++;
            }

            _GoList[_Number].GetComponent<Renderer>().material.SetTexture("_MainTex", Wizard_DyeingBodyList[DyeingBodyNumber]);
            _text.text = _text.transform.parent.name.Remove(0, 10) + ": " + "<color=RED>" + Wizard_DyeingBodyList[DyeingBodyNumber].name + "</color>";
        }

    }
    public void Changer_Btn_Dyeing_Accessory(GameObject[] _GoList, ref int _Number, Text _text)
    {
        if (_Number == 0)
        {
            if (Ranger_DyeingAccessoryList.Length - 1 <= DyeingAccessoryNumber)
            {
                DyeingAccessoryNumber = 0;
            }
            else
            {
                DyeingAccessoryNumber++;
            }

            _GoList[_Number].GetComponent<Renderer>().material.SetTexture("_MainTex", Ranger_DyeingAccessoryList[DyeingAccessoryNumber]);
            _text.text = _text.transform.parent.name.Remove(0, 10) + ": " + "<color=RED>" + Ranger_DyeingAccessoryList[DyeingAccessoryNumber].name + "</color>";
        }

        if (_Number == 1)
        {
            if (Healer_DyeingAccessoryList.Length - 1 <= DyeingAccessoryNumber)
            {
                DyeingAccessoryNumber = 0;
            }
            else
            {
                DyeingAccessoryNumber++;
            }

            _GoList[_Number].GetComponent<Renderer>().material.SetTexture("_MainTex", Healer_DyeingAccessoryList[DyeingAccessoryNumber]);
            _text.text = _text.transform.parent.name.Remove(0, 10) + ": " + "<color=RED>" + Healer_DyeingAccessoryList[DyeingAccessoryNumber].name + "</color>";
        }

        if (_Number == 2)
        {
            if (Wizard_DyeingAccessoryList.Length - 1 <= DyeingAccessoryNumber)
            {
                DyeingAccessoryNumber = 0;
            }
            else
            {
                DyeingAccessoryNumber++;
            }

            _GoList[_Number].GetComponent<Renderer>().material.SetTexture("_MainTex", Wizard_DyeingAccessoryList[DyeingAccessoryNumber]);
            _text.text = _text.transform.parent.name.Remove(0, 10) + ": " + "<color=RED>" + Wizard_DyeingAccessoryList[DyeingAccessoryNumber].name + "</color>";
        }

    }

    public void Btn_Dyeing_Hair()
    {
        if (ClassType == eClassType.RANGER)
        {
            Changer_Btn_Dyeing_Hair(Ranger_HairList, ref HairNumber, Text_DyeingHairName);
            return;
        }
        if (ClassType == eClassType.HEALER)
        {
            Changer_Btn_Dyeing_Hair(Healer_HairList, ref HairNumber, Text_DyeingHairName);
            return;
        }
        if (ClassType == eClassType.WIZARD)
        {
            Changer_Btn_Dyeing_Hair(Wizard_HairList, ref HairNumber, Text_DyeingHairName);
            return;
        }
    }
    public void Btn_Dyeing_Face()
    {
        if (ClassType == eClassType.RANGER)
        {
            Changer_Btn_Dyeing_Face(Ranger_FaceList, ref FaceNumber, Text_DyeingFaceName);
            return;
        }
        if (ClassType == eClassType.HEALER)
        {
            Changer_Btn_Dyeing_Face(Healer_FaceList, ref FaceNumber, Text_DyeingFaceName);
            return;
        }
        if (ClassType == eClassType.WIZARD)
        {
            Changer_Btn_Dyeing_Face(Wizard_FaceList, ref FaceNumber, Text_DyeingFaceName);
            return;
        }
    }

    public void Btn_Dyeing_Body()
    {
        if (ClassType == eClassType.RANGER)
        {
            Changer_Btn_Dyeing_Body(Ranger_BodyList, ref BodyNumber, Text_DyeingBodyName);
            return;
        }
        if (ClassType == eClassType.HEALER)
        {
            Changer_Btn_Dyeing_Body(Healer_BodyList, ref BodyNumber, Text_DyeingBodyName);
            return;
        }
        if (ClassType == eClassType.WIZARD)
        {
            Changer_Btn_Dyeing_Body(Wizard_BodyList, ref BodyNumber, Text_DyeingBodyName);
            return;
        }
    }
    public void Btn_Dyeing_Accessory()
    {
        if (ClassType == eClassType.RANGER)
        {
            Changer_Btn_Dyeing_Accessory(Ranger_AccessoryList, ref AccessoryNumber, Text_DyeingAccessoryName);
            return;
        }
        if (ClassType == eClassType.HEALER)
        {
            Changer_Btn_Dyeing_Accessory(Healer_AccessoryList, ref AccessoryNumber, Text_DyeingAccessoryName);
            return;
        }
        if (ClassType == eClassType.WIZARD)
        {
            Changer_Btn_Dyeing_Accessory(Wizard_AccessoryList, ref AccessoryNumber, Text_DyeingAccessoryName);
            return;
        }
    }

    public void NameReset()
    {
        Text_ClassName.text = Text_ClassName.text = Text_ClassName.transform.parent.name.Remove(0, 4) + ": " + "<color=RED>" + ClassList[0].name + "</color>";

        if (ClassType == eClassType.RANGER)
        {
            Text_HairName.text = Text_HairName.text = Text_HairName.transform.parent.name.Remove(0, 4) + ": " + "<color=RED>" + Ranger_HairList[0].name + "</color>";
            Text_FaceName.text = Text_FaceName.text = Text_FaceName.transform.parent.name.Remove(0, 4) + ": " + "<color=RED>" + Ranger_FaceList[0].name + "</color>";
            Text_BodyName.text = Text_BodyName.text = Text_BodyName.transform.parent.name.Remove(0, 4) + ": " + "<color=RED>" + Ranger_BodyList[0].name + "</color>";
            Text_AccessoryName.text = Text_AccessoryName.text = Text_AccessoryName.transform.parent.name.Remove(0, 4) + ": " + "<color=RED>" + Ranger_AccessoryList[0].name + "</color>";

            Ranger_HairList[HairNumber].gameObject.SetActive(false);
            Ranger_HairList[0].gameObject.SetActive(true);
            Ranger_FaceList[FaceNumber].gameObject.SetActive(false);
            Ranger_FaceList[0].gameObject.SetActive(true);
            Ranger_BodyList[BodyNumber].gameObject.SetActive(false);
            Ranger_BodyList[0].gameObject.SetActive(true);
            Ranger_AccessoryList[AccessoryNumber].gameObject.SetActive(false);
            Ranger_AccessoryList[0].gameObject.SetActive(true);

            HairNumber = 0;
            FaceNumber = 0;
            BodyNumber = 0;
            AccessoryNumber = 0;

            return;
        }
        if (ClassType == eClassType.HEALER)
        {
            Text_HairName.text = Text_HairName.text = Text_HairName.transform.parent.name.Remove(0, 4) + ": " + "<color=RED>" + Healer_HairList[1].name + "</color>";
            Text_FaceName.text = Text_FaceName.text = Text_FaceName.transform.parent.name.Remove(0, 4) + ": " + "<color=RED>" + Healer_FaceList[1].name + "</color>";
            Text_BodyName.text = Text_BodyName.text = Text_BodyName.transform.parent.name.Remove(0, 4) + ": " + "<color=RED>" + Healer_BodyList[1].name + "</color>";
            Text_AccessoryName.text = Text_AccessoryName.text = Text_AccessoryName.transform.parent.name.Remove(0, 4) + ": " + "<color=RED>" + Healer_AccessoryList[1].name + "</color>";

            Healer_HairList[HairNumber].gameObject.SetActive(false);
            Healer_HairList[1].gameObject.SetActive(true);
            Healer_FaceList[FaceNumber].gameObject.SetActive(false);
            Healer_FaceList[1].gameObject.SetActive(true);
            Healer_BodyList[BodyNumber].gameObject.SetActive(false);
            Healer_BodyList[1].gameObject.SetActive(true);
            Healer_AccessoryList[AccessoryNumber].gameObject.SetActive(false);
            Healer_AccessoryList[1].gameObject.SetActive(true);

            HairNumber = 1;
            FaceNumber = 1;
            BodyNumber = 1;
            AccessoryNumber = 1;

            return;
        }
        if (ClassType == eClassType.WIZARD)
        {
            Text_HairName.text = Text_HairName.text = Text_HairName.transform.parent.name.Remove(0, 4) + ": " + "<color=RED>" + Wizard_HairList[2].name + "</color>";
            Text_FaceName.text = Text_FaceName.text = Text_FaceName.transform.parent.name.Remove(0, 4) + ": " + "<color=RED>" + Wizard_FaceList[2].name + "</color>";
            Text_BodyName.text = Text_BodyName.text = Text_BodyName.transform.parent.name.Remove(0, 4) + ": " + "<color=RED>" + Wizard_BodyList[2].name + "</color>";
            Text_AccessoryName.text = Text_AccessoryName.text = Text_AccessoryName.transform.parent.name.Remove(0, 4) + ": " + "<color=RED>" + Wizard_AccessoryList[2].name + "</color>";

            Wizard_HairList[HairNumber].gameObject.SetActive(false);
            Wizard_HairList[2].gameObject.SetActive(true);
            Wizard_FaceList[FaceNumber].gameObject.SetActive(false);
            Wizard_FaceList[2].gameObject.SetActive(true);
            Wizard_BodyList[BodyNumber].gameObject.SetActive(false);
            Wizard_BodyList[2].gameObject.SetActive(true);
            Wizard_AccessoryList[AccessoryNumber].gameObject.SetActive(false);
            Wizard_AccessoryList[2].gameObject.SetActive(true);

            HairNumber = 2;
            FaceNumber = 2;
            BodyNumber = 2;
            AccessoryNumber = 2;

            return;
        }
    }
}

public enum eClassType
{
    RANGER = 0,
    HEALER,
    WIZARD,

}



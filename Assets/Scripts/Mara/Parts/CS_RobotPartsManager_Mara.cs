using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_RobotPartsManager_Mara : MonoBehaviour
{
  CS_ArmRobotPart_Mara leftArmPart = new CS_ArmRobotPart_Mara();
  CS_ArmRobotPart_Mara rightArmPart = new CS_ArmRobotPart_Mara();
  CS_LegsRobotPart_Mara legsPart = new CS_LegsRobotPart_Mara();
  CS_BackRobotPart_Mara backPart = new CS_BackRobotPart_Mara();
  CS_TorsoRobotPart_Mara torsoPart = new CS_TorsoRobotPart_Mara();

  // Start is called before the first frame update
  void Start()
  {
    leftArmPart.Start(this);
    rightArmPart.Start(this);
    legsPart.Start(this);
    backPart.Start(this);
    torsoPart.Start(this);
  }

  // Update is called once per frame
  void Update()
  {
    leftArmPart.Update(this);
    rightArmPart.Update(this);
    legsPart.Update(this);
    backPart.Update(this);
    torsoPart.Update(this);
  }

  // Update is called once per frame
  void FixedUpdate()
  {
    leftArmPart.FixedUpdate(this);
    rightArmPart.FixedUpdate(this);
    legsPart.FixedUpdate(this);
    backPart.FixedUpdate(this);
    torsoPart.FixedUpdate(this);
  }

  void SetLeftArmPart(CS_ArmRobotPart_Mara part)
  {
    leftArmPart = part;
    leftArmPart.Start(this);
  }

  void SetRightArmPart(CS_ArmRobotPart_Mara part)
  {
    rightArmPart = part;
    rightArmPart.Start(this);
  }

  void SetLegsPart(CS_LegsRobotPart_Mara part)
  {
    legsPart = part;
    legsPart.Start(this);
  }

  void SetBackPart(CS_BackRobotPart_Mara part)
  {
    backPart = part;
    backPart.Start(this);
  }

  void SetTorsoPart(CS_TorsoRobotPart_Mara part)
  {
    torsoPart = part;
    torsoPart.Start(this);
  }
}

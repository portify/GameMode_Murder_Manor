datablock PlayerData(PlayerMurderArmor : PlayerStandardArmor)
{
  uiName = "Murder Manor Player";
  isMurderPlayer = 1;

  cameraMaxDist = 3;
  cameraVerticalOffset = 1.25;
  maxFreelookAngle = 0.95;

  canJet = 0;
  maxTools = 3;
  mass = 120;

  //minImpactSpeed = 15;
  minImpactSpeed = 18;
  //speedDamageScale = 2.3;
  speedDamageScale = 1.6;

  jumpEnergyDrain = 50;
  minJumpEnergy = 50;

  jumpForce = 1200;
};

datablock PlayerData(PlayerMurderRunningArmor : PlayerMurderArmor)
{
  uiName = "";

  showEnergyBar = 0;
  maxForwardSpeed = 10.5;

  runForce = 6000;
  jumpForce = 540;

  minRunEnergy = 2.5;
  minJumpEnergy = 20;

  runEnergyDrain = 2;
  jumpEnergyDrain = 20;
};

function PlayerMurderArmor::onAdd(%this, %obj)
{
  Parent::onAdd(%this, %obj);
  %obj.monitorEnergyLevel();
}

function PlayerMurderArmor::onDamage(%this, %obj, %damage)
{
  Parent::onDamage(%this, %obj, %damage);

  if (%obj.getState() $= "Dead")
  {
    %obj.stopAudio(1);
    return;
  }

  if (%obj.getDamagePercent() >= 0.7 && !%obj.playingHeartbeat)
  {
    %obj.playingHeartbeat = 1;
    // %obj.playAudio(1, MurderHeartbeatLoopSound);
  }
}

function PlayerMurderArmor::onTrigger(%this, %obj, %slot, %state)
{
  Parent::onTrigger(%this, %obj, %slot, %state);

  if (%slot == 4 && %state && 0)
  {
    %obj.changeDataBlock(PlayerMurderRunningArmor);
    %obj.monitorEnergyLevel();
  }
}

function PlayerMurderRunningArmor::onDamage(%this, %obj, %damage)
{
  PlayerMurderArmor::onDamage(%this, %obj, %damage);
}

function PlayerMurderRunningArmor::onTrigger(%this, %obj, %slot, %state)
{
  Parent::onTrigger(%this, %obj, %slot, %state);

  if (%slot == 4 && !%state)
  {
    %obj.changeDataBlock(PlayerMurderArmor);
    %obj.monitorEnergyLevel();
  }
}

function Player::monitorEnergyLevel(%this, %last)
{
  cancel(%this.monitorEnergyLevel);

  if (%this.getState() $= "Dead" || !isObject(%this.client))
  {
    return;
  }

  %show = %this.getEnergyLevel() < %this.getDataBlock().maxEnergy;

  if (%show != %last)
  {
    commandToClient(%this.client, 'ShowEnergyBar', %show);
  }

  %this.monitorEnergyLevel = %this.schedule(100, "monitorEnergyLevel", %show);
}

function Player::murderManorTick(%this)
{
  cancel(%this.murderManorTick);

  if (%this.getState() $= "Dead")
  {
    return;
  }

  %this.murderManorTick = %this.schedule(100, "murderManorTick");
}

function Player::updateAFKKiller(%this, %previous)
{
  cancel(%this.updateAFKKiller);

  if (%this.getState() $= "Dead")
  {
    return;
  }

  %transform = %this.getTransform();

  if (%transform $= %previous)
  {
    messageAll('', "\c3" @ %this.client.getPlayerName() @ " \c0has been killed due to being idle.");
    %this.kill();
  }
  else
  {
    %this.updateAFKKiller = %this.schedule(30000, "updateAFKKiller", %transform);
  }
}

package MurderManorPlayerPackage
{
  function serverCmdLight(%client)
  {
    if (%client.miniGame != $DefaultMiniGame)
    {
      Parent::serverCmdLight(%client);
    }
  }

  function serverCmdDropTool(%client, %slot)
  {
    if (%client.miniGame != $DefaultMiniGame)
    {
      Parent::serverCmdDropTool(%client, %slot);
    }
  }

  function GameConnection::spawnPlayer(%this)
  {
    Parent::spawnPlayer(%this);
    %obj = %this.player;

    if (%this.miniGame != $DefaultMiniGame || !isObject(%obj))
    {
      return;
    }

    %obj.setShapeNameDistance(0);
    %obj.murderManorTick();

    //%obj.schedule(30000, "updateAFKKiller");
    %obj.schedule(50, "updateAFKKiller");
    %this.schedule(0, "updateMurderStatus");

    if (%this.isMurderer)
    {
      %obj.tool[0] = nameToID("MurderKnifeItem");
      messageClient(%this, 'MsgItemPickup', '', 0, %obj.tool[0], 1);

      %high = -1;

      %tool[%high++] = "CloakItem";
      %tool[%high++] = "C4Item";

      %obj.tool[1] = nameToID(%tool[getRandom(%high)]);
      messageClient(%this, 'MsgItemPickup', '', 1, %obj.tool[1], 1);
    }
  }

  function Player::activateStuff(%this)
  {
    Parent::activateStuff(%this);
    %client = %this.client;

    if (!isObject(%client) || %client.miniGame != $DefaultMiniGame)
    {
      return;
    }

    %start = %this.getEyePoint();
    %end = vectorAdd(%start, vectorScale(%this.getEyeVector(), 6));

    %mask = $TypeMasks::FxBrickObjectType | $TypeMasks::PlayerObjectType;
    %ray = containerRayCast(%start, %end, %mask, %this);

    if (!%ray || !(%ray.getType() & $TypeMasks::PlayerObjectType))
    {
      return;
    }

    %other = %ray.client;

    if (!isObject(%other) || %other.miniGame != $DefaultMiniGame)
    {
      return;
    }

    if (%ray.isCloaked || %other.charName $= "")
    {
      return;
    }

    %text = "\c6" @ (%other.charGender ? "His" : "Her") @ " name is \c3" @ %other.charName;
    %text = %text SPC "\c6(" @ %other.getRoleName(1) @ "\c6).\n";

    if (%ray.tool[%ray.currTool].uiName !$= "")
    {
      %text = %text @ "\c6They're holding a \c3" @ %ray.tool[%ray.currTool].uiName @ "\c6.\n";
    }

    %damage = %ray.getDamagePercent();

    if (%damage >= 0.7)
    {
      %text = %text @ "<color:ff66aa>They look badly injured.\n";
    }
    else if (%damage >= 0.35)
    {
      %text = %text @ "<color:ffaa66>They look injured.\n";
    }
    else if (%damage > 0)
    {
      %text = %text @ "\c6They're slightly injured.\n";
    }

    %client.centerPrint(%text, 3);
  }
};

activatePackage("MurderManorPlayerPackage");
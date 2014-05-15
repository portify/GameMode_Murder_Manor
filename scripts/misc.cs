function GameConnection::sendActiveCharacter(%this)
{
  %control = %this.getControlObject();

  if (%control.getType() & $TypeMasks::PlayerObjectType)
  {
    %target = %control.client;
  }
  else if (%control.getType() & $TypeMasks::CameraObjectType)
  {
    %target = %control.getOrbitObject().client;
  }

  if (isObject(%target) && %target.charName !$= "")
  {
    %name = %target.charName;
    %role = %target.getRoleName();
    %group = %target.getGroupName();
  }

  commandToClient(%this, 'MMC_SetCharacter', %name, %role, %group);
}

package MurderManorMiscPackage
{
  function serverCmdDropCameraAtPlayer(%client)
  {
    if (%this.miniGame != $DefaultMiniGame || isObject(%client.player))
    {
      Parent::serverCmdDropCameraAtPlayer(%client);
    }
  }

  function serverCmdDropPlayerAtCamera(%client)
  {
    if (%this.miniGame != $DefaultMiniGame || isObject(%client.player))
    {
      Parent::serverCmdDropPlayerAtCamera(%client);
    }
  }

  function Item::fadeOut(%this)
  {
    Parent::fadeOut(%this);

    if (isObject(%this.spawnBrick) && %this.spawnBrick.getName() $= "_randomItemSpawn")
    {
      %this.spawnBrick.schedule(0, "setItem", 0);
    }
  }

  function GameConnection::setControlObject(%this, %control)
  {
    Parent::setControlObject(%this, %control);

    if (%this.miniGame == $DefaultMiniGame)
    {
      %this.sendActiveCharacter();
    }
  }

  function Camera::setOrbitMode(%this, %obj, %mat, %minDist, %maxDist, %curDist, %ownObj)
  {
    Parent::setOrbitMode(%this, %obj, %mat, %minDist, %maxDist, %curDist, %ownObj);
    %client = %this.getControllingClient();

    if (isObject(%client) && %client.miniGame == $DefaultMiniGame)
    {
      %client.sendActiveCharacter();
    }
  }

  function Camera::setMode(%this, %mode, %orbit)
  {
    Parent::setMode(%this, %mode, %orbit);
    %client = %this.getControllingClient();

    if (isObject(%client) && %client.miniGame == $DefaultMiniGame)
    {
      %client.sendActiveCharacter();
    }
  }

  function Observer::setMode(%this, %mode, %orbit, %a)
  {
    Parent::setMode(%this, %mode, %orbit, %a);
  }

  function Observer::setOrbitObject(%this, %mode, %orbit, %a, %b, %c, %d, %e)
  {
    Parent::setOrbitObject(%this, %mode, %orbit, %a, %b, %c, %d, %e);
  }

  function Observer::onTrigger(%this, %obj, %slot, %state)
  {
    Parent::onTrigger(%this, %obj, %slot, %state);
    %client = %obj.getControllingClient();

    if (isObject(%client) && %client.miniGame == $DefaultMiniGame)
    {
      %client.sendActiveCharacter();
    }
  }
};

activatePackage("MurderManorMiscPackage");
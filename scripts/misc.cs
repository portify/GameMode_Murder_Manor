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
};

activatePackage("MurderManorMiscPackage");
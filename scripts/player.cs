package MurderManorPlayerPackage
{
  function GameConnection::spawnPlayer(%this)
  {
    Parent::spawnPlayer(%this);
    %obj = %this.player;

    if (%this.miniGame != $DefaultMiniGame || !isObject(%obj))
    {
      return;
    }
  }
};

activatePackage("MurderManorPlayerPackage");
function GameConnection::checkClientModules(%this)
{
}

package ClientModuleSPackage
{
  function GameConnection::onConnectRequest(%this, %addr, %lan, %net, %pre, %suf, %rtb, %nonce, %modules, %a, %b, %c)
  {
    %parent = Parent::onConnectRequest(%this, %addr, %lan, %net, %pre, %suf, %rtb, %nonce, %modules, %a, %b, %c);

    if (%parent !$= "")
    {
      return %parent;
    }

    %count = getFieldCount(%modules);

    for (%i = 1; %i < %count; %i++)
    {
      %field = getField(%modules, %i);
      %this.moduleVersion[getWord(%field, 0)] = getWord(%field, 1);
    }

    %this.schedule(0, "checkClientModules");
    return "";
  }
};

activatePackage("ClientModuleSPackage");
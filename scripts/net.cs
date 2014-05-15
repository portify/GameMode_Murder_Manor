// This file handles communication with the client-sided add-on.
$MM::ClientModuleVersion = 1;
$MM::ClientModuleURL = "google.com";

package MurderManorNetPackage
{
  function GameConnection::checkClientModules(%this)
  {
    Parent::checkClientModules(%this);

    if (!isObject(%this))
    {
      return;
    }

    // %version = %this.moduleVersion["MM"];

    // if (%version $= "")
    // {
    //   %this.delete("You need the Murder Manor client to play on this server.\n<a:" @ $MM::ClientModuleURL @ ">Download it here</a>.");
    // }
    // else if (%version < $MM::ClientModuleVersion)
    // {
    //   %this.delete("Your version of the Murder Manor client is outdated.\n<a:" @ $MM::ClientModuleURL @ ">Download the latest version here</a>.");
    // }
  }
};

activatePackage("MurderManorNetPackage");
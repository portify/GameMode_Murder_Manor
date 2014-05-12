// Game CHECK
// Misc CHECK
// Player CHECK
// Chat CHECK
// Character NEED APPEARANCE

exec("./support/misc.cs");

exec("./scripts/game.cs");
exec("./scripts/misc.cs");
exec("./scripts/player.cs");
exec("./scripts/character.cs");
exec("./scripts/chat.cs");

exec("./scripts/items/Cloak.cs");
exec("./scripts/items/C4.cs");

exec("./weapons/weapon_clushido/server.cs");
exec("./weapons/weapon_murderknife/server.cs");

$Pref::HatMod::SaveLoc = "";
$Pref::HatMod::RandomHats = 0;
$Pref::HatMod::HatChance = 0;
$Pref::HatMod::Items = 1;
$Pref::HatMod::HatSharing = 0;
$Pref::HatMod::ClearHats = 0;
$Pref::HatMod::DuplicateHats = 0;
$Pref::HatMod::HatAccess = 0;
$Pref::HatMod::ForceRandom = 0;

function serverCmdWhoIs(%client, %a, %b)
{
  if (!%client.isAdmin && !isEventPending(%client.miniGame.resetSchedule))
  {
    return;
  }

  %search = trim(%a SPC %b);
  %miniGame = %client.miniGame;

  for (%i = 0; %i < %miniGame.numMembers; %i++)
  {
    %member = %miniGame.member[%i];

    if (%member.charName $= "")
    {
      continue;
    }

    if (%search $= "" || striPos(%member.getPlayerName(), %search) != -1 || striPos(%member.charName, %search) != -1)
    {
      %suffix = "(" @ %member.getGroupName() SPC "\c3-" SPC %member.getRoleName() @ "\c3)";
      messageClient(%client, '', "\c3" @ %member.getPlayerName() SPC "\c6is \c3" @ %member.charName SPC %suffix);
    }
  }
}
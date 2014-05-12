$Role::Guest = 0;
$Role::Doctor = 1;
$Role::Detective = 2;
$Role::Guard = 3;
$Role::Family = 4;
$Role::Servant = 5;
$Role::Cook = 6;
$Role::Spy = 7;

function GameConnection::getGroupName(%this)
{
  if (%this.isMurderer)
  {
    return "<color:ff6666>Murderer";
  }
  else
  {
    return "<color:66aaff>Bystander";
  }
}

function GameConnection::getRoleName(%this, %public)
{
  switch (%this.role)
  {
    case $Role::Guest:
      return "<color:ffff66>Guest";
    case $Role::Doctor:
      return "<color:6666ff>Doctor";
    case $Role::Detective:
      return "<color:964b00>Detective";
    case $Role::Guard:
      return "<color:aaffff>Guard";
    case $Role::Family:
      return "<color:ff7722>Family";
    case $Role::Servant:
      return "<color:ff2277>Servant";
    case $Role::Cook:
      return "<color:66ff66>Cook";
    case $Role::Spy:
      if (%public)
      {
        return "<color:ffff66>Guest";
      }
      else
      {
        return "<color:ff66ff>Spy";
      }
  }

  return "<color:ff00ff>Error";
}

function GameConnection::clearMurderManorCharacter(%this)
{
  %this.charName = "";
  %this.charGender = "";
  %this.charColor = "";
  %this.charHair = "";
  %this.charHat = "";
  %this.charFace = "";
  %this.charDecal = "";

  for (%i = 0; %i < %this.charApparelCount; %i++)
  {
    %this.charApparelType[%i] = "";
    %this.charApparelFields[%i] = "";
  }

  %this.charApparelCount = "";
}

function GameConnection::createMurderManorCharacter(%this)
{
  %this.clearMurderManorCharacter();
  %this.charApparelCount = 0;

  if (%this.role == $Role::Detective)
  {
    %this.charGender = 1;
  }
  else
  {
    %this.charGender = getRandom() < 0.6;
  }

  if (%this.role == $Role::Doctor)
  {
    %prefix = "Dr.";
  }
  else if (%this.role == $Role::Family)
  {
    %prefix = %this.charGender ? "Lord" : (getRandom() < 0.3 ? "Ma'am" : "Madam");
  }
  else if (%this.charGender)
  {
    %prefix = getRandom() < 0.75 ? "Mr." : (getRandom() < 0.25 ? "Lord" : "Sir");
  }
  else
  {
    %prefix = getRandom() < 0.3 ? "Miss" : (getRandom() < 0.65 ? "Ms." : "Mrs.");
  }

  %first = getRandom() < 0.375 ? %prefix : getRandomName("first_" @ (%this.charGender ? "male" : "female"));
  %last = getRandomName("last");

  %this.charName = %first SPC %last;

  %this.charColor = getRandomSkinColor();
  %this.charHair = getRandomHairName(%this.charGender);
  %this.charFace = geTrandomFaceName(%this.charGender);
  %this.charDecal = getRandomDecalName();

  if (getRandom(1))
  {
    %this.charHat = getRandomHatName(%this.charGender);
  }

  // ...
}

function GameConnection::appendMurderApparel(%this, %type, %fields)
{
  %this.charApparelType[%this.charApparelCount] = %type;
  %this.charApparelFields[%this.charApparelCount] = %fields;

  %this.charApparelCount++;
}

function getRandomSkinColor()
{
  %index = getRandom(3);

  %color[0] = "0.956863 0.878431 0.784314";
  %color[1] = "1 0.878431 0.611765";
  %color[2] = "1 0.603922 0.423529";
  %color[3] = "0.392157 0.196078 0";

  %r = max(min(getWord(%color[%index], 0) + 0.05 - getRandom() * 0.1, 1), 0);
  %g = max(min(getWord(%color[%index], 1) + 0.05 - getRandom() * 0.1, 1), 0);
  %b = max(min(getWord(%color[%index], 2) + 0.05 - getRandom() * 0.1, 1), 0);

  return %r SPC %g SPC %b SPC 1;
}

function getRandomFaceName(%gender)
{
  %high = -1;
  %choice[%high++] = "smiley";

  if (%gender)
  {
    %choice[%high++] = "Jamie";
    %choice[%high++] = "Male07Smiley";
    %choice[%high++] = "BrownSmiley";
    %choice[%high++] = "smileyOld";
    %choice[%high++] = "smileyEvil1";
    %choice[%high++] = "smileyEvil2";
    %choice[%high++] = "smileyCreepy";
  }
  else
  {
    %choice[%high++] = "smileyFemale1";
  }

  return %choice[getRandom(%high)];
}

function getRandomDecalName()
{
  %high = -1;

  %choice[%high++] = "";
  %choice[%high++] = "Mod-Suit";
  %choice[%high++] = "Mod-Pilot";

  return %choice[getRandom(%high)];
}

function getRandomHairName(%gender)
{
  %high = -1;

  %choice[%high++] = "";
  %choice[%high++] = "";
  %choice[%high++] = "Afro";
  %choice[%high++] = "Balding";
  %choice[%high++] = "Bowl";
  %choice[%high++] = "CombOver";
  //%choice[%high++] = "CornRows";
  %choice[%high++] = "EmoHair";
  %choice[%high++] = "Messy";
  %choice[%high++] = "Parted";
  %choice[%high++] = "Thinning";

  if (!%gender)
  {
    %choice[%high++] = "Girls";
    %choice[%high++] = "Headband";
    %choice[%high++] = "Ponytail";
    %choice[%high++] = "Long";
  }
  else
  {
    %choice[%high++] = "Fancy";
    %choice[%high++] = "Flattop";
    %choice[%high++] = "Freddie";
    %choice[%high++] = "Mohawk";
    %choice[%high++] = "Mullet";
    %choice[%high++] = "Neat";
    %choice[%high++] = "Slick";
    %choice[%high++] = "";
    %choice[%high++] = "";
    %choice[%high++] = "";
  }

  return %choice[getRandom(%high)] @ "Hair";
}

function getRandomHatName(%gender)
{
  %high = -1;

  %choice[%high++] = "Bandit";
  %choice[%high++] = "Bearskin";
  %choice[%high++] = "Beret";
  %choice[%high++] = "Bowler";
  %choice[%high++] = "Dealer";
  %choice[%high++] = "Disguise";
  %choice[%high++] = "Fisherman";
  %choice[%high++] = "Goggles";
  %choice[%high++] = "GoldenTophat";
  %choice[%high++] = "Hardhat";
  %choice[%high++] = "Headband";
  %choice[%high++] = "KnitHat";
  %choice[%high++] = "MBison";
  %choice[%high++] = "Naptime";
  %choice[%high++] = "Navy";
  %choice[%high++] = "Pilgrim";
  %choice[%high++] = "Ragtime";
  %choice[%high++] = "RobinHood";
  %choice[%high++] = "Safari";
  %choice[%high++] = "Scarf";
  %choice[%high++] = "Shako";
  %choice[%high++] = "Stacker";
  %choice[%high++] = "Straw";
  %choice[%high++] = "TopHat";
  %choice[%high++] = "Tossle";
  %choice[%high++] = "Turban";

  if (%gender)
  {
    %choice[%high++] = "Gentleman";
    %choice[%high++] = "Mayor";
  }

  return %choice[getRandom(%high)];
}

function loadMurderManorNames()
{
  %pattern = "Add-Ons/GameMode_Murder_Manor/data/names/*.txt";
  %fp = new FileObject();

  for (%file = findFirstFile(%pattern); %file !$= ""; %File = findNextFile(%pattern))
  {
    %name = fileBase(%file);

    $MurderManor::NameCount[%name] = 0;
    %fp.openForRead(%file);

    while (!%fp.isEOF())
    {
      $MurderManor::Name[%name, $MurderManor::NameCount[%name]] = %fp.readLine();
      $MurderManor::NameCount[%name]++;
    }
  }
}

function getRandomName(%name)
{
  return $MurderManor::Name[%name, getRandom(0, $MurderManor::NameCount[%name] - 1)];
}

package MurderManorCharacterPackage
{
    function GameConnection::applyBodyParts(%this)
    {
    %obj = %this.player;

    if (!isObject(%obj) || %this.miniGame != $DefaultMiniGame)
    {
      Parent::applyBodyParts(%this);
      return;
    }

    if (%obj.isCloaked)
    {
      return;
    }

    %obj.hideNode("ALL");

    %obj.unHideNode("headSkin");
    %obj.unHideNode(%this.charGender ? "chest" : "femChest");
    %obj.unHideNode("pants");
    %obj.unHideNode("larm");
    %obj.unHideNode("rarm");

    if (isHat(%this.charHat))
    {
      %obj.mountHat(%this.charHat);
    }
    else if (isHat(%this.charHair))
    {
      %obj.mountHat(%this.charHair);
    }
    else
    {
      // %obj.removeHat();
    }

    %obj.unHideNode(%this.murderHookLeft ? "lhook" : "lhand");
    %obj.unHideNode(%this.murderHookRight ? "rhook" : "rhand");

    %obj.unHideNode(%this.murderPegLeft ? "lpeg" : "lshoe");
    %obj.unHideNode(%this.murderPegRight ? "rpeg" : "rshoe");
  }

  function GameConnection::applyBodyColors(%this)
  {
    %obj = %this.player;

    if (!isObject(%obj) || %this.miniGame != $DefaultMiniGame)
    {
      Parent::applyBodyColors(%this);
      return;
    }

    %obj.setDecalName(%this.charDecal);
    %obj.setFaceName(%this.charFace);

    %obj.setNodeColor("headSkin", %this.charColor);

    //%obj.setNodeColor(%this.murderGender ? "femchest" : "chest", %suitColor);
    %obj.setNodeColor(%this.charGender ? "chest" : "femChest", %this.charColor);
    //%obj.setNodeColor("pants", %pantsColor);
    %obj.setNodeColor("pants", %this.charColor);

    //%obj.setNodeColor("larm", %sleeveColor);
    %obj.setNodeColor("larm", %this.charColor);
    //%obj.setNodeColor("rarm", %sleeveColor);
    %obj.setNodeColor("rarm", %this.charColor);

    if (%pack != 0)
    {
      %obj.setNodeColor($pack[%pack], %packColor);
    }

    if (%this.murderHookLeft) {
      %obj.setNodeColor("lhook", "0.392 0.196 0 1");
    }
    else {
      %obj.setNodeColor("lhand", %this.charColor);
    }

    if (%this.murderHookRight) {
      %obj.setNodeColor("rhook", "0.392 0.196 0 1");
    }
    else {
      %obj.setNodeColor("rhand", %this.charColor);
    }

    if (%this.murderPegLeft) {
      %obj.setNodeColor("lpeg", "0.392 0.196 0 1");
    }
    else {
      %obj.setNodeColor("lshoe", %this.charColor);
    }

    if (%this.murderPegRight) {
      %obj.setNodeColor("rpeg", "0.392 0.196 0 1");
    }
    else {
      %obj.setNodeColor("rshoe", %this.charColor);
    }
  }
};

activatePackage("MurderManorCharacterPackage");
loadMurderManorNames();
function MiniGameSO::endMurderManor(%this, %winner)
{
  if (isEventPending(%this.resetSchedule))
  {
    return;
  }

  %text = %winner ? "murderer wins" : "bystanders win";
  %this.messageAll('', "\c5The " @ %text @ "! A new game will begin in 15 sceonds.");

  if (%this.murdererName !$= "")
  {
    %this.messageAll('', "\c3" @ %this.murdererName @ " \c5was the murderer this round.");
  }

  %this.scheduleReset(15000);
}

function GameConnection::updateMurderStatus(%this)
{
  cancel(%this.updateMurderStatus);
  %miniGame = %this.miniGame;

  if (%miniGame != $DefaultMiniGame || isEventPending(%miniGame.resetSchedule))
  {
    commandToClient(%this, 'ClearBottomPrint');
    return;
  }

  %timeElapsed = getSimTime() - %miniGame.lastResetTime;
  %timeRemaining = getMax(%miniGame.timeLimit - (%timeElapsed / 1000), 0);

  %factor = getMin(1, %timeRemaining / %miniGame.timeLimit);

  if (%factor >= 0.99)
  {
    %color = blendRGBA("1 1 1 1", "1 0 1" SPC ((%factor - 0.99) / 0.01));
  }
  else if (%factor <= 0.1)
  {
    %blend = %this.isMurderer ? "1 0 0" : "0 1 0";
    %color = blendRGBA(%blend SPC "1", "1 1 1" SPC %factor / 0.1);
  }
  else
  {
    %color = "1 1 1 1";
  }

  %time = mCeil(%timeRemaining);
  %timer = "<color:" @ rgbToHex(vectorScale(%color, 255)) @ ">" @ getTimeString(%time);

  %text = "<just:center><font:lucida console:30>\c6" @ %timer @ "\n";
  %control = %this.getControlObject();

  if (%control.getType() & $TypeMasks::PlayerObjectType)
  {
    %target = %control.client;

    if (isObject(%target))
    {
      %text = %text @ "<font:palatino linotype:20>\c6" @ mFloor(%control.observerCount) @ " Spectators\n";
    }
  }
  else if (%control.getType() & $TypeMasks::CameraObjectType)
  {
    %target = %control.getOrbitObject().client;

    if (isObject(%target))
    {
      %text = %text @ "<font:palatino linotype:20>\c6Spectating\n";
    }
  }

  %color = "0 0 0 1";

  if (isObject(%target))
  {
    %name = %target.charName;

    if (%name !$= "")
    {
      %text = %text @ "<just:left><font:palatino linotype:28>\c3" @ %name @ " (" @ %target.getPlayerName() @ ")<just:right>";
      %text = %text @ %target.getRoleName() SPC "\c3(" @ %target.getGroupName() @ "\c3) \n";
    }

    %color = blendRGBA(%color, "1 0 0" SPC %target.player.getDamagePercent());
  }

  commandToClient(%this, 'SetVignette', 0, %color);

  %this.bottomPrint(%text, 1, 1);
  %this.updateMurderStatus = %this.schedule(125, "updateMurderStatus");
}

package MurderManorGamePackage
{
  // Reset when the first player joins.
  function MiniGameSO::addMember(%this, %member)
  {
    %empty = %this.numMembers < 1;
    Parent::addMember(%this, %member);

    if (%this == $DefaultMiniGame && %empty)
    {
      %member.clearMurderManorCharacter();
      %this.reset(0);
    }
  }

  // Clean up and start new round on reset.
  function MiniGameSO::reset(%this, %client)
  {
    if (%this != $DefaultMiniGame || %this.numMembers < 1)
    {
      Parent::reset(%this, %client);
      return;
    }

    // Play nice with the default rate limiting.
    if (getSimTime() - %this.lastResetTime < 5000)
    {
      return;
    }

    // Now, it's important that we do this before the reset. As soon as the
    // mini-game resets, all the players will spawn. If that happens before
    // they get their roles and the like, appearances (and possibly names)
    // will be messed up.
    for (%i = 0; %i < %this.numMembers; %i++)
    {
      //%this.member[%i].role = $Role::Bystander;
      %member = %this.member[%i];

      %member.setScore(%member.score + %member.karma, 1);
      %member.karma = 0;

      %member.role = getRandom(7);
      %member.isMurderer = 0;
    }

    // Assign roles
    // ...

    // Assign murderer
    for (%i = 0; %i < %this.numMembers; %i++)
    {
      %member = %this.member[%i];

      if (%member.role == $Role::Family)
      {
        continue;
      }

      if (%i)
      {
        %choices = %choices SPC %member;
      }
      else
      {
        %choices = %member;
      }
    }

    %murderer = getWord(%choices, getRandom(getWordCount(%choices) - 1));

    if (!isObject(%murderer))
    {
      messageAll('', "ERROR: Cannot pick murderer");
    }

    %murderer.isMurderer = 1;

    // Create characters!
    for (%i = 0; %i < %this.numMembers; %i++)
    {
      %this.member[%i].createMurderManorCharacter();
    }

    %this.murdererName = "";

    if (isObject(%murderer))
    {
      %this.murdererName = %murderer.getPlayerName() SPC "(" @ %murderer.charName @ ")";
    }

    // Respawn everyone.
    Parent::reset(%this, %client);

    // Random items!
    %name = "_randomItemSpawn";
    %choices = "MeleeCaneItem MeleeUmbrellaItem MeleeWrenchItem MeleePanItem";

    %count = BrickGroup_888888.NTObjectCount[%name];

    for (%i = 0; %i < %count; %i++)
    {
      %brick = BrickGroup_888888.NTObject[%name, %i];

      if (getRandom() >= 0.33)
      {
        %brick.setItem(getWord(%choices, getRandom(0, getWordCount(%choices) - 1)));
      }
      else
      {
        %brick.setItem(0);
      }
    }

    %nameCount = BrickGroup_888888.NTObjectCount["_OutsideBlocker"];
    %check = !$OutsideMode;

    for (%i = 0; %i < %nameCount; %i++)
    {
      %obj = BrickGroup_888888.NTObject["_OutsideBlocker", %i];

      %obj.setRendering(%check);
      %obj.setColliding(%check);
      %obj.setRayCasting(%check);
    }

    serverPlay2D(SPK_Alert_Sound);
  }

  // Custom message on timeout
  function MiniGameSO::timeLimitTick(%this, %echo)
  {
    cancel(%this.timeLimitTick);
    %this.timeLimitTick = 0;

    %timeElapsed = getSimTime() - %this.lastResetTime;
    %timeRemaining = (%this.timeLimit * 1000) - %timeElapsed;

    if (%timeRemaining <= 10)
    {
      %this.messageAll('', "\c6Time's up!");
      %this.endMurderManor(0);
    }
    else
    {
      Parent::timeLimitTick(%this, %echo);
    }
  }

  function MiniGameSO::checkLastManStanding(%this)
  {
    if (%this != $DefaultMiniGame)
    {
      return Parent::checkLastManStanding(%this);
    }

    if (%this.numMembers < 1 || isEventPending(%this.scheduleReset))
    {
      return 0;
    }

    %count[0] = 0;
    %count[1] = 0;

    for (%i = 0; %i < %this.numMembers; %i++)
    {
      %member = %this.member[%i];

      if (isObject(%member.player))
      {
        %count[%member.isMurderer]++;
      }
    }

    %end = "";

    if (!%count[0])
    {
      %end = 1;
      serverPlay2D(SPK_Alert2_Sound);
    }
    else if (!%count[1])
    {
      %end = 0;
      serverPlay2D(SPK_Alert3_Sound);
    }

    if (%end !$= "")
    {
      %this.endMurderManor(%end);
    }

    return 0;
  }

  function GameConnection::onDeath(%this, %obj, %client, %type, %area)
  {
    %this.play2D(SPK_Event_Sound);

    if (%this.miniGame == $DefaultMiniGame && %client.miniGame == $DefaultMiniGame)
    {
      %this.player.lastDirectDamageType = 0;
      // %this.lastDamageType = 0;
      %type = 0;

      %count = ClientGroup.getCount();

      for (%i = 0; %i < %this.miniGame.numMembers; %i++)
      {
        %member = %this.miniGame.member[%i];

        if (%this != %member)
        {
          // %member.play2D(MurderBassSound);
        }
      }

      if (isObject(%client) && %client != %this && !%client.isMurderer)
      {
        %client.setScore(%client.score + (%this.isMurderer ? 3 : -1), 1);
      }
    }

    Parent::onDeath(%this, %obj, %client, %type, %area);
  }

  function GameConnection::setScore(%this, %score, %karma)
  {
    if (%karma)
    {
      Parent::setScore(%this, %score);
    }
  }
};

activatePackage("MurderManorGamePackage");
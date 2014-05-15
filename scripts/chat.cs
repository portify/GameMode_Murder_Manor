package MurderManorChatPackage
{
  function serverCmdMessageSent(%client, %message)
  {
    if (%client.miniGame != $DefaultMiniGame)
    {
      Parent::serverCmdMessageSent(%client, %message);
      return;
    }

    if (getSimTime() - %client.lastChatTime <= 500)
    {
      return;
    }

    %message = trim(stripMLControlChars(%message));

    if (%message $= "")
    {
      return;
    }

    echo(%client.getPlayerName() SPC "(" @ %client.charName @ "): " @ %message);
    %client.lastChatTime = getSimTime();

    if (%client.miniGame == $DefaultMiniGame && isObject(%client.player))
    {
      %client.player.playThread(0, "talk");
      %client.player.schedule(strLen(%message) * 35, "playThread", 0, "root");

      %tag = '\c3%1\c6 says, \'%2\'';
    }
    else
    {
      %tag = '\c7[Dead] \c3%1\c6: %2';
      %dead = 1;
    }

    %count = ClientGroup.getCount();

    for (%i = 0; %i < %count; %i++)
    {
      %other = ClientGroup.getObject(%i);

      %empty = 0;
      %name = %client.charName;

      if (%name $= "")
      {
        %empty = 1;
        %name = %client.getPlayerName();
      }

      if (%other.miniGame == $DefaultMiniGame && !isEventPending(%other.miniGame.resetSchedule) && isObject(%other.player))
      {
        if (%dead)
        {
          continue;
        }

        // range checks
        // continue;
      }
      else if (!%empty)
      {
        %name = %client.getPlayerName() SPC "\c7(" @ %name @ ")";
      }

      messageClient(%other, '', %tag, %name, %message);

      //for (%j = 0; %j < 16; %j++) // ????
      //  %other.play2D(MurderTalkSound);
    }
  }

  function serverCmdTeamMessageSent(%client, %message)
  {
    if (%client.miniGame != $DefaultMiniGame)
    {
      Parent::serverCmdTeamMessageSent(%client, %message);
    }
  }

  function serverCmdStartTalking(%client)
  {
    if (%client.miniGame != $DefaultMiniGame)
    {
      Parent::serverCmdStartTalking(%client);
    }
  }
};

activatePackage("MurderManorChatPackage");
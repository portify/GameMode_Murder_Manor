package MurderManorGamePackage
{
  // Reset when the first player joins.
  function MiniGameSO::addMember(%this, %member)
  {
    %empty = %this.numMembers < 1;
    Parent::addMember(%this, %member);

    if (%this == $DefaultMiniGame && %empty)
    {
      %member.clearMurderCharacter();
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
      %this.member[%i].role = $Role::Bystander;
      %this.member[%i].isMurderer = 0;
    }

    // Assign special roles
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
      %this.member[%i].createMurderCharacter();
    }

    // Respawn everyone.
    Parent::reset(%this, %client);
  }
};

activatePackage("MurderManorPackage");
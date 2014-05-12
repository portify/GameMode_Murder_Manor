function GameConnection::clearMurderManorCharacter(%this)
{
  %this.charName = "";
  %this.charGender = "";
  %this.charColor = "";
  %this.charHair = "";

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

  %first = getRandom() < 0.375 ? %prefix : getRandomName("first");
  %last = getRandomName("last");

  %this.charName = %first SPC %last;

  %this.charColor = getRandomSkinColor();
  %this.charHair = getRandomHairName(%this.charGEnder);

  // ...
}

function GameConnection::appendMurderApparel(%this, %type, %fields)
{
  %this.charApparelType[%this.charApparelCount] = %type;
  %this.charApparelFields[%this.charApparelCount] = %fields;

  %this.charApparelCount++;
}
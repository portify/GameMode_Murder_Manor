datablock ShapeBaseImageData(CloakImage)
{
	shapeFile = "base/data/shapes/empty.dts";
	item = CloakItem;
};

datablock ItemData(CloakItem)
{
	image = CloakImage;
	shapeFile = "base/data/shapes/printGun.dts";

	uiName = "Cloak";
	canDrop = false;

	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
};

function CloakImage::onMount(%this, %obj, %slot)
{
	%obj.playThread(1, "root");

	if (!isObject(%obj.client))
	{
		return;
	}

	// hl2AmmoCheck(%this, %obj, %slot);
	// %obj.player.playAudio(0, CloakLoopSound);

	if (!isEventPending(%obj.cloakUpdateTick))
	{
		// %obj.cloakUpdateTick = %obj.schedule(500, "cloakUpdateTick", %slot);
		%obj.cloakUpdateTick(%slot);
	}
}

function CloakImage::onUnMount(%this, %obj, %slot)
{
	// cancel(%obj.cloakUpdateTick);

	if (isObject(%obj.client))
	{
		commandToClient(%obj.client, 'ClearCenterPrint');
	}
}

function Player::cloakUpdateTick(%this, %slot)
{
	cancel(%this.cloakUpdateTick);
	// %shouldCloak = vectorLen(%this.getVelocity()) < 0.01;
	%shouldCloak = 1;

	if (%this.getMountedImage(0) != nameToID("CloakImage"))
	{
		if (%this.isCloaked)
		{
			%shouldCloak = false;
		}
		else
		{
			return;
		}
	}
	else if (%this.cloakUsage >= 1000)
	{
		if (%this.isCloaked)
		{
			%shouldCloak = 0;
		}
		else
		{
			return;
		}
	}

	%wasCloaked = %this.isCloaked;
	%this.isCloaked = %shouldCloak;

	if (%shouldCloak && !%wasCloaked)
	{
		%this.setCloaked(1);
		%this.startFade(0, 0, 1);

		%this.cloakSchedule1 = %this.schedule(500, "hideNode", "ALL");
		%this.cloakSchedule2 = %this.schedule(500, "removeHat");

		%this.playAudio(2, MurderCloakSound);
	}

	if (!%shouldCloak && %wasCloaked)
	{
		cancel(%this.cloakSchedule1);
		cancel(%this.cloakSchedule2);

		%this.setCloaked(0);
		%this.startFade(0, 500, 0);

		%this.client.applyBodyParts();
		%this.client.applyBodyColors();

		%this.playAudio(2, MurderUnCloakSound);
	}

	//if (%shouldCloak && %this.tool[%this.currTool] == nameToID("CloakItem") && %this.toolMag[%this.currTool])
	if (%shouldCloak)
	{
		%this.cloakUsage++;

		if (isObject(%this.client))
		{
			%bars = mFloatLength(50 * mClampF(1 - (%this.cloakUsage / 1000), 0, 1), 0);
			%message = "<just:center><font:impact:24><color:66FF66>\n\n\n\n";

			for (%i = 0; %i < %bars; %i++)
			{
				%message = %message @ "|";
			}

			%message = %message @ "<color:666666>";

			for (%i = 0; %i < 50 - %bars; %i++)
			{
				%message = %message @ "|";
			}

			%this.client.centerPrint(%message @ "\n", 0.1, 1);
		}
	}

	%this.cloakUpdateTick = %this.schedule(50, cloakUpdateTick, %slot);
}
//////////
// item //
//////////
datablock ItemData(murderKnifeItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./bknife2.dts";
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Murderer Knife";
	iconName = "./knife";
	doColorShift = false;
	colorShiftColor = "0.400 0.196 0 1.000";

	 // Dynamic properties defined by the scripts
	image = murderKnifeImage;
	canDrop = false;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(murderKnifeImage)
{
   // Basic Item properties
   shapeFile = "./bknife2.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0 0";
   //eyeOffset = "0.1 0.2 -0.55";

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = true;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   // Projectile && Ammo.
   item = murderKnifeItem;
   ammo = " ";
   projectile = swordProjectile;
   projectileType = Projectile;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = false;

   //casing = " ";
   doColorShift = true;
   colorShiftColor = "0.400 0.196 0 1.000";

   //raycastWeaponRange = 2;
   raycastWeaponRange = 3;
   raycastWeaponTargets = $TypeMasks::FxBrickObjectType |	//Targets the weapon can hit: Raycasting Bricks
   				$TypeMasks::PlayerObjectType |	//AI/Players
   				$TypeMasks::StaticObjectType |	//Static Shapes
   				$TypeMasks::TerrainObjectType |	//Terrain
   				$TypeMasks::VehicleObjectType;	//Vehicles
   raycastExplosionProjectile = swordProjectile;
   raycastExplosionSound = "";
   raycastDirectDamage = 35;
   raycastDirectDamageType = $DamageType::sword;

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
	stateName[0]					= "Activate";
	stateTimeoutValue[0]			= 0.45;
	stateTransitionOnTimeout[0]		= "Ready";
	stateSequence[0]				= "ready";
	// stateSound[0]					= swordDrawSound;

	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1] = "PreFire";
	stateAllowImageChange[1]		= true;
	stateSequence[1]				= "Ready";
	stateScript[1]					= "Ready";

	stateName[2]					= "PreFire";
	stateScript[2]					= "onPreFire";
	stateTimeoutValue[2]			= 0.2;
	stateTransitionOnTimeout[2]		= "Fire";
	stateAllowImageChange[2]		= false;

	stateName[3]					= "Fire";
	stateTransitionOnTimeout[3]		= "StopFire";
	stateTimeoutValue[3]			= 0.3;
	stateFire[3]					= true;
	stateAllowImageChange[3]		= false;
	stateSequence[3]				= "Fire";
	stateScript[3]					= "onFire";
	stateWaitForTimeout[3]			= true;

	stateName[4]					= "StopFire";
	stateSequence[4]				= "StopFire";
	stateScript[4]					= "onStopFire";
	stateTransitionOnTriggerUp[4]	= "Ready";
	stateSequence[4]				= "Ready";
	stateTimeoutValue[4]			= 0.3;
};

function murderKnifeImage::onPreFire(%this, %obj, %slot)
{
	%obj.playThread(0, "armReady");
}

function murderKnifeImage::onStopFire(%this, %obj, %slot)
{
  %obj.playThread(0, "root");
}

function murderKnifeImage::onFire(%this, %obj, %slot)
{
	%obj.playthread(2, spearThrow);
	Parent::onFire(%this, %obj, %slot);
}

function murderKnifeImage::onRaycastDamage(%this,%obj,%slot,%col,%pos,%normal,%shotVec,%crit)
{
	if(vectorDot(%obj.getForwardVector(),%col.getForwardVector()) > 0)
	{
		%col.kill();
		return;
	}
	parent::onRaycastDamage(%this,%obj,%slot,%col,%pos,%normal,%shotVec,%crit);
}
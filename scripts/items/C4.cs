datablock ItemData(C4Item)
{
  category = "Weapon";
  className = "Weapon";
  
  shapeFile = "Add-Ons/GameMode_Murder_Manor/resources/shapes/HoldC4n.dts";

  mass = 1;
  density = 0.2;
  elasticity = 0.2;
  friction = 0.6;
  emap = true;

  image = C4Image;

  uiName = "C4 Explosive";
  iconName = "Add-Ons/GameMode_Murder_Manor/resources/icons/icon_C4";
};

datablock ShapeBaseImageData(C4Image)
{
  shapeFile = "Add-Ons/GameMode_Murder_Manor/resources/shapes/HoldC4n.dts";
  mountPoint = 0;
  emap = 1;
  item = C4Item;
};

function C4Image::onMount(%this, %obj, %slot)
{
  %obj.playThread(0, "armReadyBoth");

  if (isObject(%obj.client))
  {
    %obj.client.centerPrint("\c6Left click to plant with a 15 second fuse.\n\c6Right click to plant with a 30 second fuse.", 3);
  }
}

function C4Image::onUnMount(%this, %obj, %slot)
{
  %obj.playThread(0, "root");

  if (isObject(%obj.client))
  {
    commandToClient(%obj.client, 'ClearCenterPrint');
  }
}

datablock StaticShapeData(C4Shape)
{
  shapeFile = "Add-Ons/GameMode_Murder_Manor/resources/shapes/C4e.dts";
};

datablock ParticleData(C4ExplosionParticle)
{
  dragCoefficient  = 3.5;
  windCoefficient  = 3.5;
  gravityCoefficient  = -1;
  inheritedVelFactor  = 0.0;
  constantAcceleration  = 0.0;
  //lifetimeMS  = 800;
  //lifetimeMS = 1100;
  lifetimeMS = 800;
  lifetimeVarianceMS  = 00;
  spinSpeed  = 25.0;
  spinRandomMin  = -25.0;
  spinRandomMax  = 25.0;
  useInvAlpha  = false;
  animateTexture  = false;

  textureName  = "base/data/particles/cloud";

  colors[0]   = "1 1 1 0.1";
  colors[1]   = "0.9 0.5 0.0 0.3";
  colors[2]   = "0.1 0.05 0.025 0.1";
  colors[3]   = "0.1 0.05 0.025 0.0";

  sizes[0]  = 4.0;
  sizes[1]  = 6.3;
  sizes[2] = 6.5;
  sizes[3] = 4.5;
  //sizes[0] = 2.25;
  //sizes[1] = 4.3;
  //sizes[2] = 4.5;
  //sizes[3] = 2.75;

  times[0]  = 0.0;
  times[1]  = 0.1;
  times[2] = 0.8;
  times[3] = 1.0;
};

datablock ParticleEmitterData(C4ExplosionEmitter)
{
  ejectionPeriodMS = 1;
  periodVarianceMS = 0;
  lifeTimeMS   = 21;
  //ejectionVelocity = 4;
  ejectionVelocity = 8;
  velocityVariance = 3.0;
  ejectionOffset  = 2.0;
  thetaMin        = 00;
  thetaMax        = 180;
  phiReferenceVel  = 0;
  phiVariance     = 360;
  overrideAdvance = false;
  particles = C4ExplosionParticle;
};

datablock ParticleData(C4ExplosionPointParticle)
{
  //dragCoefficient   = 6;
  dragCoefficient = 1;
  gravityCoefficient  = 0.5;
  inheritedVelFactor  = 0.2;
  constantAcceleration = 0.0;
  //lifetimeMS        = 1000;
  lifetimeMS = 350;
  textureName       = "base/data/particles/dot";
  spinSpeed  = 10.0;
  spinRandomMin  = -500.0;
  spinRandomMax  = 500.0;
  colors[0]  = "1 1 0 1";
  colors[1]  = "1 1 0 0";
  //sizes[0]   = 8;
  //sizes[1]   = 13;
  sizes[0] = 0.2;
  sizes[1] = 0.2;

  useInvAlpha = false;
};

datablock ParticleEmitterData(C4ExplosionPointEmitter)
{
  lifeTimeMS = 100;

  //ejectionPeriodMS = 1;
  ejectionPeriodMS = 5;
  periodVarianceMS = 0;
  //ejectionVelocity = 60;
  //ejectionVelocity = 20;
  ejectionVelocity = 30;
  velocityVariance = 0.0;
  ejectionOffset  = 0;
  thetaMin      = 0;
  thetaMax      = 180;
  phiReferenceVel  = 0;
  phiVariance   = 360;
  overrideAdvance = false;
  particles = C4ExplosionPointParticle;
};

datablock ParticleData(C4ExplosionChunkParticle)
{
  //dragCoefficient   = 6;
  dragCoefficient = 2;
  gravityCoefficient  = 1;
  inheritedVelFactor  = 0.05;
  constantAcceleration = 0.0;
  lifetimeMS        = 2000;
  lifetimeVarianceMS  = 500;
  textureName       = "base/data/particles/chunk";
  spinSpeed  = 10.0;
  spinRandomMin  = -500.0;
  spinRandomMax  = 500.0;
  colors[0]  = "0.545 0.27 0.07 1";
  colors[1]  = "0.545 0.27 0.07 0";
  //sizes[0]   = 8;
  //sizes[1]   = 13;
  sizes[0] = 0.4;
  sizes[1] = 0.4;

  useInvAlpha = false;
};

datablock ParticleEmitterData(C4ExplosionChunkEmitter)
{
  lifeTimeMS = 100;

  ejectionPeriodMS = 20;
  periodVarianceMS = 0;
  ejectionVelocity = 20;
  velocityVariance = 0.0;
  ejectionOffset  = 0;
  thetaMin      = 0;
  thetaMax      = 45;
  phiReferenceVel  = 0;
  phiVariance   = 360;
  overrideAdvance = false;
  particles = C4ExplosionChunkParticle;
};

datablock ParticleData(C4ExplosionSmokeParticle)
{
  //dragCoefficient   = 6;
  dragCoefficient = 2;
  gravityCoefficient  = -0.25;
  inheritedVelFactor  = 0.05;
  constantAcceleration = 0.0;
  //lifetimeMS        = 2500;
  lifetimeMS = 1250;
  lifetimeVarianceMS  = 250;
  textureName       = "base/data/particles/cloud";

  spinSpeed  = 10;
  spinRandomMin  = -50;
  spinRandomMax  = 50;

  colors[0] = "0.5 0.5 0.5 0";
  colors[1] = "0.7 0.7 0.7 0.06";
  colors[2] = "0.5 0.5 0.5 0";

  //sizes[0] = 7;
  //sizes[1] = 8;
  //sizes[2] = 9;
  sizes[0] = 4;
  sizes[1] = 6;
  sizes[2] = 8;

  times[0] = 0;
  times[1] = 0.25;
  times[2] = 1;

  useInvAlpha = true;
};

datablock ParticleEmitterData(C4ExplosionSmokeEmitter)
{
  lifeTimeMS = 100;

  ejectionPeriodMS = 2;
  periodVarianceMS = 0;
  ejectionVelocity = 5;
  velocityVariance = 0.0;
  ejectionOffset  = 0;
  thetaMin      = 0;
  thetaMax      = 180;
  phiReferenceVel  = 0;
  phiVariance   = 360;
  overrideAdvance = false;
  particles = C4ExplosionSmokeParticle;
};

datablock ExplosionData(C4Explosion)
{
  //lifeTimeMS = 500;
  //lifeTimeMS = 150;

  emitter[0] = C4ExplosionEmitter;
  emitter[1] = C4ExplosionPointEmitter;
  emitter[2] = C4ExplosionChunkEmitter;
  emitter[3] = C4ExplosionSmokeEmitter;

  faceViewer  = true;
  explosionScale = "1 1 1";

  shakeCamera = true;
  camShakeFreq = "10.0 11.0 10.0";
  camShakeAmp = "3.0 10.0 3.0";
  camShakeDuration = 0.5;
  camShakeRadius = 20.0;

  // Dynamic light
  lightStartRadius = 10;
  lightEndRadius = 25;
  lightStartColor = "1 1 1 1";
  lightEndColor = "0 0 0 1";

  damageRadius = 5;
  radiusDamage = 110;

  impulseRadius = 8;
  impulseForce = 2500;
};

addDamageType("C4Damage", '<bitmap:Add-Ons/GameMode_Murder_Manor/resources/icons/CI_C4> %1', '%2 <bitmap:Add-Ons/GameMode_Murder_Manor/resources/icons/CI_C4> %1', 1, 0);

datablock ProjectileData(C4Projectile : RocketLauncherProjectile)
{
  radiusDamageType = $DamageType::C4Damage;
  explosion = C4Explosion;
  uiName = "";
};

function StaticShape::tickC4(%this, %seconds)
{
  cancel(%this.tickC4);
  %this.setShapeName(%seconds);

  if (%seconds <= 0)
  {
    serverPlay3D(MurderArmBombSound, %this.getPosition());
    %this.schedule(1000, "detonateC4");
    return;
  }

  if (%seconds <= 3)
  {
    serverPlay3D(MurderBeepSound, %this.getPosition());
  }
  else if (%seconds <= 10)
  {
    serverPlay3D(MurderBeep07Sound, %this.getPosition());
  }

  %this.tickC4 = %this.schedule(1000, "tickC4", %seconds - 1);
}

function StaticShape::detonateC4(%this)
{
  serverPlay3D(MurderC4Explode1Sound, %this.getPosition());

  %obj = new Projectile()
  {
    dataBlock = C4Projectile;
    scale = "3 3 3";

    initialPosition = %this.getPosition();
    initialVelocity = "0 0 0";

    client = %this.client;
    sourceObject = %this.sourceObject;
  };

  MissionCleanup.add(%obj);

  %obj.explode();
  %this.delete();
}

package C4Package
{
  function Armor::onTrigger(%this, %obj, %slot, %state)
  {
    if (!%state || (%slot != 0 && %slot != 4))
    {
      Parent::onTrigger(%this, %obj, %slot, %state);
      return;
    }

    if (%obj.getMountedImage(0) != nameToID("C4Image") || %obj.tool[%obj.currTool] != nameToID("C4Item"))
    {
      Parent::onTrigger(%this, %obj, %slot, %state);
      return;
    }

    %ray = containerRayCast(
      %obj.getEyePoint(),
      vectorAdd(%obj.getEyePoint(), vectorScale(%obj.getEyeVector(), 4)),
      $TypeMasks::FxBrickObjectType
    );

    if (!%ray)
    {
      return;
    }

    %obj.tool[%obj.currTool] = 0;

    if (isObject(%obj.client))
    {
      messageClient(%obj.client, 'MsgItemPickup', '', %obj.currTool, 0);
      serverCmdUnUseTool(%obj.client);
    }
    else
    {
      %obj.unMountImage(0);
      %obj.currTool = "";
    }

    %ent = new StaticShape()
    {
      dataBlock = C4Shape;
      client = %obj.client;
      sourceObject = %obj;
    };

    MissionCleanup.add(%ent);

    %pos = getWords(%ray, 1, 3);
    %vec = getWords(%ray, 4, 6);

    %point = vectorAdd(%pos, vectorScale(%vec, 0.2));

    %ent.setTransform(%point SPC "0 0 0 1");
    //%ent.tickC4(%slot == 0 ? 30 : 60);
    %ent.tickC4(%slot == 0 ? 15 : 30);

          // %Hit = getwords(%wall,1,3);

          // %angle = getword(axisToEuler(getWords(%obj.getTransform(),3,6)),2);
          // %angle = mFloor(%angle + 0.5);
          // if(%angle < 0)
          //   %angle = %angle + 360;
          // %direction = getword(roundAngle(%angle),1);
          // %dir = getword(roundAngle(%angle),0);
          // %trans = axisToEuler(getWords(%obj.getTransform(),3,6));
          // %trans = setword(%trans,2,%direction);
          // %trans = setword(%trans,0,90);
          
          // %client.c4.setTransform(%hit SPC eulerToAxis(%trans));
          // %found = ContainerRayCast(%client.c4.getPosition(),VectorAdd(%client.c4.getPosition(),"0 0 0.1"),($TypeMasks::FxBrickObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::VehicleObjectType));
          // if(isObject(%found))
          // {
          //   %trans = axisToEuler(getWords(%obj.getTransform(),3,6));
          //   %trans = setword(%trans,1,180);
          //   %trans = VectorSub(%hit,"0 0 0.15") SPC eulerToAxis(%trans);
          //   %client.c4.setTransform(%trans);
          //   return;
          // }
          // %found = ContainerRayCast(VectorAdd(%client.c4.getPosition(),"0 0 0.2"),VectorSub(%client.c4.getPosition(),"0 0 0.1"),($TypeMasks::FxBrickObjectType | $TypeMasks::InteriorObjectType | $TypeMasks::VehicleObjectType));
          // if(isObject(%found))
          // {
          //   %client.c4.setTransform(VectorAdd(%hit,"0 0 0.15") SPC getwords(%obj.getTransform(),3,6));
          //   return;
          // }
          // if(%dir $= north)
          //   %client.c4.setTransform(VectorSub(%client.c4.getTransform(),"0 0.15"));
          // if(%dir $= south)
          //   %client.c4.setTransform(VectorAdd(%client.c4.getTransform(),"0 0.15"));
          // if(%dir $= east)
          //   %client.c4.setTransform(VectorSub(%client.c4.getTransform(),"0.15"));
          // if(%dir $= west)
          //   %client.c4.setTransform(VectorAdd(%client.c4.getTransform(),"0.15"));
  }
};

activatePackage("C4Package");

function roundAngle(%angle)
{
  if(%angle == 360)
    return "North 0";
  if(%angle >= 0 && %angle < 45)
    return "North 0";
  if(%angle >= 45 && %angle < 135)
    return "West 90";
  if(%angle >= 135 && %angle < 225)
    return "South 180";
  if(%angle >= 225 && %angle < 315)
    return "East 270";
  if(%angle >= 315 && %angle < 360)
    return "North 0";
  else
    return false;
}
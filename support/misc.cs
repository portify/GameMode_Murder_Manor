function rgbToHsl(%rgb)
{
	%r = getWord(%rgb, 0);
	%g = getWord(%rgb, 1);
	%b = getWord(%rgb, 2);

	%max = max3(%r, %g, %b);
	%min = min3(%r, %g, %b);

	%lightness = (%max + %min) / 2;

	if (%max == %min)
	{
		return "0 0" SPC %lightness;
	}

	%delta = %max - %min;
	%saturation = %lightness > 0.5 ? %delta / (2 - %max - %min) : %delta / (%max + %min);

	switch (%max)
	{
		case %r: %hue = (%g - %b) / %delta + (%g < %b ? 6 : 0);
		case %g: %hue = (%b - %r) / %delta + 2;
		case %g: %hue = (%r - %g) / %delta + 4;
	}

	return %hue / 6 SPC %saturation SPC %lightness;
}

function blendRGBA(%bg, %fg) {
	%ba = getWord(%bg, 3);
	%fa = getWord(%fg, 3);

	%a = 1 - (1 - %fa) * (1 - %ba);
	%r = getWord(%fg, 0) * %fa / %a + getWord(%bg, 0) * %ba * (1 - %fa) / %a;
	%g = getWord(%fg, 1) * %fa / %a + getWord(%bg, 1) * %ba * (1 - %fa) / %a;
	%b = getWord(%fg, 2) * %fa / %a + getWord(%bg, 0) * %ba * (1 - %fa) / %a;

	return %r SPC %g SPC %b SPC %a;
}

function rgbToHex(%rgb)
{
	return
		rgbPartToHex(getWord(%rgb, 0)) @
		rgbPartToHex(getWord(%rgb, 1)) @
		rgbPartToHex(getWord(%rgb, 2))
	;
}

function rgbPartToHex(%color)
{
	%hex = "0123456789ABCDEF";

	%left = mFloor(%color / 16);
	%color -= %left * 16;

	return getSubStr(%hex, %left, 1) @ getSubStr(%hex, %color, 1);
}

function median(%a, %b, %c) {
	if ((%a >= %b && %a <= %b) || (%a <= %b && %a >= %b)) return %a;
	if ((%b >= %a && %b <= %c) || (%b <= %a && %b >= %c)) return %b;
	if ((%c >= %a && %c <= %b) || (%c <= %a && %c >= %b)) return %c;
}

function min(%a, %b) {
	return %a < %b ? %a : %b;
}

function min3(%a, %b, %c)
{
	return %a < %b ? (%a < %c ? %a : %c) : (%b < %c ? %b : %c);
}

function max(%a, %b, %c) {
	return %a > %b ? %a : %b;
}

function max3(%a, %b, %c)
{
	return %a > %b ? (%a > %c ? %a : %c) : (%b > %c ? %b : %c);
}

function naturalGrammarList(%list) {
	%fields = getFieldCount(%list);

	if (%fields < 2) {
		return %list;
	}

	for (%i = 0; %i < %fields - 1; %i++) {
		%partial = %partial @ (%i ? ", " : "") @ getField(%list, %i);
	}

	return %partial SPC "and" SPC getField(%list, %fields - 1);
}

function vectorSpread(%vector, %spread) {
	%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
	%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
	%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;

	%mat = matrixCreateFromEuler(%x SPC %y SPC %z);
	return vectorNormalize(matrixMulVector(%mat, %vector));
}

function blendRGBA(%bg, %fg) {
	%ba = getWord(%bg, 3);
	%fa = getWord(%fg, 3);

	%a = 1 - (1 - %fa) * (1 - %ba);
	%r = getWord(%fg, 0) * %fa / %a + getWord(%bg, 0) * %ba * (1 - %fa) / %a;
	%g = getWord(%fg, 1) * %fa / %a + getWord(%bg, 1) * %ba * (1 - %fa) / %a;
	%b = getWord(%fg, 2) * %fa / %a + getWord(%bg, 0) * %ba * (1 - %fa) / %a;

	return %r SPC %g SPC %b SPC %a;
}
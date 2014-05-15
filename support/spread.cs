function spreadVector(%vector, %spread)
{
	%scalars = randomScalar() SPC randomScalar() SPC randomScalar();
	%scalars = vectorScale(%scalars, mDegToRad(%spread));

	return matrixMulVector(matrixCreateFromEuler(%scalars), %vector);
}

function randomScalar()
{
	return getRandom() * 2 - 1;
}
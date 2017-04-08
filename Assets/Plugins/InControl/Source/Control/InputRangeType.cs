namespace InControl
{
	public enum InputRangeType : int
	{
		None = 0,
		MinusOneToOne,
		OneToMinusOne,
		ZeroToOne,
		ZeroToMinusOne,
		OneToZero,
		MinusOneToZero,

		// TODO: These should be deprecated when custom profiles are.
		ZeroToNegativeInfinity,
		ZeroToPositiveInfinity,
		Everything
	}
}

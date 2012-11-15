using System;

namespace YAMP
{
    public interface IHasIndex
    {
        int Length { get; }
        int[] Dimensions { get; }

        Value Get(IIsIndex index);
		void Set(IIsIndex index, Value value);

		IHasIndex Create(int[] _dimensions);
	}
}

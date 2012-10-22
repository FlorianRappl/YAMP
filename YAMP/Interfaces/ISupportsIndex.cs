using System;

namespace YAMP
{
    public interface ISupportsIndex
    {
        int Length { get; }
        int Dimensions { get; }
        int GetDimension(int dimension);

        Value Get(int[] indices);
        void Set(int[] indices, Value value);

        ISupportsIndex Create(int[] dimensions);
    }
}

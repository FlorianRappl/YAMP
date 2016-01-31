namespace YAMP.Physics
{
    using System;

    /// <summary>
    /// Used for conversions from one unit to another.
    /// </summary>
    class ConversionUnit : PhysicalUnit
    {
        #region Fields

        readonly PhysicalUnit _from;

        #endregion

        #region ctor

        public ConversionUnit(String name, PhysicalUnit from)
        {
            Unit = name;
            _from = from;
        }

        #endregion

        #region Methods

        protected override PhysicalUnit Create()
        {
            return new ConversionUnit(Unit, _from);
        }

        public override Boolean HasConversion(String target)
        {
            if (_from.CanBe(target))
            {
                return true;
            }

            return _from.HasConversion(target);
        }

        public override Func<Double, Double> GetConversion(String unit)
        {
            //Example: Conversion from yd -> ft
            //Get transformation from yd -> m
            var backTransformation = _from.GetInverseConversion(Unit);

            //In case we want just yd to m
            if (_from.Unit == unit)
            {
                return backTransformation;
            }

            //Get transformation from m -> ft
            var newTransformation = _from.GetConversion(unit);
            //Apply toFt(toM(yd))
            return x => newTransformation(backTransformation(x));
        }

        public override Func<Double, Double> GetInverseConversion(String unit)
        {
            //Example: Conversion from 1/yd -> 1/ft
            //Get transformation from 1/yd -> 1/m
            var backTransformation = _from.GetConversion(Unit);

            //In case we want just 1/yd to 1/m
            if (_from.Unit == unit)
            {
                return backTransformation;
            }

            //Get transformation from 1/m -> 1/ft
            var newTransformation = _from.GetInverseConversion(unit);
            //Apply toInverseFt(toInverseM(yd))
            return x => newTransformation(backTransformation(x));
        }

        #endregion
    }
}

using System;

namespace SummaryCreator.Basic
{
    /// <summary>
    /// Select between none, kilo, mega and giga.
    /// </summary>
    public enum Unit
    {
        None = 0,
        Kilo = 1,
        Mega = 2,
        Giga = 3
    }

    public static class UnitExtension
    {
        /// <summary>
        /// Get the short string form from the unit.
        /// </summary>
        /// <param name="unit">Get from this unit.</param>
        /// <returns>Return the short string form.</returns>
        public static string GetUnitValue(this Unit unit)
        {
            switch(unit)
            {
                case Unit.None:
                    return String.Empty;
                case Unit.Kilo:
                    return "k";
                case Unit.Mega:
                    return "M";
                case Unit.Giga:
                    return "G";
                default:
                    return String.Empty;
            }
        }
    }
}

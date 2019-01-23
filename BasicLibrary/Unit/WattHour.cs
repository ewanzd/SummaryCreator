using System;
using System.Globalization;

namespace BasicLibrary.Unit
{
    /// <summary>
    /// Can calculating with other WattHour, int and double. 
    /// </summary>
    public class WattHour
    {
        /// <summary>
        /// Full watt hour of this object.
        /// </summary>
        protected double watthour;

        /// <summary>
        /// Set watt hour with a specific unit.
        /// </summary>
        /// <param name="value">Value of watt hour in unit.</param>
        /// <param name="unit">The unit of the value.</param>
        public virtual void SetValue(double value, Unit unit)
        {
            if(unit == Unit.None)
                watthour = value;

            else if(unit == Unit.Kilo)
                watthour = value / 1000;

            else if(unit == Unit.Mega)
                watthour = value / 1000000;

            else if(unit == Unit.Giga)
                watthour = value / 1000000000;

            else
                throw new ArgumentException("Unbekannte Einheit");
        }

        /// <summary>
        /// Get and calculate the watt hour of this object in this unit.
        /// </summary>
        /// <param name="unit">The unit of the return.</param>
        /// <returns>Return the calculated watt hour.</returns>
        public virtual double GetValue(Unit unit)
        {
            if(unit == Unit.None)
                return watthour;

            else if(unit == Unit.Kilo)
                return watthour / 1000;

            else if(unit == Unit.Mega)
                return watthour / 1000000;

            else if(unit == Unit.Giga)
                return watthour / 1000000000;

            else
                throw new ArgumentException("Unbekannte Einheit");
        }

        public override string ToString()
        {
            return watthour.ToString();
        }

        public string ToString(Unit unit)
        {
            if(unit == Unit.None)
                return String.Format("{0}", Math.Round(GetValue(unit), 2));
            else
                return String.Format("{0:0.00}", Math.Round(GetValue(unit),2));
        }

        public static bool TryParse(string s, out WattHour wh)
        {
            wh = (WattHour)0;
            double value;
            if(!Double.TryParse(s, out value))
                return false;
            wh.watthour = value;
            return true;
        }

        public static WattHour operator +(WattHour wh1, WattHour wh2)
        {
            WattHour wh = new WattHour();
            wh = (WattHour)(wh1.watthour + wh2.watthour);
            return wh;
        }

        public static WattHour operator -(WattHour wh1, WattHour wh2)
        {
            WattHour wh = new WattHour();
            wh = (WattHour)(wh1.watthour - wh2.watthour);
            return wh;
        }

        public static WattHour operator *(WattHour wh1, WattHour wh2)
        {
            WattHour wh = new WattHour();
            wh = (WattHour)(wh1.watthour * wh2.watthour);
            return wh;
        }

        public static WattHour operator /(WattHour wh1, WattHour wh2)
        {
            WattHour wh = new WattHour();
            wh = (WattHour)(wh1.watthour / wh2.watthour);
            return wh;
        }

        public static implicit operator WattHour(int wattHour)
        {
            WattHour wh = new WattHour();
            wh.watthour = wattHour;
            return wh;
        }

        public static implicit operator WattHour(double wattHour)
        {
            WattHour wh = new WattHour();
            wh.watthour = wattHour;
            return wh;
        }

        public static implicit operator double(WattHour wattHour)
        {
            if(wattHour == null)
                return new WattHour();
            return wattHour.watthour;
        }

        public static implicit operator int(WattHour wattHour)
        {
            if(wattHour == null)
                return new WattHour();
            return Convert.ToInt32(wattHour.watthour);
        }
    }
}

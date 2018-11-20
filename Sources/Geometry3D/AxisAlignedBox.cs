#region Using directives
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Globalization;

using Sharp3D.Math.Core;
#endregion
namespace Sharp3D.Math.Geometry3D
{
    
    /// <summary>
    /// Represents an axis aligned box in 3D space.
    /// </summary>
    [Serializable]
    [TypeConverter(typeof(AxisALignedBoxConverter))]
    public class AxisAlignedBox : ICloneable
    {
        #region Private Fields
        private Vector3F _min;
        private Vector3F _max;
        #endregion
        
        #region Constructors

        public AxisAlignedBox(Vector3F min, Vector3F max)
        {
            _min = min;
            _max = max;
        }
        
        public AxisAlignedBox(AxisAlignedBox box)
        {
            _min = box.Min;
            _max = box.Max;
        }
        #endregion
        
        #region Public Properties

        public Vector3F Min
        {
            get { return _min; }
            set { _min = value; }
        }
        
        public Vector3F Max
        {
            get { return _max; }
            set { _max = value; }
        }
        #endregion
        
        #region ICloneable Members
        object ICloneable.Clone()
        {
            return new AxisAlignedBox(this);
        }
        
        public AxisAlignedBox Clone()
        {
            return new AxisAlignedBox(this);
        }
        #endregion
        
        #region Public Static Parse Methods
       
        public static AxisAlignedBox Parse(string s)
        {
            Regex r = new Regex(@"AxisAlignedBox\(Min=(?<min>\([^\)]*\)), Max=(?<max>\([^\)]*\))\)", RegexOptions.None);
            Match m = r.Match(s);
            if (m.Success)
            {
                return new AxisAlignedBox(
                    Vector3F.Parse(m.Result("${Min}")),
                    Vector3F.Parse(m.Result("${Max}"))
                );
            }
            else
            {
                throw new ParseException("Unsuccessful Match.");
            }
        }
        #endregion
        
        #region Public methods

        #endregion
        
        #region System.Object Overrides
        public override int GetHashCode()
        {
            return _min.GetHashCode() ^ _max.GetHashCode();
        }
        
        public override bool Equals(object obj)
        {
            if (obj is AxisAlignedBox)
            {
                AxisAlignedBox b = (AxisAlignedBox)obj;
                return
                    (_min == b.Min) && (_max == b.Max);
            }
            return false;
        }
        
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "OrientedBox(Min={0}, Max={1})",
                Min,Max);
        }
        #endregion
        
        #region Comparison Operators
        public static bool operator ==(AxisAlignedBox left, AxisAlignedBox right)
        {
            return ValueType.Equals(left, right);
        }
        
        public static bool operator !=(AxisAlignedBox left, AxisAlignedBox right)
        {
            return !ValueType.Equals(left, right);
        }
        #endregion
    }
    
    #region AxisAlignedBoxConverter class

    public class AxisALignedBoxConverter : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }
        
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;

            return base.CanConvertTo(context, destinationType);
        }
        
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if ((destinationType == typeof(string)) && (value is AxisAlignedBox))
            {
                AxisAlignedBox box = (AxisAlignedBox)value;
                return box.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
        
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                return AxisAlignedBox.Parse((string)value);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }

    
    #endregion
}
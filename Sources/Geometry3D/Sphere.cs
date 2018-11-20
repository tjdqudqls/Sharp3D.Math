#region Using directives
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Globalization;
using Sharp3D.Math.Core;

#endregion

namespace Sharp3D.Math.Geometry3D
{
    [Serializable]
    [TypeConverter(typeof(SphereConverter))]
    public class Sphere : ICloneable
    {
        #region Private Fields
        private Vector3F _c;
        private float _r;
        #endregion
        
        #region Constructors

        public Sphere(Vector3F center, float radius)
        {
            _c = center;
            _r = radius;
        }

        public Sphere(Sphere s)
        {
            _c = s.Center;
            _r = s.Radius;
        }
        #endregion
        
        #region Public Properties

        public Vector3F Center
        {
            get { return _c; }
            set { _c = value; }
        }

        public float Radius
        {
            get { return _r; }
            set { _r = value; }
        }
        
        #endregion
        
        #region ICloneable Members
        object ICloneable.Clone()
        {
            return new Sphere(this);
        }
        
        public Sphere Clone()
        {
            return new Sphere(this);
        }
        #endregion
        
        #region Public Static Parse Methods
        public static Sphere Parse(string s)
        {
            Regex r = new Regex(@"Sphere\(Center=(?<center>\([^\)]*\)), Radius=(?<radius>.*)\)", RegexOptions.None);
            Match m = r.Match(s);
            if (m.Success)
            {
                return new Sphere(
                    Vector3F.Parse(m.Result("${center}")),
                    float.Parse(m.Result("${radius}"))
                );
            }
            else
            {
                throw new ParseException("Unsuccessful Match.");
            }
        }
        #endregion
        
        #region Public Methods

        public float GetVolume()
        {
            return ((float) MathFunctions.PI) * _r * _r * _r * 4 / 3;
        }
        
        public float GetSurface()
        {
            return ((float) MathFunctions.PI) * 4 * _r * _r;
        }        
        #endregion
        
        #region System.Object Overrides
        public override int GetHashCode()
        {
            return _c.GetHashCode() ^ _r.GetHashCode();
        }
        
        public override bool Equals(object obj)
        {
            if (obj is Sphere)
            {
                Sphere s = (Sphere)obj;
                return
                    (_c == s.Center) && (_r == s.Radius);
            }
            return false;
        }
        
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Sphere(Center={0}, Radius={1})",
                Center, Radius);
        }
        #endregion
        
        #region Comparison Operators
        public static bool operator ==(Sphere left, Sphere right)
        {
            return ValueType.Equals(left, right);
        }
        
        public static bool operator !=(Sphere left, Sphere right)
        {
            return !ValueType.Equals(left, right);
        }
        #endregion
    }
    
    #region SphereConverter class

    public class SphereConverter : ExpandableObjectConverter
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
                SphereConverter sphere = (SphereConverter)value;
                return sphere.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
        
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                return Sphere.Parse((string)value);
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
    #endregion
}
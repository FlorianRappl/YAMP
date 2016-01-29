using System;
using YAMP.Converter;

namespace YAMP
{
    /// <summary>
    /// The abstract base class for plot annotations.
    /// </summary>
    public abstract class Annotation
    {
        #region Properties

        /// <summary>
        /// Gets or sets the text of the annotation.
        /// </summary>
        [StringToStringConverter]
        public string Text { get; set; }

        /// <summary>
        /// Gets the x coordinate of the annotation.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets the y coordinate of the annotation.
        /// </summary>
        public double Y { get; set; }

        #endregion

        #region Serialization

        internal virtual void Serialize(Serializer s)
        {
            s.Serialize(Text);
            s.Serialize(X);
            s.Serialize(Y);
        }

        internal virtual void Deserialize(Deserializer ds)
        {
            Text = ds.GetString();
            X = ds.GetDouble();
            Y = ds.GetDouble();
        }

        #endregion
    }
}

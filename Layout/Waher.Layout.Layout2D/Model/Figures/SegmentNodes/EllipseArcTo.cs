﻿using System;
using System.Xml;
using SkiaSharp;
using Waher.Layout.Layout2D.Model.Attributes;

namespace Waher.Layout.Layout2D.Model.Figures.SegmentNodes
{
	/// <summary>
	/// Draws a ellipse arc to a point, relative to the origio of the current container
	/// </summary>
	public class EllipseArcTo : Point, ISegment
	{
		private LengthAttribute radiusX;
		private LengthAttribute radiusY;
		private BooleanAttribute clockwise;

		/// <summary>
		/// Draws a ellipse arc to a point, relative to the origio of the current container
		/// </summary>
		/// <param name="Document">Layout document containing the element.</param>
		/// <param name="Parent">Parent element.</param>
		public EllipseArcTo(Layout2DDocument Document, ILayoutElement Parent)
			: base(Document, Parent)
		{
		}

		/// <summary>
		/// Local name of type of element.
		/// </summary>
		public override string LocalName => "EllipseArcTo";

		/// <summary>
		/// Populates the element (including children) with information from its XML definition.
		/// </summary>
		/// <param name="Input">XML definition.</param>
		public override void FromXml(XmlElement Input)
		{
			base.FromXml(Input);

			this.radiusX = new LengthAttribute(Input, "radiusX");
			this.radiusY = new LengthAttribute(Input, "radiusY");
			this.clockwise = new BooleanAttribute(Input, "clockwise");
		}

		/// <summary>
		/// Exports attributes to XML.
		/// </summary>
		/// <param name="Output">XML output.</param>
		public override void ExportAttributes(XmlWriter Output)
		{
			base.ExportAttributes(Output);

			this.radiusX.Export(Output);
			this.radiusY.Export(Output);
			this.clockwise.Export(Output);
		}

		/// <summary>
		/// Creates a new instance of the layout element.
		/// </summary>
		/// <param name="Document">Document containing the new element.</param>
		/// <param name="Parent">Parent element.</param>
		/// <returns>New instance.</returns>
		public override ILayoutElement Create(Layout2DDocument Document, ILayoutElement Parent)
		{
			return new EllipseArcTo(Document, Parent);
		}

		/// <summary>
		/// Copies contents (attributes and children) to the destination element.
		/// </summary>
		/// <param name="Destination">Destination element</param>
		public override void CopyContents(ILayoutElement Destination)
		{
			base.CopyContents(Destination);

			if (Destination is EllipseArcTo Dest)
			{
				Dest.radiusX = this.radiusX.CopyIfNotPreset();
				Dest.radiusY = this.radiusY.CopyIfNotPreset();
				Dest.clockwise = this.clockwise.CopyIfNotPreset();
			}
		}

		/// <summary>
		/// Measures layout entities and defines unassigned properties.
		/// </summary>
		/// <param name="State">Current drawing state.</param>
		/// <param name="PathState">Current path state.</param>
		public virtual void Measure(DrawingState State, PathState PathState)
		{
			if (this.radiusX.TryEvaluate(State.Session, out Length R))
				this.rX = State.GetDrawingSize(R, this, true);
			else
				this.defined = false;

			if (this.radiusY.TryEvaluate(State.Session, out R))
				this.rY = State.GetDrawingSize(R, this, false);
			else
				this.defined = false;

			if (!this.clockwise.TryEvaluate(State.Session, out this.clockDir))
				this.defined = false;

			if (this.defined)
				PathState.Set(this.xCoordinate, this.yCoordinate);
		}

		/// <summary>
		/// Measured radius along X-axis
		/// </summary>
		protected float rX;

		/// <summary>
		/// Measured radius along Y-axis
		/// </summary>
		protected float rY;

		/// <summary>
		/// Measured direction of arc
		/// </summary>
		protected bool clockDir;

		/// <summary>
		/// Draws layout entities.
		/// </summary>
		/// <param name="State">Current drawing state.</param>
		/// <param name="PathState">Current path state.</param>
		/// <param name="Path">Path being generated.</param>
		public virtual void Draw(DrawingState State, PathState PathState, SKPath Path)
		{
			if (this.defined)
			{
				PathState.Set(this.xCoordinate, this.yCoordinate);
				Path.ArcTo(this.rX, this.rY, 0, SKPathArcSize.Small,
					this.clockDir ? SKPathDirection.Clockwise : SKPathDirection.CounterClockwise,
					this.xCoordinate, this.yCoordinate);
			}
		}
	}
}
﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class GlParabola : GlCurve
{
	private float a;

	private float b;

	private const float FAULT = 0.02f;

	private float c;

	private GlPointR2 parabolaFocus;

	private GlLineR2 parabolaDirectriss;

	public virtual float A
	{
		get

		{
			throw new System.NotImplementedException();
		}
		set

		{
			throw new System.NotImplementedException();
		}
	}

	public virtual float B
	{
		get

		{
			throw new System.NotImplementedException();
		}
		private set

		{
			throw new System.NotImplementedException();
		}
	}

	public virtual float C
	{
		get

		{
			throw new System.NotImplementedException();
		}
		private set

		{
			throw new System.NotImplementedException();
		}
	}

	private GlPointR2 Focus
	{
		get

		{
			throw new System.NotImplementedException();
		}
		private set

		{
			throw new System.NotImplementedException();
		}
	}

	private GlLineR2 Directriss
	{
		get

		{
			throw new System.NotImplementedException();
		}
		private set

		{
			throw new System.NotImplementedException();
		}
	}

	public virtual GlPointR2 Vertex
	{
		get

		{
			throw new System.NotImplementedException();
		}
		private set

		{
			throw new System.NotImplementedException();
		}
	}

	public override GlRectangle RealBox
	{
		get

		{
			throw new System.NotImplementedException();
		}
		private set

		{
			throw new System.NotImplementedException();
		}
	}

	public GlParabola(float aCoeff, GlPointR2 vertex, GlVectorR2 directVector)
	{
	}

	public GlParabola(GlParabola copyParabola)
	{
	}

	public override GlLineR2 getTangentFromBelongs(GlPointR2 belongsPoint)
	{
		throw new System.NotImplementedException();
	}

	public override bool isPointInside(GlPointR2 P)
	{
		throw new System.NotImplementedException();
	}

	public override bool getFDiff(float X)
	{
		throw new System.NotImplementedException();
	}

	public override int getSDiff(float X)
	{
		throw new System.NotImplementedException();
	}

	protected override void updatePointsPosition()
	{
		throw new System.NotImplementedException();
	}

	public virtual GlFigure getScaled(float scale)
	{
		throw new System.NotImplementedException();
	}

	public virtual IEnumerable<GlPointR2> getIntersection(GlLineR2 L)
	{
		throw new System.NotImplementedException();
	}

	public virtual bool isPointBelongs(GlPointR2 P)
	{
		throw new System.NotImplementedException();
	}

	public virtual string ToString()
	{
		throw new System.NotImplementedException();
	}

	public virtual bool Equals(object obj)
	{
		throw new System.NotImplementedException();
	}

	public virtual bool isNullParabola()
	{
		throw new System.NotImplementedException();
	}

}

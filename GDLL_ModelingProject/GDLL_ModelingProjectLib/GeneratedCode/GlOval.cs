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

public class GlOval : GlCurve
{
	private const float FAULT = 0.001f;

	private float Ra;

	private float Rb;

	public virtual float Length
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

	public virtual float RadA
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

	public virtual float RadB
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

	public virtual IEnumerable<GlPointR2> Focuses
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

	public GlOval(float R1, float R2, GlVectorR2 directVector, GlPointR2 OvalCenter)
	{
	}

	public GlOval(GlOval copyOval)
	{
	}

	public override bool getFDiff(float X)
	{
		throw new System.NotImplementedException();
	}

	public override int getSDiff(float X)
	{
		throw new System.NotImplementedException();
	}

	public override GlLineR2 getTangentFromBelongs(GlPointR2 belongsPoint)
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

	public virtual bool isIntersects(LineR2 L)
	{
		throw new System.NotImplementedException();
	}

	public virtual IEnumerable<GlLineR2> getTangentFromPoint(GlPointR2 P)
	{
		throw new System.NotImplementedException();
	}

	public override bool isPointInside(GlPointR2 P)
	{
		throw new System.NotImplementedException();
	}

	public virtual bool isNullOval()
	{
		throw new System.NotImplementedException();
	}

	public virtual bool Equals(object obj)
	{
		throw new System.NotImplementedException();
	}

	public virtual string ToString()
	{
		throw new System.NotImplementedException();
	}

}


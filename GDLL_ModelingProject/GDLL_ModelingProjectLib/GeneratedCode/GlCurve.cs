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

public abstract class GlCurve : GlFigure
{
	protected GlPointR2 systemCenter;

	protected GlVectorR2 directVector;

	protected IEnumerable<GlPointR2> curvePoints;

	public override GlRectangle BOX
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

	public virtual GlRectangle RealBox
	{
		get;
		private set;
	}

	public override int CountOfPoints
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

	public override GlPointR2 Center
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

	public virtual GlVectorR2 DirectVector
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

	public virtual float SIN
	{
		get;
		protected set;
	}

	public virtual float COS
	{
		get;
		protected set;
	}

	public virtual float CenterX
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

	public virtual float CenterY
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

	public virtual GlPointR2 this[int i]
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

	public override IEnumerable<GlPointR2> getIntersection(GlCurve C)
	{
		throw new System.NotImplementedException();
	}

	public override IEnumerable<GlPointR2> getIntersection(GlPolygon POLY)
	{
		throw new System.NotImplementedException();
	}

	public override void Draw()
	{
		throw new System.NotImplementedException();
	}

	public override void Draw(GlRectangle Border)
	{
		throw new System.NotImplementedException();
	}

	public override void Draw(GlTexture T)
	{
		throw new System.NotImplementedException();
	}

	public override void DrawFill()
	{
		throw new System.NotImplementedException();
	}

	public virtual void DrawFill(GlRectangle Boreder)
	{
		throw new System.NotImplementedException();
	}

	private void DrawPoints(GlRectangle Border, int GlDrawMode)
	{
		throw new System.NotImplementedException();
	}

	private void DrawPoints(int GlDrawMode)
	{
		throw new System.NotImplementedException();
	}

	private bool ActivateDrawStart()
	{
		throw new System.NotImplementedException();
	}

	private void ActivateDrawing()
	{
		throw new System.NotImplementedException();
	}

	private void ActivateDrawed()
	{
		throw new System.NotImplementedException();
	}

	public abstract GlLineR2 getTangentFromBelongs(GlPointR2 belongsPoint);

	public virtual GlCircle getCurvatureCircle(GlPointR2 belongsPoint)
	{
		throw new System.NotImplementedException();
	}

	public abstract bool isPointInside(GlPointR2 P);

	public abstract bool getFDiff(float X);

	public abstract int getSDiff(float X);

	protected abstract void updatePointsPosition();

	public override void moveTo(float x, float y)
	{
		throw new System.NotImplementedException();
	}

	public override void Rotate(float angle)
	{
		throw new System.NotImplementedException();
	}

	public override void Rotate(float SIN, float COS)
	{
		throw new System.NotImplementedException();
	}

}

